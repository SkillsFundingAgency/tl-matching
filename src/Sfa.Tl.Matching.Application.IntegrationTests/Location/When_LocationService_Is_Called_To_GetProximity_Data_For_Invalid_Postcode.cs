using System;
using System.Net.Http;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_GetProximity_Data_For_Invalid_Postcode
    {
        private readonly LocationApiClient _locationApiClient;

        public When_LocationService_Is_Called_To_GetProximity_Data_For_Invalid_Postcode()
        {
            _locationApiClient = new LocationApiClient(new HttpClient(), new MatchingConfiguration
            {
                PostcodeRetrieverBaseUrl = "https://api.postcodes.io/"
            });
        }

        [Fact]
        public void Then_service_Throws_Exception()
        {
            Action action = () => _locationApiClient.GetGeoLocationDataAsync("CV1234").GetAwaiter().GetResult();

            action.Should().ThrowExactly<HttpRequestException>();
        }
    }
}