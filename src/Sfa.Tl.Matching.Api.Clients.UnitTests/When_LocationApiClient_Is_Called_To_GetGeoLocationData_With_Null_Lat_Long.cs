using System.Globalization;
using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.UnitTests.Factories;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_LocationApiClient_Is_Called_To_GetGeoLocationData_With_Null_Lat_Long
    {
        private readonly LocationApiClient _locationApiClient;

        public When_LocationApiClient_Is_Called_To_GetGeoLocationData_With_Null_Lat_Long()
        {
            var httpClient = new PostcodesTestHttpClientFactory()
                .Get("GY1 4NS", null, null);

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
                .GetGeoLocationDataAsync("GY14NS");

            postcodeData.Should().NotBeNull();
            postcodeData.Postcode.Should().Be("GY1 4NS");
            postcodeData.Latitude.Should().Be(LocationApiClient.DefaultLatitude.ToString(CultureInfo.InvariantCulture));
            postcodeData.Longitude.Should().Be(LocationApiClient.DefaultLongitude.ToString(CultureInfo.InvariantCulture));
        }
    }
}
