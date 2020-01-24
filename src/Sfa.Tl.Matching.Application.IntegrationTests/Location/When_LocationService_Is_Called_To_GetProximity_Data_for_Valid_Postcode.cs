using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_GetProximity_Data_For_Valid_Postcode : IClassFixture<LocationApiClientFixture>
    {
        private readonly PostcodeLookupResultDto _postcodeLookupResultDto;

        public When_LocationService_Is_Called_To_GetProximity_Data_For_Valid_Postcode(LocationApiClientFixture fixture)
        {
            fixture.GetLocationApiClient("CV1 2WT");
            
            _postcodeLookupResultDto = fixture.LocationApiClient.GetGeoLocationDataAsync("CV1 2WT").GetAwaiter().GetResult();
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
