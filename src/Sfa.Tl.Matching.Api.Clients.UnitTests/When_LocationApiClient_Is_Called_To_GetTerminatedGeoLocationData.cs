using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.UnitTests.Factories;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_LocationApiClient_Is_Called_To_GetTerminatedGeoLocationData
    {
        private readonly LocationApiClient _locationApiClient;

        public When_LocationApiClient_Is_Called_To_GetTerminatedGeoLocationData()
        {
            var httpClient = new TerminatedPostcodesTestHttpClientFactory()
                .Get("S70 2YW", 50.001, -1.234);

            _locationApiClient = new LocationApiClient(httpClient,
                new MatchingConfiguration
                {
                    PostcodeRetrieverBaseUrl = "https://example.com/"
                });
        }

        [Fact]
        public async Task Then_Terminated_Postcode_Is_Returned_Correctly()
        {
            var postcodeData = await _locationApiClient
                .GetTerminatedPostcodeGeoLocationDataAsync("S702YW");

            postcodeData.Should().NotBeNull();
            postcodeData.Postcode.Should().Be("S70 2YW");
            postcodeData.Latitude.Should().Be("50.001");
            postcodeData.Longitude.Should().Be("-1.234");
        }
    }
}
