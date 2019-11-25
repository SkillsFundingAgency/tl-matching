using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderProximity
{
    public class When_ProviderProximity_Controller_Finish_Is_Called
    {
        private readonly IActionResult _result;

        public When_ProviderProximity_Controller_Finish_Is_Called()
        {
            var locationService = Substitute.For<ILocationService>();
            var routePathService = Substitute.For<IRoutePathService>();
            var providerProximityService = Substitute.For<IProviderProximityService>();

            var providerProximityController = new ProviderProximityController(routePathService, 
                providerProximityService, locationService);

            _result = providerProximityController.Finish();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<RedirectToRouteResult>();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect.RouteName.Should().BeEquivalentTo("Start");
        }
    }
}