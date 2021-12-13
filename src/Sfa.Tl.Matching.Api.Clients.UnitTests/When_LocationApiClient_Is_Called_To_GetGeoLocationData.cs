using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.UnitTests.Factories;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_LocationApiClient_Is_Called_To_GetGeoLocationData
    {
        private readonly LocationApiClient _locationApiClient;

        public When_LocationApiClient_Is_Called_To_GetGeoLocationData()
        {
            var httpClient = new PostcodesTestHttpClientFactory()
                .Get("CV1 2WT", 50.001, -1.234);

            _locationApiClient = new LocationApiClient(httpClient,
                new MatchingConfiguration
                {
                    PostcodeRetrieverBaseUrl = "https://example.com/"
                });
        }

        [Fact]
        public async Task Then_Postcode_Is_Returned_Correctly()
        {
            var postcodeData = await _locationApiClient
                .GetGeoLocationDataAsync("CV12WT");

            postcodeData.Should().NotBeNull();
            postcodeData.Postcode.Should().Be("CV1 2WT");
            postcodeData.Latitude.Should().Be("50.001");
            postcodeData.Longitude.Should().Be("-1.234");
        }
    }
}
