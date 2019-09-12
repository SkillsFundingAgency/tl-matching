using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.Filters
{
    public class ServiceUnavailableFilterAttribute : IActionFilter
    {
        private readonly IMaintenanceHistoryService _maintenanceHistoryService;

        public ServiceUnavailableFilterAttribute(IMaintenanceHistoryService maintenanceHistoryService)
        {
            _maintenanceHistoryService = maintenanceHistoryService;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated) return;
            if (IsSignOut(context)) return;
            if (IsServiceUnavailable(context)) return;

            var maintenanceHistory = _maintenanceHistoryService.GetLatestMaintenanceHistory().GetAwaiter().GetResult();
            if (maintenanceHistory.IsOnline) return;

            if (context.HttpContext.User.HasAdminRole())
            {
                AttachIsOnlineTo(context);
                return;
            }

            var routeValues = new RouteValueDictionary
            {
                { "controller", "Home" },
                { "action", nameof(HomeController.ServiceUnavailable) }
            };

            context.Result = new RedirectToRouteResult(routeValues);
            context.Result.ExecuteResultAsync(context);
        }

        private static void AttachIsOnlineTo(ActionContext context)
        {
            if (context.HttpContext.Items.ContainsKey("IsOnline"))
                context.HttpContext.Items["IsOnline"] = false;
            else
                context.HttpContext.Items.Add("IsOnline", false);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        private static bool IsSignOut(ActionContext context)
        {
            return context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor
                   && controllerActionDescriptor.ControllerName == "Account"
                   && controllerActionDescriptor.ActionName == nameof(AccountController.SignOut);
        }

        private static bool IsServiceUnavailable(ActionContext context)
        {
            return context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor
                   && controllerActionDescriptor.ControllerName == "Home"
                   && controllerActionDescriptor.ActionName == nameof(HomeController.ServiceUnavailable);
        }
    }
}