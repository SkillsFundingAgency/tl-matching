using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_RefineSearchResults_Is_Called_With_Unformatted_Postcode : IClassFixture<OpportunityProximityControllerFixture>
    {
        private readonly OpportunityProximityControllerFixture _fixture;
        private readonly IActionResult _result;

        public When_OpportunityProximity_Controller_RefineSearchResults_Is_Called_With_Unformatted_Postcode(OpportunityProximityControllerFixture fixture)
        {
            _fixture = fixture;
            const string requestPostcode = "Cv 12 Wt";

            _fixture.GetOpportunityProximityController(requestPostcode);
            _fixture.RoutePathService.GetRouteIdsAsync().Returns(new List<int> { 1, 2 });

            var viewModel = new SearchParametersViewModel
            {
                Postcode = requestPostcode,
                SelectedRouteId = 1
            };

            _result = _fixture.OpportunityProximityController.RefineSearchResultsAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_RedirectToRoute_Result_Is_Returned()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<RedirectToRouteResult>();

            var redirect = _result as RedirectToRouteResult;
            redirect.Should().NotBeNull();
            redirect?.RouteValues.Count.Should().BeGreaterOrEqualTo(3);
            redirect?.RouteValues["Postcode"].Should().Be("CV1 2WT");
            redirect?.RouteValues["SelectedRouteId"].Should().Be(1);
        }

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var result = _result as RedirectToRouteResult;
            // ReSharper disable once PossibleNullReferenceException
            result.RouteValues.Count.Should().BeGreaterOrEqualTo(3);
        }

        [Fact]
        public void Then_RouteService_GetRouteIdsAsync_Is_Called_exactly_Once()
        {
            _fixture.RoutePathService.Received(1).GetRouteIdsAsync();
        }
    }
}