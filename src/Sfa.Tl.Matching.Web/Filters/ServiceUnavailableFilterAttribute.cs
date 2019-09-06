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

        public async void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated) return;
            if (context.HttpContext.User.HasAdminRole()) return;

            if (IsSignOut(context)) return;
            
            var maintenanceHistory = await _maintenanceHistoryService.GetLatestMaintenanceHistory();
            if (maintenanceHistory.IsOnline) return;

            if (IsServiceUnavailable(context)) return;

            var routeValues = new RouteValueDictionary
            {
                { "controller", "Home" },
                { "action", nameof(HomeController.ServiceUnavailable) }
            };

            context.Result = new RedirectToRouteResult(routeValues);
            await context.Result.ExecuteResultAsync(context);
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