using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.UnitTests.Factories;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_LocationApiClient_Is_Called_To_GetGeoLocationData : IClassFixture<LocationApiClientFixture>
    {
        private readonly LocationApiClientFixture _fixture;

        public When_LocationApiClient_Is_Called_To_GetGeoLocationData(LocationApiClientFixture fixture)
        {
            _fixture = fixture;
            fixture.GetPostCodeHttpClient("CV1 2WT");
        }

        [Fact]
        public async Task Then_Postcode_Is_Returned_Correctly()
        {
            var postcodeData = await _fixture.LocationApiClient.GetGeoLocationDataAsync("CV12WT");

            postcodeData.Should().NotBeNull();
            postcodeData.Postcode.Should().Be("CV1 2WT");
            postcodeData.Latitude.Should().Be("50.001");
            postcodeData.Longitude.Should().Be("-1.234");
        }
    }
}
