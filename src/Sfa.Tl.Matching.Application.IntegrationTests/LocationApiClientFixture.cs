using System.Net;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common.HttpClient;

namespace Sfa.Tl.Matching.Application.IntegrationTests
{
    public class LocationApiClientFixture : TestHttpClientFactory
    {
        public ILocationApiClient LocationApiClient;

        public void GetLocationApiClient(string requestPostcode, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            var response = new PostcodeLookupResponse
            {
                Result = new PostcodeLookupResultDto
                {
                    Latitude = "52.400997",
                    Longitude = "-1.508122",
                    Postcode = requestPostcode
                }
            };
            
            var httpClient = CreateHttpClient(response, "https://api.postcodes.io/postcodes/{requestPostcode.ToLetterOrDigit()}", "application/json", statusCode);
           
            LocationApiClient = new LocationApiClient(httpClient, new MatchingConfiguration
            {
                PostcodeRetrieverBaseUrl = "https://api.postcodes.io/"
            });
        }
    }
}