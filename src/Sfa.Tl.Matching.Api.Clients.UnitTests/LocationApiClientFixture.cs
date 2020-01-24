using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;
using Sfa.Tl.Matching.Tests.Common.HttpClient;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class LocationApiClientFixture : TestHttpClientFactory
    {
        public LocationApiClient LocationApiClient;

        public void GetPostCodeHttpClient(string requestPostcode, string urlPart = "postcodes")
        {
            var response = new PostcodeLookupResponse
            {
                Result = new PostcodeLookupResultDto
                {
                    Postcode = requestPostcode,
                    Latitude = "50.001",
                    Longitude = "-1.234"
                },
                Status = "OK"
            };

            UseMockedHttpClient = true;
            var httpClient = CreateHttpClient(response, $"https://example.com/{urlPart}/{requestPostcode.Replace(" ", "")}");

            LocationApiClient = new LocationApiClient(httpClient, new MatchingConfiguration
            {
                PostcodeRetrieverBaseUrl = "https://example.com/"
            });
        }
    }
}