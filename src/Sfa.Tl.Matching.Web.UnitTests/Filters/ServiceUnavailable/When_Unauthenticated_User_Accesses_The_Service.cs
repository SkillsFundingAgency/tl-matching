using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NSubstitute;
using Sfa.Tl.Matching.Web.Filters;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Filters.ServiceUnavailable
{
    public class When_Unauthenticated_User_Accesses_The_Service
    {
        private readonly ActionExecutingContext _actionExecutingContext;

        public When_Unauthenticated_User_Accesses_The_Service()
        {
            var defaultHttpContext = Substitute.For<HttpContext>();
            defaultHttpContext.User.Identity.IsAuthenticated.Returns(false);

            var routeData = new RouteData();
            var actionContext = new ActionContext(defaultHttpContext, routeData, new ActionDescriptor());

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