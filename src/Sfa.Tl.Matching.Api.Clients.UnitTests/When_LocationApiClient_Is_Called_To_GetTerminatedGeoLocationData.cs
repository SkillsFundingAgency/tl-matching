using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.UnitTests.Factories;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_LocationApiClient_Is_Called_To_GetTerminatedGeoLocationData : IClassFixture<LocationApiClientFixture>
    {
        private readonly LocationApiClientFixture _fixture;

        public When_LocationApiClient_Is_Called_To_GetTerminatedGeoLocationData(LocationApiClientFixture fixture)
        {
            _fixture = fixture;
            fixture.GetTerminatedPostCodeHttpClient("S70 2YW");
        }

        [Fact]
        public async Task Then_Terminated_Postcode_Is_Returned_Correctly()
        {
            var postcodeData = await _fixture.LocationApiClient.GetTerminatedPostcodeGeoLocationDataAsync("S702YW");

            postcodeData.Should().NotBeNull();
            postcodeData.Postcode.Should().Be("S70 2YW");
            postcodeData.Latitude.Should().Be("50.001");
            postcodeData.Longitude.Should().Be("-1.234");
        }
    }
}
