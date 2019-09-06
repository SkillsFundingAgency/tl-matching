using System.Collections.Generic;
using System.Security.Claims;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Filters;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Filters.ServiceUnavailable
{
    public class When_Admin_User_Accesses_The_Service
    {
        private readonly ActionExecutingContext _actionExecutingContext;

        public When_Admin_User_Accesses_The_Service()
        {
            var claimsList = new List<Claim>
            {
                new Claim(ClaimTypes.Role, RolesExtensions.AdminUser)
            };

            var user = new ClaimsPrincipal(new ClaimsIdentity(claimsList, "AuthenticationType"));

            var defaultHttpContext = Substitute.For<HttpContext>();
            defaultHttpContext.User = user;

            var routeData = new RouteData();
            var actionContext = new ActionContext(defaultHttpContext, routeData, new ActionDescriptor());

            _actionExecutingContext = new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                null);

            var maintenanceHistoryService = Substitute.For<IMaintenanceHistoryService>();
            var filterAttribute = new ServiceUnavailableFilterAttribute(maintenanceHistoryService);

            filterAttribute.OnActionExecuting(_actionExecutingContext);
        }

        [Fact]
        public void Then_User_Does_Not_See_Service_Unavailable_Page()
        {
            _actionExecutingContext.Result.Should().BeNull();
        }
    }
}