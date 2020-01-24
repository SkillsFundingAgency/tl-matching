using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Tests.Common.HttpClient;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class GoogleMapsApiClientFixture : TestHttpClientFactory
    {
        public GoogleMapApiClient GoogleMapsApiClient;

        public void GetGoogleMapsApiClient(string requestPostcode = "CV12WT", string responseTown = "Coventry")
        {
            var response = new GooglePlacesResult
            {
                Results = new[] { new Result { FormattedAddress = $"Test Street, {responseTown} {requestPostcode}" } },
                Status = "OK"
            };

            var httpClient = CreateHttpClient(response, "https://example.com/place/textsearch/json?region=uk&radius=1&key=TEST_KEY&query=CV12WT");

            GoogleMapsApiClient = new GoogleMapApiClient(httpClient, new MatchingConfiguration
            {
                GoogleMapsApiKey = "TEST_KEY",
                GoogleMapsApiBaseUrl = "https://example.com/"
            });
        }
    }
}