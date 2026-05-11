using Microsoft.AspNetCore.Mvc.Filters;

namespace HRManagement.API.Filters
{
    public class DepartmentHeaderFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(
            ResultExecutingContext context,
            ResultExecutionDelegate next)
        {
            context.HttpContext.Response.Headers["X-Controller"] = "Departments";
            await next();
        }
    }
}
