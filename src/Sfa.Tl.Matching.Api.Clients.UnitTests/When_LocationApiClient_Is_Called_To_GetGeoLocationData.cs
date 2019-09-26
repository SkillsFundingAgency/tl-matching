using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.UnitTests.Factories;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_LocationApiClient_Is_Called_To_GetGeoLocationData
    {
        private readonly LocationApiClient _locationApiClient;

        public When_LocationApiClient_Is_Called_To_GetGeoLocationData()
        {
            var responseData = new PostcodeLookupResultDto
            {
                Postcode = "CV1 2WT",
                Latitude = "50.001",
                Longitude = "-1.234"
            };

            var httpClient = new PostcodesHttpClientFactory()
                .Get("CV1 2WT", responseData);

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
                .GetGeoLocationData("CV12WT");

            postcodeData.Should().NotBeNull();
            postcodeData.Postcode.Should().Be("CV1 2WT");
            postcodeData.Latitude.Should().Be("50.001");
            postcodeData.Longitude.Should().Be("-1.234");
        }
    }
}
