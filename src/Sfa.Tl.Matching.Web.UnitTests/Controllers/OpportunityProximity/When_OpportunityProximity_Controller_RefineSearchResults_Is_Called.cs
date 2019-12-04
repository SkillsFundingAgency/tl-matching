using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.OpportunityProximity
{
    public class When_ProviderProximity_Controller_RefineSearchResults_Is_Called
    {
        private readonly IActionResult _result;
        private readonly IRoutePathService _routeService;

        public When_ProviderProximity_Controller_RefineSearchResults_Is_Called()
        {
            var locationService = Substitute.For<ILocationService>();
            locationService.IsValidPostcodeAsync(Arg.Any<string>()).Returns((true, "CV1 2WT"));

            var opportunityProximityService = Substitute.For<IOpportunityProximityService>();

            _routeService = Substitute.For<IRoutePathService>();
            _routeService.GetRouteIdsAsync().Returns(new List<int> { 1, 2 });

            var opportunityService = Substitute.For<IOpportunityService>();

            var opportunityProximityController = new OpportunityProximityController(_routeService, opportunityProximityService, opportunityService,
                locationService);

            var viewModel = new SearchParametersViewModel
            {
                Postcode = "CV12WT",
                SelectedRouteId = 1,
                CompanyNameWithAka = "CompanyName (AlsoKnownAs)"
            };

            _result = opportunityProximityController.RefineSearchResultsAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_RedirectToRoute_Result_Is_Returned()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<RedirectToRouteResult>();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().BeEquivalentTo("GetOpportunityProviderResults");

            redirect?.RouteValues["SelectedRouteId"].Should().Be(1);
            redirect?.RouteValues["Postcode"].Should().Be("CV1 2WT");
            redirect?.RouteValues["OpportunityId"].Should().Be(0);
            redirect?.RouteValues["OpportunityItemId"].Should().Be(0);
            redirect?.RouteValues["CompanyNameWithAka"].Should().Be("CompanyName (AlsoKnownAs)");
        }

        [Fact]
        public void Then_RouteService_GetRouteIdsAsync_Is_Called_exactly_Once()
        {
            _routeService.Received(1).GetRouteIdsAsync();
        }
    }
}