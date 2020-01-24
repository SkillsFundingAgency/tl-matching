using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_Validate_A_Terminated_Postcode : IClassFixture<LocationApiClientFixture>
    {
        private readonly (bool, string) _postcodeResultDto;

        public When_LocationService_Is_Called_To_Validate_A_Terminated_Postcode(LocationApiClientFixture fixture)
        {
            const string requestPostcode = "S70 2YW";
            fixture.GetLocationApiClient(requestPostcode);
            _postcodeResultDto = fixture.LocationApiClient.IsTerminatedPostcodeAsync(requestPostcode).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Should_Be_True()
        {
            _postcodeResultDto.Item1.Should().BeTrue();
            _postcodeResultDto.Item2.Should().Be("S70 2YW");
        }
    }
}