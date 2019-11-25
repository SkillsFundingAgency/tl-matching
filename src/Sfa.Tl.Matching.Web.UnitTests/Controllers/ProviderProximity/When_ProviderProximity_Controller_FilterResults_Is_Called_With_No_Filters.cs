using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderProximity
{
    public class When_ProviderProximity_Controller_FilterResults_Is_Called_With_No_Filters
    {
        private readonly IActionResult _result;

        public When_ProviderProximity_Controller_FilterResults_Is_Called_With_No_Filters()
        {
            var routePathService = Substitute.For<IRoutePathService>();
            var providerProximityService = Substitute.For<IProviderProximityService>();
            var locationService = Substitute.For<ILocationService>();

            var providerProximityController = new ProviderProximityController(routePathService, providerProximityService
                , locationService);

            var viewModel = new ProviderProximitySearchParametersViewModel
            {
                Postcode = "CV12WT"
            };

            _result = providerProximityController.FilterResultsAsync(viewModel);
        }

        [Fact]
        public void Then_RedirectToRoute_Result_Is_Returned()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<RedirectToRouteResult>();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().BeEquivalentTo("GetProviderProximityResults");

            redirect?.RouteValues["searchCriteria"].Should().Be("CV12WT");
        }
    }
}