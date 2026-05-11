// HRManagement.API/Filters/LogActionFilter.cs

// This filter logs every request
// It writes to console BEFORE and AFTER each action
// So you can see which endpoints are being called
// and how long they take

using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace HRManagement.API.Filters
{
    public class LogActionFilter(
        ILogger<LogActionFilter> logger) : IActionFilter
    {
        // Nullable because it starts null
        // and gets assigned in OnActionExecuting
        private Stopwatch? _stopwatch;

        // Runs BEFORE the action — start the timer
        public void OnActionExecuting(
            ActionExecutingContext context)
        {
            _stopwatch = Stopwatch.StartNew();

            var controller = context
                .RouteData.Values["controller"];
            var action = context
                .RouteData.Values["action"];

            logger.LogInformation(
                "STARTED: {Controller}/{Action}",
                controller, action);
        }

        // Runs AFTER the action — stop timer and log
        public void OnActionExecuted(
            ActionExecutedContext context)
        {
            _stopwatch?.Stop();

            var controller = context
                .RouteData.Values["controller"];
            var action = context
                .RouteData.Values["action"];

            logger.LogInformation(
                "FINISHED: {Controller}/{Action} in {Ms}ms",
                controller, action,
                _stopwatch?.ElapsedMilliseconds ?? 0);
        }
    }
}