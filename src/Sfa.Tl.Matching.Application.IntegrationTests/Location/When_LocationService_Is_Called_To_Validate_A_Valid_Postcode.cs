using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_Validate_A_Valid_Postcode : IClassFixture<LocationApiClientFixture>
    {
        private readonly (bool, string) _postcodeResultDto;

        public When_LocationService_Is_Called_To_Validate_A_Valid_Postcode(LocationApiClientFixture fixture)
        {
            const string requestPostcode = "Cv12wT";
            fixture.GetLocationApiClient(requestPostcode);
            _postcodeResultDto = fixture.LocationApiClient.IsValidPostcodeAsync(requestPostcode).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Should_Be_True()
        {
            _postcodeResultDto.Item1.Should().BeTrue();
            _postcodeResultDto.Item2.Should().Be("CV1 2WT");
        }
    }
}