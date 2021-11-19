using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_FindProviders_Is_Called_For_Invalid_Postcode
    {
        private readonly IActionResult _result;
        private readonly IRoutePathService _routeService;

        public When_OpportunityProximity_Controller_FindProviders_Is_Called_For_Invalid_Postcode()
        {
            var locationService = Substitute.For<ILocationService>();
            locationService.IsValidPostcodeAsync(Arg.Any<string>()).Returns((false, null));

            var opportunityProximityService = Substitute.For<IOpportunityProximityService>();

            _routeService = Substitute.For<IRoutePathService>();
            _routeService.GetRouteIdsAsync().Returns(new List<int> { 1, 2 });

            var opportunityService = Substitute.For<IOpportunityService>();

            var opportunityProximityController = new OpportunityProximityController(_routeService, opportunityProximityService, opportunityService, locationService);

            const string postcode = "XYZ A12";

            var viewModel = new SearchParametersViewModel
            {
                RoutesSelectList = new List<SelectListItem>
                {
                    new() { Text = "1", Value = "Route 1" },
                    new() { Text = "2", Value = "Route 2" }
                },
                SelectedRouteId = 1,
                Postcode = postcode
            };
            _result = opportunityProximityController.FindProviders(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Model_Is_Of_Type_SearchParametersViewModel()
        {
            _result.Should().NotBeNull();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
            viewResult?.Model.Should().BeOfType<SearchParametersViewModel>();
        }

        [Fact]
        public void Then_Model_Contains_Postcode_Error()
        {
            var viewResult = _result as ViewResult;
            viewResult?.ViewData.ModelState.IsValid.Should().BeFalse();
            viewResult?.ViewData.ModelState["Postcode"]
                .Errors
                .Should()
                .ContainSingle(error => error.ErrorMessage == "You must enter a real postcode");
        }
        
        [Fact]
        public void Then_RouteService_GetRouteIdsAsync_Is_Called_exactly_Once()
        {
            _routeService.Received(1).GetRouteIdsAsync();
        }
    }
}