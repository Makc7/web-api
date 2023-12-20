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
        public IActionResult GetPilots()
        {
            var pilots = _repository.Pilot.GetAllPilots(trackChanges: false);
            var pilotsDto = _mapper.Map<IEnumerable<Pilot>>(pilots);
            return Ok(pilotsDto);
        }

        [HttpGet("{id}")]
        public IActionResult GetPilot(Guid id)
        {
            var pilot =_repository.Pilot.GetPilot(id, trackChanges: false);
            if(pilot == null)
            {
                _logger.LogInfo($"Pilot with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var pilotDto = _mapper.Map<PilotDto>(pilot);
            return Ok(pilotDto);
        }
    }
}
