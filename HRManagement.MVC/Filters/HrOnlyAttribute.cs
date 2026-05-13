using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HRManagement.MVC.Filters;

public class HrOnlyAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.User.Identity?.IsAuthenticated == true &&
            !context.HttpContext.User.IsInRole("Admin"))
        {
            context.Result = new ForbidResult();
        }
    }
}
