using System.Net.Http;
using FluentAssertions;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_GetProximity_Data_For_Valid_PostCode
    {
        private readonly PostCodeLookupResultDto _postCodeLookupResultDto;

        public When_LocationService_Is_Called_To_GetProximity_Data_For_Valid_PostCode()
        {
            var locationService = new LocationService(new HttpClient(), new MatchingConfiguration { PostcodeRetrieverBaseUrl = "https://api.postcodes.io/postcodes" });
            _postCodeLookupResultDto = locationService.GetGeoLocationData("CV1 2WT").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_GeoLocationData_Is_Returned()
        {
            _postCodeLookupResultDto.Should().NotBeNull();
        }

        [Fact]
        public void Then_GeoLocationData_Has_Valid_Data()
        {
            _postCodeLookupResultDto.Latitude.Should().Be("52.400997");
            _postCodeLookupResultDto.Longitude.Should().Be("-1.508122");
        }
    }
}
