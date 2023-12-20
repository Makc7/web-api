using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API_Solution.Controllers
{
    [Route("api/pilots")]
    [ApiController]
    public class PilorsV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        public PilorsV2Controller(IRepositoryManager repository)
        {
            _repository = repository;
        }
        [HttpGet]
        public async Task<IActionResult> GetPilors()
        {
            var pilots = await _repository.Pilor.GetAllPilorsAsync(trackChanges: false);
            return Ok(pilots);
        }
    }
}
