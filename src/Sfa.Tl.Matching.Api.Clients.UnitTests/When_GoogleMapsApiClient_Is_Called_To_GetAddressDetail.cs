using System.Threading.Tasks;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.UnitTests.Factories;
using Xunit;

namespace Sfa.Tl.Matching.Api.Clients.UnitTests
{
    public class When_GoogleMapsApiClient_Is_Called_To_GetAddressDetail : IClassFixture<GoogleMapsApiClientFixture>
    {
        private readonly GoogleMapsApiClientFixture _fixture;

        public When_GoogleMapsApiClient_Is_Called_To_GetAddressDetail(GoogleMapsApiClientFixture fixture)
        {
            _fixture = fixture;
            fixture.GetGoogleMapsApiClient();
        }

        [Fact]
        public async Task Then_PostTown_Is_Returned_Correctly()
        {
            var addressDetails = await _fixture.GoogleMapsApiClient.GetAddressDetailsAsync("CV12WT");
            addressDetails.Should().NotBeNull();
            addressDetails.Should().Be("Coventry");
        }
    }
}
