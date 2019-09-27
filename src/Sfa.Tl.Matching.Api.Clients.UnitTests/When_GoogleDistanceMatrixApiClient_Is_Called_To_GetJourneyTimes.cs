using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GoogleDistanceMatrix;
using Sfa.Tl.Matching.Api.Clients.UnitTests.Factories;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_GoogleDistanceMatrixApiClient_Is_Called_To_GetJourneyTimes
    {
        private readonly GoogleDistanceMatrixApiClient _googleDistanceMatrixApiClient;

        public When_GoogleDistanceMatrixApiClient_Is_Called_To_GetJourneyTimes()
        {
            var httpClient = new GoogleDistanceMatrixHttpClientFactory().Get();
            _googleDistanceMatrixApiClient = new GoogleDistanceMatrixApiClient(httpClient,
                new MatchingConfiguration
                {
                    GoogleMapsApiKey = "TEST_KEY", 
                    GoogleMapsApiBaseUrl = "https://example.com/"
                });
        }

        [Fact]
        public async Task Then_JourneyTimes_Are_Returned_Correctly()
        {
            var origin = new LocationDto
            {
                Latitude = 52.400997M,
                Longitude = -1.508122M,
            };

            var destinations = new List<LocationDto>
            {
                new LocationDto {Latitude = 51.50354M, Longitude = -0.127695M}
            };
            
            var journeyTimes = await _googleDistanceMatrixApiClient
                .GetJourneyTimesAsync(origin.Latitude, origin.Longitude,
                    destinations, TravelMode.Driving);

            journeyTimes.Should().NotBeNull();
            journeyTimes.Count.Should().Be(1);

            journeyTimes.First().TravelTime.Should().Be(7603);
            journeyTimes.First().TravelDistance.Should().Be(172648);
        }
    }
}
