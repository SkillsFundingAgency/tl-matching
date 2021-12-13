using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Api.Clients.UnitTests.Factories;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_GoogleMapsApiClient_Is_Called_To_GetAddressDetail_With_No_Results
    {
        private readonly GoogleMapApiClient _googleMapsApiClient;

        public When_GoogleMapsApiClient_Is_Called_To_GetAddressDetail_With_No_Results()
        {
            var httpClient = new GoogleMapsTestHttpClientFactory().GetWithBadStatus();
            _googleMapsApiClient = new GoogleMapApiClient(httpClient,
                new MatchingConfiguration
                {
                    GoogleMapsApiKey = "TEST_KEY", 
                    GoogleMapsApiBaseUrl = "https://example.com/"
                });
        }

        [Fact]
        public async Task Then_Empty_Town_Is_Returned()
        {
            var addressDetails = await _googleMapsApiClient.GetAddressDetailsAsync("CV12WT");
            addressDetails.Should().BeEmpty();
        }
    }
}
