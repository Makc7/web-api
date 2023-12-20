using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Solution.Controllers
{
    [Route("api/pilots/{pilotId}/plans")]
    [ApiController]
    public class PlanesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        public PlanesController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetPlanesWithHelpPilot(Guid pilotId)
        {
            var driver = _repository.Pilot.GetPilot(pilotId, trackChanges: false);
            if(driver == null)
            {
                _logger.LogInfo($"Pilot with id: {pilotId} doesn't exist in the database.");
                return NotFound();
            }
            var plansFromDB = _repository.Plane.GetPlanes(pilotId, trackChanges: false);
            var plansDto = _mapper.Map<IEnumerable<PlaneDto>>(plansFromDB);
            return Ok(plansDto);
        }

        [HttpGet("{id}")]
        public ActionResult GetPlaneWithHelpPilot(Guid pilotId, Guid id)
        {
            var driver = _repository.Pilot.GetPilot(pilotId, trackChanges: false);
            if (driver == null)
            {
                _logger.LogInfo($"Pilot with id: {pilotId} doesn't exist in the database.");
                return NotFound();
            }
            var plansDB = _repository.Plane.GetPlaneById(pilotId,id, trackChanges: false);
            if(plansDB == null)
            {
                _logger.LogInfo($"Plane with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            var plansDto = _mapper.Map<PlaneDto>(plansDB);
            return Ok(plansDto);
        }
    }
}
