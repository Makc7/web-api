using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API_Solution.ActionFilters
{
    public class ValidatePlaneForPiotExistsAttribute: IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;

        public ValidatePlaneForPiotExistsAttribute(ILoggerManager logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var method = context.HttpContext.Request.Method;
            var trackChanges = (method.Equals("PUT") || method.Equals("PATCH")) ? true : false;
            var pilotId = (Guid)context.ActionArguments["pilotId"];
            var pilot = await _repository.Piot.GetPiotAsync(pilotId, false);
            if (pilot == null)
            {
                _logger.LogInfo($"Company with id: {pilotId} doesn't exist in the database.");
                return;
                context.Result = new NotFoundResult();
            }
            var id = (Guid)context.ActionArguments["id"];
            var car = await _repository.Plane.GetPlaneByIdAsync(pilotId, id, trackChanges);
            if (car == null)
            {
                _logger.LogInfo($"Plane with id: {id} doesn't exist in the database.");
                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("car", car);
                await next();
            }
        }
    }
}
