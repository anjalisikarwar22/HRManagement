
﻿// HRManagement.API/Filters/LogActionFilter.cs

using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace HRManagement.API.Filters
{
    public class LogActionFilter(
        ILogger<LogActionFilter> logger) : IActionFilter
    {
        private Stopwatch? _stopwatch;

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
