using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
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
        private readonly long _arrivalTimeSeconds;

        public When_GoogleDistanceMatrixApiClient_Is_Called_To_GetJourneyTimes()
        {
            _arrivalTimeSeconds = 1570014314;
            var httpClient = new GoogleDistanceMatrixTestHttpClientFactory().Get(arrivalTimeSeconds: _arrivalTimeSeconds);
            var logger = new NullLogger<GoogleDistanceMatrixApiClient>();

            _googleDistanceMatrixApiClient = new GoogleDistanceMatrixApiClient(
                logger,
                httpClient,
                new MatchingConfiguration
                {
                    GoogleMapsApiKey = "TEST_KEY",
                    GoogleMapsApiBaseUrl = "https://example.com/"
                });
        }

        [Fact]
        public async Task Then_JourneyTimes_Are_Returned_Correctly()
        {
            var originPostcode = "CV1 2WT";

            var destinations = new List<LocationDto>
            {
                new LocationDto
                {
                    Id = 1,
                    Postcode = "SW1A 2AA",
                    Latitude = 51.50354M,
                    Longitude = -0.127695M
                }
            };

            var journeyTimes = await _googleDistanceMatrixApiClient
                .GetJourneyTimesAsync(
                    originPostcode, 
                    destinations, 
                    TravelMode.Driving, 
                    _arrivalTimeSeconds);

            journeyTimes.Should().NotBeNull();
            journeyTimes.Count.Should().Be(1);

            journeyTimes.First().Key.Should().Be(1);
            journeyTimes.First().Value.JourneyTime.Should().Be(7603);
            journeyTimes.First().Value.JourneyDistance.Should().Be(172648);
        }
    }
}
