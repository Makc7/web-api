using Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API_Solution.ActionFilters
{
    public class ValidatePilotExistsAtribute: IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryManager _repository;

        public ValidatePilotExistsAtribute(ILoggerManager logger, IRepositoryManager repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var trackChanges = context.HttpContext.Request.Method.Equals("PUT");
            var id = (Guid)context.ActionArguments["id"];
            var pilot = await _repository.Pilot.GetPilotAsync(id, trackChanges);

            if (pilot == null)
            {
                _logger.LogInfo($"Pilot with id: {id} doesn't exist in the database.");

                context.Result = new NotFoundResult();
            }
            else
            {
                context.HttpContext.Items.Add("pilot", pilot);
                await next();
            }
        }
    }
}
