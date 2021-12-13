using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Controllers;
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
        private const int SelectedRouteId = 1;

        public When_OpportunityProximity_Controller_Results_Is_Loaded_With_Existing_Opportunity()
        {
            var providerSearchResultDto = new List<OpportunityProximitySearchResultViewModelItem>
            {
                new()
                {
                    ProviderVenuePostcode = Postcode,
                    Distance = 1.5d,
                    ProviderVenueId = ProviderVenueId,
                    ProviderVenueName = ProviderVenueDisplayName,
                    ProviderDisplayName = ProviderDisplayName
                }
            };

            var locationService = Substitute.For<ILocationService>();
            locationService.IsValidPostcodeAsync(Arg.Any<string>()).Returns((true, Postcode));

            var routeService = Substitute.For<IRoutePathService>();
            routeService.GetRouteIdsAsync().Returns(new List<int> { 1, 2 });

            _opportunityProximityService = Substitute.For<IOpportunityProximityService>();
            _opportunityProximityService
                .SearchOpportunitiesByPostcodeProximityAsync(Arg.Is<OpportunityProximitySearchParametersDto>(a =>
                    a.Postcode == Postcode && 
                    a.SelectedRouteId == RouteId))
                .Returns(providerSearchResultDto);

            _opportunityService = Substitute.For<IOpportunityService>();
            _opportunityService.GetOpportunityItemAsync(OpportunityItemId)
                .Returns(new OpportunityItemDto
                {
                    OpportunityId = OpportunityId,
                    OpportunityItemId = OpportunityItemId,
                    Postcode = Postcode,
                    RouteId = RouteId
                });

            _opportunityService.GetReferralsAsync(OpportunityItemId).Returns(new List<ReferralDto>
            {
                new()
                {
                    ProviderVenueId = ProviderVenueId,
                    Name = "Referral"
                }
            });

            _opportunityService
                .GetCompanyNameWithAkaAsync(OpportunityId)
                .Returns("CompanyName (AlsoKnownAs)");

            var opportunityProximityController = new OpportunityProximityController(routeService, _opportunityProximityService, _opportunityService, locationService);

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
        public void Then_OpportunityService_GetOpportunityItemAsync_Is_Called_Exactly_Once()
        {
            _opportunityService
                .Received(1)
                .GetOpportunityItemAsync(Arg.Any<int>());
        }

        [Fact]
        public void Then_GetReferrals_Is_Called_Exactly_Once()
        {
            _opportunityService.Received(1).GetReferralsAsync(OpportunityItemId);
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
            var viewModel = _result.GetViewModel<OpportunityProximitySearchViewModel>();

            viewModel.SearchParameters.Should().NotBeNull();
            var searchParametersViewModel = _result.GetViewModel<OpportunityProximitySearchViewModel>().SearchParameters;
            searchParametersViewModel.Postcode.Should().Be(Postcode);
            searchParametersViewModel.SelectedRouteId.Should().Be(SelectedRouteId);
            searchParametersViewModel.CompanyNameWithAka.Should().Be("CompanyName (AlsoKnownAs)");

            viewModel.SearchResults.Should().NotBeNull();
            var searchResultsViewModel = _result.GetViewModel<OpportunityProximitySearchViewModel>().SearchResults;
            searchResultsViewModel.Results.Count.Should().Be(1);
            searchResultsViewModel.AdditionalResults.Should().BeNullOrEmpty();

            searchResultsViewModel.Results.Select(x => x.ProviderDisplayName).Should().Contain("Provider display name");
            searchResultsViewModel.Results.Select(x => x.ProviderVenueName).Should().Contain("Provider venue display name");

            searchResultsViewModel.Results[0].IsSelected.Should().BeTrue();
        }
    }
}
