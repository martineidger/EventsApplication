using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Events.Authentications.CustomAuthorization
{
    public class IsUsersResourceFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var userIdFromRoute = (int)context.ActionArguments["userId"];
            var currentUserId = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (context.HttpContext.User.IsInRole("Admin"))
            {
                return; 
            }

            if (currentUserId == null || currentUserId != userIdFromRoute.ToString())
            {
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // Логика после выполнения действия (если нужна)
        }
    }
}

