using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.OpportunityProximity.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.OpportunityProximity
{
    public class When_OpportunityProximityService_Is_Called_To_Search_Providers_By_Postcode_Proximity
    {
        private const string Postcode = "SW1A 2AA";
        private const int SearchRadius = 5;
        private const int RouteId = 2;
        private readonly IList<SearchResultsViewModelItem> _result;
        private readonly ILocationService _locationService;
        private readonly ISearchProvider _searchProvider;

        public When_OpportunityProximityService_Is_Called_To_Search_Providers_By_Postcode_Proximity()
        {
            var dto = new ProviderSearchParametersDto
            {
                Postcode = Postcode,
                SearchRadius = SearchRadius,
                SelectedRouteId = RouteId
            };

            _searchProvider = Substitute.For<ISearchProvider>();
            _searchProvider
                .SearchProvidersByPostcodeProximityAsync(dto)
                .Returns(new SearchResultsBuilder().Build());

            _locationService = Substitute.For<ILocationService>();
            _locationService.GetGeoLocationDataAsync(Postcode, true)
                .Returns(new PostcodeLookupResultDto
                {
                    Postcode = Postcode,
                    Longitude = "1.2",
                    Latitude = "1.2"
                });

            var googleDistanceMatrixApiClient = Substitute.For<IGoogleDistanceMatrixApiClient>();
            googleDistanceMatrixApiClient.GetJourneyTimesAsync(Arg.Any<string>(), 
                    Arg.Any<decimal>(), Arg.Any<decimal>(),
                    Arg.Any<IList<LocationDto>>(), TravelMode.Driving, 
                    Arg.Any<long>())
                .Returns(new JourneyTimesBuilder().BuildDrivingResults());
            googleDistanceMatrixApiClient.GetJourneyTimesAsync(Arg.Any<string>(), 
                    Arg.Any<decimal>(), Arg.Any<decimal>(),
                    Arg.Any<IList<LocationDto>>(), TravelMode.Transit, 
                    Arg.Any<long>())
                .Returns(new JourneyTimesBuilder().BuildPublicTransportResults());

            var service = new OpportunityProximityService(_searchProvider, _locationService, googleDistanceMatrixApiClient);

            _result = service.SearchProvidersByPostcodeProximityAsync(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Search_Results_Is_Returned()
        {
            _result.Count.Should().Be(2);
        }
        
        [Fact]
        public void Then_The_Search_Results_Should_Have_Expected_Values()
        {
            _result[0].ProviderVenueId.Should().Be(1);
            _result[1].ProviderVenueId.Should().Be(2);
        }

        [Fact]
        public void Then_The_LocationService_Is_Called_Exactly_Once()
        {
            _locationService.Received(1).GetGeoLocationDataAsync(Postcode, true);
        }

        [Fact]
        public void Then_The_ISearchProvider_Is_Called_Exactly_Once()
        {
            _searchProvider.Received(1).SearchProvidersByPostcodeProximityAsync(Arg.Any<ProviderSearchParametersDto>());
        }
    }
}
