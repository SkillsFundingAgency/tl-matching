using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_RefineSearchResults_Is_Called_With_Invalid_Postcode : IClassFixture<OpportunityProximityControllerFixture>
    {
        private readonly OpportunityProximityControllerFixture _fixture;
        private readonly IActionResult _result;

        public When_OpportunityProximity_Controller_RefineSearchResults_Is_Called_With_Invalid_Postcode(OpportunityProximityControllerFixture fixture)
        {
            _fixture = fixture;
            const string requestPostcode = "CV1234";

            var routes = new List<SelectListItem>
            {
                new SelectListItem {Text = "1", Value = "Route 1"}
            };

            _fixture.GetOpportunityProximityController(requestPostcode);
            _fixture.RoutePathService.GetRouteSelectListItemsAsync().Returns(routes);
            
            var viewModel = new SearchParametersViewModel
            {
                Postcode = "CV1234",
                SelectedRouteId = 1
            };

            _result = _fixture.OpportunityProximityController.RefineSearchResultsAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            _result.Should().NotBeNull();
            _result.Should().BeOfType<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Model_Contains_Postcode_Error()
        {
            _fixture.OpportunityProximityController.ViewData.ModelState.IsValid.Should().BeFalse();
            _fixture.OpportunityProximityController.ViewData.ModelState["Postcode"].Errors.Should().ContainSingle(error => error.ErrorMessage == "You must enter a real postcode");
        }
    }
}