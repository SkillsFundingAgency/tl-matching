using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.OpportunityProximity
{
    public class When_ProviderProximity_Controller_RefineSearchResults_Is_Called : IClassFixture<OpportunityProximityControllerFixture>
    {
        private readonly OpportunityProximityControllerFixture _fixture;
        private readonly IActionResult _result;
        
        public When_ProviderProximity_Controller_RefineSearchResults_Is_Called()
        {
            _fixture = new OpportunityProximityControllerFixture();

            _fixture.LocationService.IsValidPostcodeAsync(Arg.Any<string>()).Returns((true, "CV1 2WT"));
            _fixture.RouteService.GetRouteIdsAsync().Returns(new List<int> { 1, 2 });

            var viewModel = new SearchParametersViewModel
            {
                Postcode = "CV12WT",
                SelectedRouteId = 1,
                CompanyNameWithAka = "CompanyName (AlsoKnownAs)"
            };

            _result = _fixture.Sut.RefineSearchResultsAsync(viewModel).GetAwaiter().GetResult();
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
            _fixture.RouteService.Received(1).GetRouteIdsAsync();
        }
    }
}