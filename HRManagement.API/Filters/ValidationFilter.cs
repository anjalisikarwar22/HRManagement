// HRManagement.API/Filters/ValidationFilter.cs

// This filter runs BEFORE the controller action
// It checks if the data sent by user is valid
// If not valid → returns 400 immediately
// Controller code never runs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HRManagement.API.Filters
{
    public class ValidationFilter : IActionFilter
    {
        // Runs BEFORE the action
        public void OnActionExecuting(
            ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                // Cancel the action and return 400
                context.Result =
                    new BadRequestObjectResult(
                        context.ModelState);
            }
        }

        // Runs AFTER the action — nothing needed here
        public void OnActionExecuted(
            ActionExecutedContext context)
        { }
    }
}