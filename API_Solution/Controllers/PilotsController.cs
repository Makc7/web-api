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
    public class PilorsController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public PilorsController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPilors()
        {
            var pilots = await _repository.Pilor.GetAllPilorsAsync(trackChanges: false);
            var pilotsDto = _mapper.Map<IEnumerable<Pilor>>(pilots);
            return Ok(pilotsDto);
        }

        [HttpGet("{id}", Name = "PilorById")]
        public async Task<IActionResult> GetPilorAsync(Guid id)
        {
            var pilot = await _repository.Pilor.GetPilorAsync(id, trackChanges: false);
            if(pilot == null)
            {
                _logger.LogInfo($"Pilor with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var pilotDto = _mapper.Map<PilorDto>(pilot);
            return Ok(pilotDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePilorAsync([FromBody] PilorForCreatonDto pilot) 
        {            
            var pilotEntity = _mapper.Map<Pilor>(pilot);
            _repository.Pilor.CreatePilor(pilotEntity);
            await _repository.SaveAsync();
            var pilotToReturn = _mapper.Map<PilorDto>(pilotEntity);
            return CreatedAtRoute("PilorById", new { id = pilotToReturn.Id }, pilotToReturn);
        }

        [HttpGet("collection/({ids})", Name = "PilorCollection")]
        public async Task<IActionResult> GetPilorCollection(IEnumerable<Guid> ids) 
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var pilotEntities = await _repository.Pilor.GetByIdsAsync(ids, trackChanges: true);
            if (ids.Count() != pilotEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var pilotsToReturn = _mapper.Map<IEnumerable<PilorDto>> (pilotEntities);
            return Ok(pilotsToReturn);
        }

        [HttpPost("collection")]
        public async Task<IActionResult> CreatePilorCollection([ModelBinder (BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> pilotCollection)
        {
            if (pilotCollection == null)
            {
                _logger.LogError("Pilor collection sent from client is null.");
                return BadRequest("Pilor collection is null");
            }
            var pilotEntitiees = _mapper.Map<IEnumerable<Pilor>>(pilotCollection);
            foreach (var pilot in pilotEntitiees)
            {
                _repository.Pilor.CreatePilor(pilot);
            }
            await _repository.SaveAsync();
            var pilotCollectionToReturn = _mapper.Map<IEnumerable<PilorDto>>(pilotEntitiees);
            var ids = string.Join(",", pilotCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("PilorCollection", new { ids }, pilotCollectionToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidatePilorExistsAtribute))]
        public async Task<IActionResult> DeletePilor(Guid id)
        {
            var pilot = HttpContext.Items["pilot"] as Pilor;
            _repository.Pilor.DeletePilor(pilot);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidatePilorExistsAtribute))]
        public async Task<IActionResult> UpdateCompany(Guid id, [FromBody] PilorForUpdateDto pilot)
        {            
            var pilotEntity = HttpContext.Items["pilot"] as Pilor;
            _mapper.Map(pilot, pilotEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetPilorsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
    }
}
