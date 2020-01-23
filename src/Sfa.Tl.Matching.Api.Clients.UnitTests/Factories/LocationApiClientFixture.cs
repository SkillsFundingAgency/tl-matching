using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common.HttpClient;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests.Factories
{
    public class LocationApiClientFixture : TestHttpClientFactory
    {
        public LocationApiClient LocationApiClient;

        public void GetPostCodeHttpClient(string requestPostcode)
        {
            var response = new PostcodeLookupResponse
            {
                Result = new PostcodeLookupResultDto
                {
                    Postcode = "CV1 2WT",
                    Latitude = "50.001",
                    Longitude = "-1.234"
                },
                Status = "OK"
            };

            var httpClient = CreateMockClient(response, $"https://example.com/postcodes/{requestPostcode.Replace(" ", "")}");

            LocationApiClient = new LocationApiClient(httpClient, new MatchingConfiguration
            {
                PostcodeRetrieverBaseUrl = "https://example.com/"
            });
        }

        public void GetTerminatedPostCodeHttpClient(string requestPostcode)
        {
            var response = new PostcodeLookupResponse
            {
                Result = new PostcodeLookupResultDto
                {
                    Postcode = "S70 2YW",
                    Latitude = "50.001",
                    Longitude = "-1.234"
                },
                Status = "OK"
            };

            var httpClient = CreateMockClient(response, $"https://example.com/terminated_postcodes/{requestPostcode.Replace(" ", "")}");

            LocationApiClient = new LocationApiClient(httpClient, new MatchingConfiguration
            {
                PostcodeRetrieverBaseUrl = "https://example.com/"
            });
        }
    }
}