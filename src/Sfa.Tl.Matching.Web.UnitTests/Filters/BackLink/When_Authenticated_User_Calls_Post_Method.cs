using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Filters;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Filters.BackLink
{
    public class When_Authenticated_User_Calls_Post_Method
    {
        private readonly INavigationService _navigationService;

        public When_Authenticated_User_Calls_Post_Method()
        {
            var defaultHttpContext = Substitute.For<HttpContext>();

            defaultHttpContext.User.Identity!.IsAuthenticated.Returns(true);
            defaultHttpContext.Request.Method.Returns("POST");

            var routeData = new RouteData();
            var actionContext = new ActionContext(defaultHttpContext, routeData, new ActionDescriptor());

            var actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                null!);

            var logger = Substitute.For<ILogger<BackLinkFilter>>();
            _navigationService = Substitute.For<INavigationService>();
            var filter = new BackLinkFilter(logger, _navigationService);

            var actionExecutionDelegate = Substitute.For<ActionExecutionDelegate>();

            filter.OnActionExecutionAsync(actionExecutingContext, actionExecutionDelegate).GetAwaiter().GetResult();
        }

        [Fact]
        public async Task Then_NavigationService_AddCurrentUrlAsync_Is_Not_Called()
        {
            await _navigationService
                .DidNotReceive()
                .AddCurrentUrlAsync(Arg.Any<string>(), Arg.Any<string>());
        }
    }
}