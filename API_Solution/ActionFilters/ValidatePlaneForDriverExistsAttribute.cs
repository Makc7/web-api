using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API_Solution.ActionFilters
{
    public class ValidatePlaneForPilotExistsAttribute: IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;

        public ValidatePlaneForPilotExistsAttribute(ILoggerManager logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;
            var driverId = (Guid)context.ActionArguments["driverId"];
            var driver = await _repository.Pilot.GetPilotAsync(driverId, false);
            if (driver == null)
            {
                _logger.LogInfo($"Company with id: {driverId} doesn't exist in the database.");
                return;
                context.Result = new NotFoundResult();
            }
            var id = (Guid)context.ActionArguments["id"];
            var plane = await _repository.Plane.GetPlaneByIdAsync(driverId, id, trackChanges);
            if (plane == null)
            {
                _logger.LogInfo($"Plane with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("plane", plane);
                await next();
            }
        }
    }
}
