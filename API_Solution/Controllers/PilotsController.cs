using API_Solution.ActionFilters;
using API_Solution.ModelBinders;
using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Solution.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/drivers")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "v1")]

    public class PilotsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public PilotsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        /// <summary>
        /// Получает список всех водителей
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetPilots()
        {
            var drivers = await _repository.Pilot.GetAllPilotsAsync(trackChanges: false);
            var driversDto = _mapper.Map<IEnumerable<Pilot>>(drivers);
            return Ok(driversDto);
        }

        /// <summary>
        /// Получает водителя по Id
        /// </summary>
        /// <param name="id">Id водителя</param>
        /// <returns></returns>
        [HttpGet("{id}", Name = "PilotById")]
        public async Task<IActionResult> GetPilotAsync(Guid id)
        {
            var driver = await _repository.Pilot.GetPilotAsync(id, trackChanges: false);
            if(driver == null)
            {
                _logger.LogInfo($"Pilot with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var driverDto = _mapper.Map<PilotDto>(driver);
            return Ok(driverDto);
        }

        /// <summary>
        /// Создает водителя
        /// </summary>
        /// <param name="driver">Экземпляр нового водителя</param>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePilotAsync([FromBody] PilotForCreatonDto driver) 
        {            
            var driverEntity = _mapper.Map<Pilot>(driver);
            _repository.Pilot.CreatePilot(driverEntity);
            await _repository.SaveAsync();
            var driverToReturn = _mapper.Map<PilotDto>(driverEntity);
            return CreatedAtRoute("PilotById", new { id = driverToReturn.Id }, driverToReturn);
        }

        /// <summary>
        /// Получает список водителей по их Id
        /// </summary>
        /// <param name="ids">Id водителей которых хотим получить</param>
        /// <returns></returns>
        [HttpGet("collection/({ids})", Name = "PilotCollection")]
        public async Task<IActionResult> GetPilotCollection(IEnumerable<Guid> ids) 
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var driverEntities = await _repository.Pilot.GetByIdsAsync(ids, trackChanges: true);
            if (ids.Count() != driverEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var driversToReturn = _mapper.Map<IEnumerable<PilotDto>> (driverEntities);
            return Ok(driversToReturn);
        }

        /// <summary>
        /// Создает список водителей
        /// </summary>
        /// <param name="driverCollection">Коллекция новых водителей</param>
        /// <returns></returns>
        [HttpPost("collection")]
        public async Task<IActionResult> CreatePilotCollection([ModelBinder (BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> driverCollection)
        {
            if (driverCollection == null)
            {
                _logger.LogError("Pilot collection sent from client is null.");
                return BadRequest("Pilot collection is null");
            }
            var driverEntitiees = _mapper.Map<IEnumerable<Pilot>>(driverCollection);
            foreach (var driver in driverEntitiees)
            {
                _repository.Pilot.CreatePilot(driver);
            }
            await _repository.SaveAsync();
            var driverCollectionToReturn = _mapper.Map<IEnumerable<PilotDto>>(driverEntitiees);
            var ids = string.Join(",", driverCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("PilotCollection", new { ids }, driverCollectionToReturn);
        }

        /// <summary>
        /// Удаляет водителя по Id
        /// </summary>
        /// <param name="id">Id водителя которого удаляем</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidatePilotExistsAtribute))]
        public async Task<IActionResult> DeletePilot(Guid id)
        {
            var driver = HttpContext.Items["driver"] as Pilot;
            _repository.Pilot.DeletePilot(driver);
            await _repository.SaveAsync();
            return NoContent();
        }

        /// <summary>
        /// Редактирует водителя по Id
        /// </summary>
        /// <param name="id">Id водителя которого редактируем</param>
        /// <param name="driver">Экземпляр редактированного водителя</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidatePilotExistsAtribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] PilotForUpdateDto driver)
        {            
            var driverEntity = HttpContext.Items["driver"] as Pilot;
            _mapper.Map(driver, driverEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetPilotsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
    }
}
