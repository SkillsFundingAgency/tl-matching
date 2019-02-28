using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Provider
{
    public class When_Provider_Controller_Results_Post_Is_Called
    {
        private readonly IProviderService _providerService;
        private const string Postcode = "SW1A 2AA";
        private const int SearchRadius = 5;
        private const int RouteId = 1;
        private readonly string _selectedRouteId;
        private readonly IActionResult _result;

        public When_Provider_Controller_Results_Post_Is_Called()
        {
            var routes = new List<Route>
                {
                    new Route { Id = 1, Name = "Route 1" }
                }
                .AsQueryable();

            var logger = Substitute.For<ILogger<ProviderController>>();
            var mapper = Substitute.For<IMapper>();

            var providerSearchResultDto = new List<ProviderVenueSearchResultDto>
            {
                new ProviderVenueSearchResultDto
                {
                    Postcode = Postcode,
                    Distance = 1.5M
                }
            };

            _providerService = Substitute.For<IProviderService>();
            _providerService
                .SearchProvidersByPostcodeProximity(Postcode, SearchRadius, RouteId)
                .Returns(providerSearchResultDto);

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);
            var providerController = new ProviderController(logger, mapper, routePathService, _providerService);
            providerController.CreateContextWithSubstituteTempData();

            _selectedRouteId = routes.First().Id.ToString();

            var viewModel = new SearchParametersViewModel
            {
                RoutesSelectList = mapper.Map<SelectListItem[]>(routes),
                SearchRadius = SearchRadius,
                SelectedRouteId = _selectedRouteId,
                Postcode = Postcode
            };

            _result = providerController.Results(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderService_SearchProvidersByPostcodeProximity_Is_Called_Exactly_Once()
        {
            _providerService
                .Received(1)
                .SearchProvidersByPostcodeProximity(Postcode, SearchRadius, RouteId);
        }

        [Fact]
        public void Then_Result_Is_Not_Null() =>
            _result.Should().NotBeNull();

        [Fact]
        public void Then_View_Result_Is_Returned() =>
            _result.Should().BeAssignableTo<ViewResult>();

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_Model_Is_Of_Type_SearchViewModel()
        {
            var viewResult = _result as ViewResult;
            viewResult?.Model.Should().BeOfType<SearchViewModel>();
        }

        [Fact]
        public void Then_ViewModel_Contains_SearchParametersViewModel()
        {
            var viewModel = _result.GetViewModel<SearchViewModel>();
            viewModel.SearchParameters.Should().NotBeNull();
        }

        [Fact]
        public void Then_ViewModel_Contains_SearchResultsViewModel()
        {
            var viewModel = _result.GetViewModel<SearchViewModel>();
            viewModel.SearchResults.Should().NotBeNull();
        }

        [Fact]
        public void Then_ViewModel_SearchParameters_Postcode_Should_Be_Input_Postcode()
        {
            var searchParametersViewModel = _result.GetViewModel<SearchViewModel>().SearchParameters;
            searchParametersViewModel.Postcode.Should().Be(Postcode);
        }

        [Fact]
        public void Then_ViewModel_SearchParameters_SearchRadius_Should_Be_Input_SearchRadius()
        {
            var searchParametersViewModel = _result.GetViewModel<SearchViewModel>().SearchParameters;
            searchParametersViewModel.SearchRadius.Should().Be(SearchRadius);
        }

        [Fact]
        public void Then_ViewModel_SearchParameters_SelectedRouteIds_Should_Be_Input_SelectedRouteId()
        {
            var searchParametersViewModel = _result.GetViewModel<SearchViewModel>().SearchParameters;
            searchParametersViewModel.SelectedRouteId.Should().Be(_selectedRouteId);
        }

        [Fact]
        public void Then_ViewModel_SearchResults_Should_Have_Expected_Number_Of_Items()
        {
            var searchResultsViewModel = _result.GetViewModel<SearchViewModel>().SearchResults;
            searchResultsViewModel.Results.Count().Should().Be(1);
        }
    }
}
