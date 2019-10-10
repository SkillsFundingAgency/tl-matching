using System.Net.Http;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests.Factories
{
    public class GoogleMapsTestHttpClientFactory : TestHttpClientFactory
    {
        public HttpClient Get(string requestPostcode = "CV12WT", string responseTown = "Coventry")
        {
            var response = new GooglePlacesResult
            {
                Results = new[]{ new Result
                {
                    FormattedAddress = $"Test Street, {responseTown} {requestPostcode}"
                }},
                Status = "OK"
            };

            return CreateClient(response,
                "https://example.com/place/textsearch/json?region=uk&radius=1&key=TEST_KEY&query=CV12WT");
        }
    }
}