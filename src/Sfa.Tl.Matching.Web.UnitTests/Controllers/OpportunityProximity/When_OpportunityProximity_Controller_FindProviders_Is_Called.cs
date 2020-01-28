using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.UnitTests.Fixtures;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_FindProviders_Is_Called : IClassFixture<OpportunityProximityControllerFixture>
    {
        private readonly OpportunityProximityControllerFixture _fixture;
        private readonly IActionResult _result;
        
        public When_OpportunityProximity_Controller_FindProviders_Is_Called()
        {
            _fixture = new OpportunityProximityControllerFixture();
            _fixture.LocationService.IsValidPostcodeAsync(Arg.Any<string>()).Returns((true, "CV1 2WT"));
            _fixture.RouteService.GetRouteIdsAsync().Returns(new List<int> { 1, 2 });

            const string postcode = "SW1A 2AA";

            var viewModel = new SearchParametersViewModel
            {
                RoutesSelectList = new List<SelectListItem>
                {
                    new SelectListItem { Text = "1", Value = "Route 1" },
                    new SelectListItem { Text = "2", Value = "Route 2" }
                },
                SelectedRouteId = 1,
                Postcode = postcode
            };
            
            _result = _fixture.Sut.FindProviders(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
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
            redirect?.RouteValues["CompanyNameWithAka"].Should().BeNull();
        }

        [Fact]
        public void Then_RouteService_GetRouteIdsAsync_Is_Called_exactly_Once()
        {
            _fixture.RouteService.Received(1).GetRouteIdsAsync();
        }
    }
}