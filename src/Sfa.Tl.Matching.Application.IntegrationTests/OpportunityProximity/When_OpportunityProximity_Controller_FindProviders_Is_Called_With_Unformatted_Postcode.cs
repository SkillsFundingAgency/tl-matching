using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_FindProviders_Is_Called_With_Unformatted_Postcode : IClassFixture<OpportunityProximityControllerFixture>
    {
        private readonly OpportunityProximityControllerFixture _fixture;
        private readonly IActionResult _result;

        public When_OpportunityProximity_Controller_FindProviders_Is_Called_With_Unformatted_Postcode(OpportunityProximityControllerFixture fixture)
        {
            _fixture = fixture;
            const string requestPostcode = "cV12 Wt";
            var routes = new List<SelectListItem>
            {
                new SelectListItem {Text = "1", Value = "Route 1"},
                new SelectListItem {Text = "2", Value = "Route 2"}
            };
            
            fixture.GetOpportunityProximityController(requestPostcode);
            fixture.RoutePathService.GetRouteIdsAsync().Returns(new List<int> { 1, 2 });

            var viewModel = new SearchParametersViewModel
            {
                RoutesSelectList = routes,
                SelectedRouteId = int.Parse(routes.First().Text),
                Postcode = requestPostcode
            };

            _result = fixture.OpportunityProximityController.FindProviders(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Is_Redirect_To_Results()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<RedirectToRouteResult>();
            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteName.Should().BeEquivalentTo("GetOpportunityProviderResults");

            redirect?.RouteValues["Postcode"].Should().Be("CV1 2WT");
            redirect?.RouteValues["SelectedRouteId"].Should().Be(1);
        }

        [Fact]
        public void Then_RouteService_GetRouteIdsAsync_Is_Called_exactly_Once()
        {
            _fixture.RoutePathService.Received(1).GetRouteIdsAsync();
        }
    }
}