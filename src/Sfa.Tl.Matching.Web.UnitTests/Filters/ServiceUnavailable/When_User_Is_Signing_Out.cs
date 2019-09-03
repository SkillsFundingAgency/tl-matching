using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Filters;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Filters.ServiceUnavailable
{
    public class When_User_Is_Signing_Out
    {
        private readonly ActionExecutingContext _actionExecutingContext;

        public When_User_Is_Signing_Out()
        {
            var claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.Role, RolesExtensions.StandardUser)
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(claimsList, "AuthenticationType"));

            var defaultHttpContext = Substitute.For<HttpContext>();
            defaultHttpContext.User = user;

            var routeData = new RouteData();
            routeData.Values.Add("controller", "Account");
            routeData.Values.Add("action", nameof(AccountController.SignOut));

            var controllerActionDescriptor = new ControllerActionDescriptor
            {
                ControllerName = "Account",
                ActionName = nameof(AccountController.SignOut)
            };

            var actionContext = new ActionContext(defaultHttpContext, routeData, controllerActionDescriptor);

            _actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                null);

            var filterAttribute = new ServiceUnavailableFilterAttribute();

            filterAttribute.OnActionExecuting(_actionExecutingContext);
        }

        [Fact]
        public void Then_User_Does_Not_See_Service_Unavailable_Page()
        {
            _actionExecutingContext.Result.Should().BeNull();
        }
    }
}