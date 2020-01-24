using System.Net;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Tests.Common.HttpClient;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class LocationApiClientFixture : TestHttpClientFactory
    {
        public ILocationApiClient LocationApiClient;

        public void GetLocationApiClient(string requestPostcode, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var httpClient = CreateHttpClient(requestPostcode, "https://api.postcodes.io/postcodes/{requestPostcode.ToLetterOrDigit()}", "application/json");
           
            LocationApiClient = new LocationApiClient(httpClient, new MatchingConfiguration
            {
                PostcodeRetrieverBaseUrl = "https://api.postcodes.io/"
            });
        }
    }
}