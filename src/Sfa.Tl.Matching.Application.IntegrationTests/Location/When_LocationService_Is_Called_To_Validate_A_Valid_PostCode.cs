using FluentAssertions;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.IntegrationTests.Proximity;
using Sfa.Tl.Matching.Application.Services;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_Validate_A_Valid_PostCode
    {
        private readonly (bool, string) _postCodeResultDto;

        public When_LocationService_Is_Called_To_Validate_A_Valid_PostCode()
        {
            const string requestPostcode = "Cv12wT";
            var httpClient = new PostcodesIoHttpClient().Get(requestPostcode);

            var locationService = new LocationService(httpClient, new MatchingConfiguration { PostcodeRetrieverBaseUrl = "https://api.postcodes.io/postcodes" });
            _postCodeResultDto = locationService.IsValidPostCode(requestPostcode).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Should_Be_True()
        {
            _postCodeResultDto.Item1.Should().BeTrue();
            _postCodeResultDto.Item2.Should().Be("CV1 2WT");
        }
    }
}