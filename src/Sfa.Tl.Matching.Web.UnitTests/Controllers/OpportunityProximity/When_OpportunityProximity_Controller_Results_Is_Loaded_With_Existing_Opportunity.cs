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

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.OpportunityProximity
{
    public class When_OpportunityProximity_Controller_Results_Is_Loaded_With_Existing_Opportunity
    {
        private readonly IActionResult _result;
        private readonly IOpportunityProximityService _opportunityProximityService;
        private readonly IOpportunityService _opportunityService;

        private const int OpportunityId = 1;
        private const int OpportunityItemId = 1;

        private const int RouteId = 1;
        private const int ProviderVenueId = 11;

        private const string ProviderDisplayName = "Provider display name";
        private const string ProviderVenueDisplayName = "Provider venue display name";

        private const string Postcode = "SW1A 2AA";
        private readonly int _selectedRouteId;

        public When_OpportunityProximity_Controller_Results_Is_Loaded_With_Existing_Opportunity()
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
                    Distance = 1.5d,
                    ProviderVenueId = ProviderVenueId,
                    ProviderVenueName = ProviderVenueDisplayName,
                    ProviderDisplayName = ProviderDisplayName
                }
            };

            var config = new MapperConfiguration(c => c.AddMaps(typeof(SearchParametersViewModelMapper).Assembly));
            IMapper mapper = new Mapper(config);

            var locationService = Substitute.For<ILocationService>();

            _opportunityProximityService = Substitute.For<IOpportunityProximityService>();
            _opportunityProximityService
                .SearchOpportunitiesByPostcodeProximityAsync(Arg.Is<OpportunityProximitySearchParametersDto>(a =>
                    a.Postcode == Postcode && a.SelectedRouteId == RouteId))
                .Returns(providerSearchResultDto);

            var routePathService = Substitute.For<IRoutePathService>();
            routePathService.GetRoutes().Returns(routes);

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetReferrals(OpportunityItemId).Returns(new List<ReferralDto>
            {
                new ReferralDto
                {
                    ProviderVenueId = ProviderVenueId,
                    Name = "Referral"
                }
            });
            _opportunityService
                .GetCompanyNameWithAkaAsync(OpportunityId)
                .Returns("CompanyName (AlsoKnownAs)");

            var employerService = Substitute.For<IEmployerService>();

            var opportunityProximityController = new OpportunityProximityController(mapper, routePathService, _opportunityProximityService, _opportunityService,
                employerService, locationService);

            _result = opportunityProximityController.GetOpportunityProviderResultsAsync(new SearchParametersViewModel
            {
                SelectedRouteId = RouteId,
                Postcode = Postcode,
                OpportunityId = OpportunityId,
                OpportunityItemId = OpportunityItemId
            }).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProximityService_SearchOpportunitiesByPostcodeProximity_Is_Called_Exactly_Once()
        {
            _opportunityProximityService
                .Received(1)
                .SearchOpportunitiesByPostcodeProximityAsync(
                    Arg.Is<OpportunityProximitySearchParametersDto>(
                        a => a.Postcode == Postcode &&
                             a.SelectedRouteId == RouteId));
        }


        [Fact]
        public void Then_ProximityService_SearchOpportunitiesForOtherRoutesByPostcodeProximity_Is_Called_Exactly_Once()
        {
            _opportunityProximityService
                .DidNotReceive()
                .SearchOpportunitiesForOtherRoutesByPostcodeProximityAsync(
                    Arg.Any<OpportunityProximitySearchParametersDto>());
        }

        [Fact]
        public void Then_GetReferrals_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetReferrals(OpportunityItemId);
        }

        [Fact]
        public void Then_OpportunityService_GetCompanyNameWithAkaAsync_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .GetCompanyNameWithAkaAsync(Arg.Any<int>());
        }

        [Fact]
        public void Then_Model_Is_Not_Null()
        {
            _result.Should().NotBeNull();
            _result.Should().BeAssignableTo<ViewResult>();
            var viewResult = _result as ViewResult;
            viewResult.Should().NotBeNull();
            viewResult?.Model.Should().NotBeNull();
        }

        [Fact]
        public void Then_ViewModel_Should_Have_Expected_Values()
        {
            var viewModel = _result.GetViewModel<SearchViewModel>();

            viewModel.SearchParameters.Should().NotBeNull();
            var searchParametersViewModel = _result.GetViewModel<SearchViewModel>().SearchParameters;
            searchParametersViewModel.Postcode.Should().Be(Postcode);
            searchParametersViewModel.SelectedRouteId.Should().Be(_selectedRouteId);
            searchParametersViewModel.CompanyNameWithAka.Should().Be("CompanyName (AlsoKnownAs)");

            viewModel.SearchResults.Should().NotBeNull();
            var searchResultsViewModel = _result.GetViewModel<SearchViewModel>().SearchResults;
            searchResultsViewModel.Results.Count.Should().Be(1);
            searchResultsViewModel.AdditionalResults.Should().BeNullOrEmpty();

            searchResultsViewModel.Results.Select(x => x.ProviderDisplayName).Should()
                .Contain("Provider display name");
            searchResultsViewModel.Results.Select(x => x.ProviderVenueName).Should()
                .Contain("Provider venue display name");

            searchResultsViewModel.Results[0].IsSelected.Should().BeTrue();
        }
    }
}
