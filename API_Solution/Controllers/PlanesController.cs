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
    [Route("api/pilots/{pilotId}/planes")]
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

        [HttpGet]
        [HttpHead]
        public async Task<ActionResult> GetPlanesWithHelpPilor(Guid pilotId, [FromQuery] PlaneParameters planeParameters)
        {
            var pilot = await _repository.Pilor.GetPilorAsync(pilotId, trackChanges: false);
            if(pilot == null)
            {
                _logger.LogInfo($"Pilor with id: {pilotId} doesn't exist in the database.");
                return NotFound();
            }
            var planesFromDB = await _repository.Plane.GetPlanesAsync(pilotId, planeParameters, trackChanges: false);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(planesFromDB.MetaData));
            var planesDto = _mapper.Map<IEnumerable<PlaneDto>>(planesFromDB);
            return Ok(_dataShaper.ShapeData(planesDto, planeParameters.Fields));
        }

        [HttpGet("{id}", Name = "GetPlaneForPilor")]
        public async Task<ActionResult> GetPlaneWithHelpPilor(Guid pilotId, Guid id)
        {
            var pilot = await _repository.Pilor.GetPilorAsync(pilotId, trackChanges: false);
            if (pilot == null)
            {
                _logger.LogInfo($"Pilor with id: {pilotId} doesn't exist in the database.");
                return NotFound();
            }
            var planeDB = await _repository.Plane.GetPlaneByIdAsync(pilotId, id, trackChanges: false);
            if(planeDB == null)
            {
                _logger.LogInfo($"Plane with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var planeDto = _mapper.Map<PlaneDto>(planeDB);
            return Ok(planeDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePlaneForPilorAsync(Guid pilotId, [FromBody] PlaneForCreationDto plane)
        {           
            var pilot = await _repository.Pilor.GetPilorAsync(pilotId, trackChanges: false);
            if(pilot == null)
            {
                _logger.LogInfo($"Pilor with id: {pilotId} doesn't exist in the database.");
                return NotFound();
            }
            var planeEntity = _mapper.Map<Plane>(plane);
            _repository.Plane.CreatePlaneForPilor(pilotId,planeEntity);
            await _repository.SaveAsync();
            var planeToReturn = _mapper.Map<PlaneDto>(planeEntity);
            return CreatedAtRoute("GetPlaneForPilor", new { pilotId, id = planeToReturn.Id }, planeToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidatePlaneForPilorExistsAttribute))]
        public async Task<IActionResult> DeletePlaneForPilor(Guid pilotId, Guid id) 
        {
            var planeForPilor = HttpContext.Items["plane"] as Plane;            
            _repository.Plane.DeletePlane(planeForPilor);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidatePlaneForPilorExistsAttribute))]
        public async Task<IActionResult> UpdatePlaneForPilor(Guid pilotId, Guid id, [FromBody] PlaneForUpdateDto plane)
        {   
            var planeEntity = HttpContext.Items["plane"] as Plane;            
            _mapper.Map(plane, planeEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPatch("{id}")]
        [ServiceFilter(typeof(ValidatePlaneForPilorExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdatePlaneForPilor(Guid pilotId, Guid id, [FromBody] JsonPatchDocument<PlaneForUpdateDto> patchDoc)
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
