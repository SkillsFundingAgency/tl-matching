using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Application.IntegrationTests.Proximity;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_Validate_A_Terminated_Postcode
    {
        private readonly (bool, string) _postcodeResultDto;

        public When_LocationService_Is_Called_To_Validate_A_Terminated_Postcode()
        {
            const string requestPostcode = "S70 2YW";
            var httpClient = new TestPostcodesIoHttpClient().Get(requestPostcode);

            var locationService = new LocationApiClient(httpClient, new MatchingConfiguration
            {
                PostcodeRetrieverBaseUrl = "https://api.postcodes.io/"
            });
            _postcodeResultDto = locationService.IsTerminatedPostcode(requestPostcode).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Should_Be_True()
        {
            _postcodeResultDto.Item1.Should().BeTrue();
            _postcodeResultDto.Item2.Should().Be("S70 2YW");
        }
    }
}