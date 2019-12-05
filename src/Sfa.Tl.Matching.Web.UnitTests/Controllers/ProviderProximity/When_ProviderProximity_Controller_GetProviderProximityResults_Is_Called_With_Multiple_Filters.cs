using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.ProviderProximity
{
    public class When_ProviderProximity_Controller_GetProviderProximityResults_Is_Called_With_Multiple_Filters
    {
        private readonly IActionResult _result;
        private readonly IProviderProximityService _providerProximityService;
        private const string Postcode = "CV1 2WT";

        public When_ProviderProximity_Controller_GetProviderProximityResults_Is_Called_With_Multiple_Filters()
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

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRouteSelectListItemsAsync().Returns(routes);
            routePathService.GetRouteDictionaryAsync().Returns(routeDictionary);

            _providerProximityService = Substitute.For<IProviderProximityService>();
            _providerProximityService.SearchProvidersByPostcodeProximityAsync(
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

            var providerProximityController = new ProviderProximityController(routePathService, _providerProximityService,
                locationService);

            _result = providerProximityController.GetProviderProximityResults($"{Postcode}-Route 1-Route 2")
                .GetAwaiter().GetResult();
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
            viewModel.SearchParameters.Postcode.Should().Be(Postcode);
            viewModel.SearchParameters.SelectedFilters.Count.Should().Be(2);
            viewModel.SearchParameters.SelectedFilters[0].Should().Be("Route 1");
            viewModel.SearchParameters.SelectedFilters[1].Should().Be("Route 2");

            viewModel.SearchParameters.Filters[0].Name.Should().Be("Route 1");
            viewModel.SearchParameters.Filters[0].IsSelected.Should().Be(true);
            viewModel.SearchParameters.Filters[1].Name.Should().Be("Route 2");
            viewModel.SearchParameters.Filters[1].IsSelected.Should().Be(true);

            viewModel.SearchResults.SearchResultProviderCount.Should().Be(1);
            viewModel.SearchResults.Results[0].Distance.Should().Be(1.4);

            viewModel.SearchResults.Results.Select(x => x.ProviderDisplayName).Should()
                .Contain("Provider display name");
            viewModel.SearchResults.Results.Select(x => x.ProviderVenueName).Should()
                .Contain("Provider venue display name");
        }

        [Fact]
        public void Then_ProximityService_SearchOpportunitiesByPostcodeProximity_Is_Called_Exactly_Once()
        {
            _providerProximityService
                .Received(1)
                .SearchProvidersByPostcodeProximityAsync(
                    Arg.Is<ProviderProximitySearchParametersDto>(
                        p => p.Postcode == Postcode &&
                             p.SearchRadius == SearchParametersViewModel.DefaultSearchRadius &&
                             p.SelectedRoutes.Count == 2));
        }
    }
}