using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Web.Filters
{
    public class BackLinkFilter : IAsyncActionFilter
    {
        private readonly ILogger<BackLinkFilter> _logger;
        private readonly INavigationService _navigationService;

        public BackLinkFilter(ILogger<BackLinkFilter> logger, INavigationService navigationService)
        {
            _logger = logger;
            _navigationService = navigationService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                if (!context.HttpContext.User.Identity.IsAuthenticated)
                {
                    await next();
                    return;
                }

                if (context.HttpContext.Request.Method == "GET")
                {
                    var path = context.HttpContext.Request.Path.ToString();
                    var username = context.HttpContext.User.GetUserName();

                    await _navigationService.AddCurrentUrlAsync(path, username);
                }

                await next();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, exception.Message);
            }
        }
    }
}
