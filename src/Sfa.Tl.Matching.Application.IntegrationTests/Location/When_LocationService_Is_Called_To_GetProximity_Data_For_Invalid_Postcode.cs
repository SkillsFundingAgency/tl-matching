using System;
using System.Net.Http;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_GetProximity_Data_For_Invalid_Postcode : IClassFixture<LocationApiClientFixture>
    {
        private readonly ILocationApiClient _locationService;

        public When_LocationService_Is_Called_To_GetProximity_Data_For_Invalid_Postcode(LocationApiClientFixture fixture)
        {
            fixture.GetLocationApiClient(String.Empty);
            _locationService = fixture.LocationApiClient;
        }

        [Fact]
        public void Then_Service_Throws_Exception()
        {
            Action action = () => _locationService.GetGeoLocationDataAsync("CV1234").GetAwaiter().GetResult();

            action.Should().ThrowExactly<HttpRequestException>();
        }
    }
}