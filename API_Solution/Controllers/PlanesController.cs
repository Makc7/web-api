using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API_Solution.ActionFilters;
using API_Solution.ModelBinders;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace API_Solution.Controllers
{
    [Route("api/drivers/{driverId}/planes")]
    [ApiController]
    public class PlanesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<PlaneDto> _dataShaper;

        public PlanesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<PlaneDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }

        /// <summary>
        /// Получает список всех машин для определенного водителя
        /// </summary>
        /// <param name="driverId">Id водителя</param>
        /// <param name="planeParameters">Параметры для частичных результатов запроса</param>
        /// <returns></returns>
        [HttpGet]
        [HttpHead]
        public async Task<ActionResult> GetPlanesWithHelpPilot(Guid driverId, [FromQuery] PlaneParameters planeParameters)
        {
            var driver = await _repository.Pilot.GetPilotAsync(driverId, trackChanges: false);
            if(driver == null)
            {
                _logger.LogInfo($"Pilot with id: {driverId} doesn't exist in the database.");
                return NotFound();
            }
            var planesFromDB = await _repository.Plane.GetPlanesAsync(driverId, planeParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(planesFromDB.MetaData));
            var planesDto = _mapper.Map<IEnumerable<PlaneDto>>(planesFromDB);
            return Ok(_dataShaper.ShapeData(planesDto, planeParameters.Fields));
        }

        /// <summary>
        /// Получает определенную машину для определенного водителя
        /// </summary>
        /// <param name="driverId">Id водителя</param>
        /// <param name="id">Id машины которую хотим получить</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "GetPlaneForPilot")]
        public async Task<ActionResult> GetPlaneWithHelpPilot(Guid driverId, Guid id)
        {
            var driver = await _repository.Pilot.GetPilotAsync(driverId, trackChanges: false);
            if (driver == null)
            {
                _logger.LogInfo($"Pilot with id: {driverId} doesn't exist in the database.");
                return NotFound();
            }
            var planeDB = await _repository.Plane.GetPlaneByIdAsync(driverId, id, trackChanges: false);
            if(planeDB == null)
            {
                _logger.LogInfo($"Plane with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var planeDto = _mapper.Map<PlaneDto>(planeDB);
            return Ok(planeDto);
        }

        /// <summary>
        /// Создает машину для определенного водителя
        /// </summary>
        /// <param name="driverId">Id водителя</param>
        /// <param name="plane">"Экземпляр новой машины</param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePlaneForPilotAsync(Guid driverId, [FromBody] PlaneForCreationDto plane)
        {           
            var driver = await _repository.Pilot.GetPilotAsync(driverId, trackChanges: false);
            if(driver == null)
            {
                _logger.LogInfo($"Pilot with id: {driverId} doesn't exist in the database.");
                return NotFound();
            }
            var planeEntity = _mapper.Map<Plane>(plane);
            _repository.Plane.CreatePlaneForPilot(driverId,planeEntity);
            await _repository.SaveAsync();
            var planeToReturn = _mapper.Map<PlaneDto>(planeEntity);
            return CreatedAtRoute("GetPlaneForPilot", new { driverId, id = planeToReturn.Id }, planeToReturn);
        }

        /// <summary>
        /// Удаляет определенную машину для определенного водителя
        /// </summary>
        /// <param name="driverId">Id водителя</param>
        /// <param name="id">Id машины которую хотим удалить</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidatePlaneForPilotExistsAttribute))]
        public async Task<IActionResult> DeletePlaneForPilot(Guid driverId, Guid id) 
        {
            var planeForPilot = HttpContext.Items["plane"] as Plane;            
            _repository.Plane.DeletePlane(planeForPilot);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Редактирует определенную машину для определенного водителя
        /// </summary>
        /// <param name="driverId">Id водителя</param>
        /// <param name="id">Id машины которую редактируем</param>
        /// <param name="plane">Экземпляр редактированной машины</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidatePlaneForPilotExistsAttribute))]
        public async Task<IActionResult> UpdatePlaneForPilot(Guid driverId, Guid id, [FromBody] PlaneForUpdateDto plane)
        {   
            var planeEntity = HttpContext.Items["plane"] as Plane;            
            _mapper.Map(plane, planeEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Редактирует определенную машину для определенного водителя
        /// </summary>
        /// <param name="driverId">Id водителя</param>
        /// <param name="id">Id машины которую редактируем</param>
        /// <param name="patchDoc">Параметры для patch запроса</param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidatePlaneForPilotExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdatePlaneForPilot(Guid driverId, Guid id, [FromBody] JsonPatchDocument<PlaneForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }           
            var planeEntity = HttpContext.Items["plane"] as Plane;            
            var planeToPatch = _mapper.Map<PlaneForUpdateDto>(planeEntity);
            patchDoc.ApplyTo(planeToPatch, ModelState);
            TryValidateModel(planeToPatch);
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the PlaneForCreationDto object");
                return UnprocessableEntity(ModelState);
            }
            _mapper.Map(planeToPatch, planeEntity);
            await _repository.SaveAsync();
            return NoContent(); 
        }
    }
}
