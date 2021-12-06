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
    public class ServiceUnavailableFilter : IAsyncActionFilter
    {
        private readonly IServiceStatusHistoryService _serviceStatusHistoryService;

        public ServiceUnavailableFilter(IServiceStatusHistoryService serviceStatusHistoryService)
        {
            _serviceStatusHistoryService = serviceStatusHistoryService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.User.Identity.IsAuthenticated)
            {
                await next();
                return;
            }

            if (IsSignOut(context))
            {
                await next();
                return;
            }

            if (IsServiceUnavailable(context))
            {
                await next();
                return;
            }

            var serviceStatusHistory = await _serviceStatusHistoryService.GetLatestServiceStatusHistoryAsync();
            if (serviceStatusHistory.IsOnline)
            {
                await next();
                return;
            }

            if (context.HttpContext.User.HasAdminRole())
            {
                AttachIsOnlineTo(context);
                await next();
                return;
            }

            var routeValues = new RouteValueDictionary
            {
                { "controller", "Home" },
                { "action", nameof(HomeController.ServiceUnavailable) }
            };

            context.Result = new RedirectToRouteResult(routeValues);
            await context.Result.ExecuteResultAsync(context);
        }

        private static void AttachIsOnlineTo(ActionContext context)
        {
            if (context.HttpContext.Items.ContainsKey("IsOnline"))
                context.HttpContext.Items["IsOnline"] = false;
            else
                context.HttpContext.Items.Add("IsOnline", false);
        }

        private static bool IsSignOut(ActionContext context)
        {
            return context.ActionDescriptor is ControllerActionDescriptor
            {
                ControllerName: "Account", ActionName: nameof(AccountController.SignOut)
            };
        }

        private static bool IsServiceUnavailable(ActionContext context)
        {
            return context.ActionDescriptor is ControllerActionDescriptor
            {
                ControllerName: "Home", ActionName: nameof(HomeController.ServiceUnavailable)
            };
        }
    }
}