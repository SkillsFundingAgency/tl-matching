using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Application.IntegrationTests.Proximity;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_GetProximity_Data_For_Valid_Postcode
    {
        private readonly PostcodeLookupResultDto _postcodeLookupResultDto;

        public When_LocationService_Is_Called_To_GetProximity_Data_For_Valid_Postcode()
        {
            var httpClient = new TestPostcodesIoHttpClient().Get();

            var locationService = new LocationApiClient(httpClient, new MatchingConfiguration
            {
                PostcodeRetrieverBaseUrl = "https://api.postcodes.io/"
            });
            _postcodeLookupResultDto = locationService.GetGeoLocationDataAsync("CV1 2WT").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GeoLocationData_Has_Valid_Data()
        {
            _postcodeLookupResultDto.Should().NotBeNull();
            _postcodeLookupResultDto.Latitude.Should().Be("52.400997");
            _postcodeLookupResultDto.Longitude.Should().Be("-1.508122");
        }
    }
}
