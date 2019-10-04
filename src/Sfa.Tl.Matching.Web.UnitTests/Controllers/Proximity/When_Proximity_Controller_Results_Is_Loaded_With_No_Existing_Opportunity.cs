using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Web.UnitTests.Controllers.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Proximity
{
    public class When_Proximity_Controller_Results_Is_Loaded_With_No_Existing_Opportunity
    {
        private readonly IActionResult _result;
        private readonly IProximityService _proximityService;
        private readonly IOpportunityService _opportunityService;

        private const int RouteId = 1;
        private const string Postcode = "SW1A 2AA";
        private readonly int _selectedRouteId;

        public When_Proximity_Controller_Results_Is_Loaded_With_No_Existing_Opportunity()
        {
            var routes = new List<Route>
                {
                    new Route { Id = 1, Name = "Route 1" }
                }
                .AsQueryable();

            _selectedRouteId = routes.First().Id;

            var providerSearchResultDto = new List<SearchResultsViewModelItem>
            {
                new SearchResultsViewModelItem
                {
                    ProviderVenuePostcode = Postcode,
                    Distance = 1.5d
                }
            };

            var config = new MapperConfiguration(c => c.AddMaps(typeof(SearchParametersViewModelMapper).Assembly));
            IMapper mapper = new Mapper(config);

            _proximityService = Substitute.For<IProximityService>();
            _proximityService
                .SearchProvidersByPostcodeProximityAsync(Arg.Is<ProviderSearchParametersDto>(a => a.Postcode == Postcode && a.SelectedRouteId == RouteId))
                .Returns(providerSearchResultDto);

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);

            _opportunityService = Substitute.For<IOpportunityService>();
            var employerService = Substitute.For<IEmployerService>();

            var proximityController = new ProximityController(mapper, routePathService, _proximityService, _opportunityService,
                employerService);

            _result = proximityController.GetProviderResultsAsync(new SearchParametersViewModel
            {
                SelectedRouteId = RouteId,
                Postcode = Postcode
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProximityService_SearchProvidersByPostcodeProximity_Is_Called_Exactly_Once()
        {
            _proximityService
                .Received(1)
                .SearchProvidersByPostcodeProximityAsync(
                    Arg.Is<ProviderSearchParametersDto>(
                        a => a.Postcode == Postcode && 
                             a.SelectedRouteId == RouteId));
        }
        
        [Fact]
        public void Then_ProximityService_SearchProvidersForOtherRoutesByPostcodeProximity_Is_Called_Exactly_Once()
        {
            _proximityService
                .DidNotReceive()
                .SearchProvidersForOtherRoutesByPostcodeProximityAsync(
                    Arg.Any<ProviderSearchParametersDto>());
        }

        [Fact]
        public void Then_SearchViewModel_Contains_SearchParametersViewModel()
        {
            var viewModel = _result.GetViewModel<SearchViewModel>();
            viewModel.SearchParameters.Should().NotBeNull();
        }

        [Fact]
        public void Then_SearchViewModel_Contains_SearchResultsViewModel()
        {
            var viewModel = _result.GetViewModel<SearchViewModel>();
            viewModel.SearchResults.Should().NotBeNull();
        }

        [Fact]
        public void Then_SearchViewModel_SearchParameters_Values_Should_Match_Input_Values()
        {
            var searchParametersViewModel = _result.GetViewModel<SearchViewModel>().SearchParameters;
            searchParametersViewModel.Postcode.Should().Be(Postcode);
            searchParametersViewModel.SelectedRouteId.Should().Be(_selectedRouteId);
        }

        [Fact]
        public void Then_SearchViewModel_SearchResults_Should_Have_Expected_Number_Of_Items()
        {
            var searchResultsViewModel = _result.GetViewModel<SearchViewModel>().SearchResults;
            searchResultsViewModel.Results.Count.Should().Be(1);
        }

        [Fact]
        public void Then_SearchViewModel_AdditionalSearchResults_Should_Be_Null_Or_Empty()
        {
            var searchResultsViewModel = _result.GetViewModel<SearchViewModel>().SearchResults;
            searchResultsViewModel.AdditionalResults.Should().BeNullOrEmpty();
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
        public void Then_GetReferrals_Is_Not_Called()
        {
            _opportunityService.DidNotReceive().GetReferrals(Arg.Any<int>());
        }

        [Fact]
        public void Then_SearchViewModel_Results_Is_Not_Selected()
        {
            var viewModel = _result.GetViewModel<SearchViewModel>();
            viewModel.SearchResults.Results[0].IsSelected.Should().BeFalse();
        }

        [Fact]
        public void Then_OpportunityService_GetCompanyNameWithAkaAsync_Is_Not_Called()
        {
            _opportunityService
                .DidNotReceiveWithAnyArgs()
                .GetCompanyNameWithAkaAsync(Arg.Any<int>());
        }
    }
}