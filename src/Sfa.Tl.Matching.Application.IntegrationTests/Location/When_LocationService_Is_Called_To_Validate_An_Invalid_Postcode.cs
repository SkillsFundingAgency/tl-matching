using System.Net;
using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_Validate_An_Invalid_Postcode : IClassFixture<LocationApiClientFixture>
    {
        private readonly (bool, string) _result;

        public When_LocationService_Is_Called_To_Validate_An_Invalid_Postcode(LocationApiClientFixture fixture)
        {
            fixture.GetLocationApiClient("CV1234", HttpStatusCode.NotFound);
            _result = fixture.LocationApiClient.IsValidPostcodeAsync("CV1234").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Should_Be_False()
        {
            _result.Item1.Should().BeFalse();
            _result.Item2.Should().BeNullOrEmpty();
        }
    }
}