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
    [Route("api/pilots")]
    [ApiController]
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

        [HttpGet]
        public async Task<IActionResult> GetPilots()
        {
            var pilots = await _repository.Pilot.GetAllPilotsAsync(trackChanges: false);
            var pilotsDto = _mapper.Map<IEnumerable<Pilot>>(pilots);
            return Ok(pilotsDto);
        }

        [HttpGet("{id}", Name = "PilotById")]
        public async Task<IActionResult> GetPilotAsync(Guid id)
        {
            var pilot = await _repository.Pilot.GetPilotAsync(id, trackChanges: false);
            if(pilot == null)
            {
                _logger.LogInfo($"Pilot with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var pilotDto = _mapper.Map<PilotDto>(pilot);
            return Ok(pilotDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePilotAsync([FromBody] PilotForCreatonDto pilot) 
        {            
            var pilotEntity = _mapper.Map<Pilot>(pilot);
            _repository.Pilot.CreatePilot(pilotEntity);
            await _repository.SaveAsync();
            var pilotToReturn = _mapper.Map<PilotDto>(pilotEntity);
            return CreatedAtRoute("PilotById", new { id = pilotToReturn.Id }, pilotToReturn);
        }

        [HttpGet("collection/({ids})", Name = "PilotCollection")]
        public async Task<IActionResult> GetPilotCollection(IEnumerable<Guid> ids) 
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var pilotEntities = await _repository.Pilot.GetByIdsAsync(ids, trackChanges: true);
            if (ids.Count() != pilotEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var pilotsToReturn = _mapper.Map<IEnumerable<PilotDto>> (pilotEntities);
            return Ok(pilotsToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreatePilotCollection([ModelBinder (BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> pilotCollection)
        {
            if (pilotCollection == null)
            {
                _logger.LogError("Pilot collection sent from client is null.");
                return BadRequest("Pilot collection is null");
            }
            var pilotEntitiees = _mapper.Map<IEnumerable<Pilot>>(pilotCollection);
            foreach (var pilot in pilotEntitiees)
            {
                _repository.Pilot.CreatePilot(pilot);
            }
            await _repository.SaveAsync();
            var pilotCollectionToReturn = _mapper.Map<IEnumerable<PilotDto>>(pilotEntitiees);
            var ids = string.Join(",", pilotCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("PilotCollection", new { ids }, pilotCollectionToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidatePilotExistsAtribute))]
        public async Task<IActionResult> DeletePilot(Guid id)
        {
            var pilot = HttpContext.Items["pilot"] as Pilot;
            _repository.Pilot.DeletePilot(pilot);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidatePilotExistsAtribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] PilotForUpdateDto pilot)
        {            
            var pilotEntity = HttpContext.Items["pilot"] as Pilot;
            _mapper.Map(pilot, pilotEntity);
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
