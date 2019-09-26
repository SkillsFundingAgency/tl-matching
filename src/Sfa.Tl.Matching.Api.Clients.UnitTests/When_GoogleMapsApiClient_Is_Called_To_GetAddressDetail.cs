using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Api.Clients.UnitTests.Factories;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_GoogleMapsApiClient_Is_Called_To_GetAddressDetail
    {
        private readonly GoogleMapApiClient _googleMapsApiClient;

        public When_GoogleMapsApiClient_Is_Called_To_GetAddressDetail()
        {
            var httpClient = new GoogleMapsHttpClientFactory().Get();
            _googleMapsApiClient = new GoogleMapApiClient(httpClient,
                new MatchingConfiguration
                {
                    GoogleMapsApiKey = "TEST_KEY", 
                    GoogleMapsApiBaseUrl = "https://example.com/"
                });
        }

        [Fact]
        public async Task Then_PostTown_Is_Returned_Correctly()
        {
            var addressDetails = await _googleMapsApiClient.GetAddressDetails("CV12WT");
            addressDetails.Should().NotBeNull();
            addressDetails.Should().Be("Coventry");
        }
    }
}
