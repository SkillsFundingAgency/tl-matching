using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_FindProviders_Is_Called_For_Invalid_Postcode
    {
        private readonly IActionResult _result;

        public When_OpportunityProximity_Controller_FindProviders_Is_Called_For_Invalid_Postcode()
        {
            var routes = new List<SelectListItem>
            {
                new SelectListItem {Text = "1", Value = "Route 1"},
                new SelectListItem {Text = "2", Value = "Route 2"}
            };

            var routeDictionary = new Dictionary<int, string>
            {
                {1, "Route 1" },
                {2, "Route 2" }
            };

            var config = new MapperConfiguration(c => c.AddMaps(typeof(SearchParametersViewModelMapper).Assembly));
            IMapper mapper = new Mapper(config);

            var locationService = Substitute.For<ILocationService>();
            locationService.IsValidPostcodeAsync(Arg.Any<string>()).Returns((false, null));

            var opportunityProximityService = Substitute.For<IOpportunityProximityService>();

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRouteSelectListItemsAsync().Returns(routes);
            routePathService.GetRouteDictionaryAsync().Returns(routeDictionary);

            var opportunityService = Substitute.For<IOpportunityService>();

            var opportunityProximityController = new OpportunityProximityController(routePathService,
                opportunityProximityService, opportunityService, locationService);

            var selectedRouteId = routes.First().Text;
            const string postcode = "XYZ A12";

            var viewModel = new SearchParametersViewModel
            {
                RoutesSelectList = mapper.Map<SelectListItem[]>(routes),
                SelectedRouteId = int.Parse(selectedRouteId),
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
    }
}