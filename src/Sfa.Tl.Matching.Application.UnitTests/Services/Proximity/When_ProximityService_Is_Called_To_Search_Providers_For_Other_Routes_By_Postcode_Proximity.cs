using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Proximity.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Proximity
{
    public class When_ProximityService_Is_Called_To_Search_Providers_For_Other_Routes_By_Postcode_Proximity
    {
        private const string Postcode = "SW1A 2AA";
        private const int SearchRadius = 25;
        private const int RouteId = 2;
        private readonly IList<SearchResultsByRouteViewModelItem> _result;
        private readonly ILocationApiClient _locationApiClient;

        private readonly ISearchProvider _searchProvider;

        public When_ProximityService_Is_Called_To_Search_Providers_For_Other_Routes_By_Postcode_Proximity()
        {
            var dto = new ProviderSearchParametersDto
            {
                Postcode = Postcode,
                SearchRadius = SearchRadius,
                SelectedRouteId = RouteId
            };

            _searchProvider = Substitute.For<ISearchProvider>();
            _searchProvider
                .SearchProvidersForOtherRoutesByPostcodeProximityAsync(dto)
                .Returns(new SearchResultsByRouteBuilder().Build());

            _locationApiClient = Substitute.For<ILocationApiClient>();
            _locationApiClient.GetGeoLocationDataAsync(Postcode, true).Returns(new PostcodeLookupResultDto
            {
                Postcode = Postcode,
                Longitude = "1.2",
                Latitude = "1.2"
            });

            var googleDistanceMatrixApiClient = Substitute.For<IGoogleDistanceMatrixApiClient>();
            
            var service = new ProximityService(_searchProvider, _locationApiClient, googleDistanceMatrixApiClient);

            _result = service.SearchProvidersForOtherRoutesByPostcodeProximityAsync(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Search_Results_Is_Returned()
        {
            _result.Count.Should().Be(2);
        }

        [Fact]
        public void Then_The_Search_Results_Should_Have_Expected_Values()
        {
            _result[0].NumberOfResults.Should().Be(1);
            _result[0].RouteName.Should().Be("digital");
            _result[1].NumberOfResults.Should().Be(2);
            _result[1].RouteName.Should().Be("health and beauty");
        }

        [Fact]
        public void Then_The_LocationService_Is_Called_Exactly_Once()
        {
            _locationApiClient.Received(1).GetGeoLocationDataAsync(Postcode, true);
        }

        [Fact]
        public void Then_The_ISearchProvider_Is_Called_Exactly_Once()
        {
            _searchProvider.Received(1).SearchProvidersForOtherRoutesByPostcodeProximityAsync(Arg.Any<ProviderSearchParametersDto>());
        }
    }
}
