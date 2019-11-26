using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderProximity
{
    public class When_ProviderProximity_Controller_FilterResults_Is_Called_With_Multiple_Filters
    {
        private readonly IActionResult _result;
        private const string SearchCriteria = "CV12WT-Agriculture, environmental and animal care-Business and administration";

        public When_ProviderProximity_Controller_FilterResults_Is_Called_With_Multiple_Filters()
        {
            var routePathService = Substitute.For<IRoutePathService>();
            var providerProximityService = Substitute.For<IProviderProximityService>();
            var locationService = Substitute.For<ILocationService>();

            var providerProximityController = new ProviderProximityController(routePathService, providerProximityService,
                locationService);

            var routes = new List<string>
            {
                "Agriculture, environmental and animal care",
                "Business and administration"
            };

            var viewModel = new ProviderProximitySearchParametersViewModel(SearchCriteria, routes);

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
            redirect?.RouteValues["searchCriteria"].Should().Be(SearchCriteria);
        }
    }
}