using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderProximity
{
    public class When_ProviderProximity_Controller_GetProviderProximityResults_Is_Called_With_No_Filters
    {
        private readonly IActionResult _result;

        public When_ProviderProximity_Controller_GetProviderProximityResults_Is_Called_With_No_Filters()
        {
            var routes = new List<Route>
                {
                    new Route { Id = 1, Name = "Route 1" },
                    new Route { Id = 2, Name = "Route 2" }
                }
                .AsQueryable();

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);

            var providerProximityService = Substitute.For<IProviderProximityService>();
            providerProximityService.SearchProvidersByPostcodeProximityAsync(
                    Arg.Any<ProviderProximitySearchParametersDto>()).Returns
                (
                     new List<ProviderProximitySearchResultViewModelItem>
                     {
                         new ProviderProximitySearchResultViewModelItem
                         {
                             Distance = 1.4,
                             ProviderDisplayName   = "Provider display name",
                             ProviderVenueName = "Provider venue display name"
                         }
                     }
                );

            var locationService = Substitute.For<ILocationService>();

            var providerProximityController = new ProviderProximityController(routePathService, providerProximityService,
                locationService);

            _result = providerProximityController.GetProviderProximityResults("CV1 2WT").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ViewModel_Should_Have_Expected_Values()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();

            var viewModel = _result.GetViewModel<ProviderProximitySearchViewModel>();
            viewModel.SearchParameters.Postcode.Should().Be("CV1 2WT");
            viewModel.SearchParameters.SelectedFilters.Count.Should().Be(0);
            viewModel.SearchParameters.Filters[0].Name.Should().Be("Route 1");
            viewModel.SearchParameters.Filters[1].Name.Should().Be("Route 2");

            viewModel.SearchResults.SearchResultProviderCount.Should().Be(1);
            viewModel.SearchResults.Results[0].Distance.Should().Be(1.4);

            viewModel.SearchResults.Results.Select(x => x.ProviderDisplayName).Should()
                .Contain("Provider display name");
            viewModel.SearchResults.Results.Select(x => x.ProviderVenueName).Should()
                .Contain("Provider venue display name");
        }
    }
}