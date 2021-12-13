using System.Globalization;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Application.IntegrationTests.TestClients;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_GetProximity_Data_For_Valid_Postcode_With_No_Lat_Long
    {
        private readonly PostcodeLookupResultDto _postcodeLookupResultDto;

        public When_LocationService_Is_Called_To_GetProximity_Data_For_Valid_Postcode_With_No_Lat_Long()
        {
            var httpClient = new TestPostcodesIoHttpClient().Get();

            var locationService = new LocationApiClient(httpClient, new MatchingConfiguration
            {
                PostcodeRetrieverBaseUrl = "https://api.postcodes.io/"
            });
            _postcodeLookupResultDto = locationService.GetGeoLocationDataAsync("GY1 4NS").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GeoLocationData_Has_Valid_Data()
        {
            _postcodeLookupResultDto.Should().NotBeNull();
            _postcodeLookupResultDto.Latitude.Should().Be(LocationApiClient.DefaultLatitude.ToString(CultureInfo.InvariantCulture));
            _postcodeLookupResultDto.Longitude.Should().Be(LocationApiClient.DefaultLongitude.ToString(CultureInfo.InvariantCulture));
        }
    }
}
