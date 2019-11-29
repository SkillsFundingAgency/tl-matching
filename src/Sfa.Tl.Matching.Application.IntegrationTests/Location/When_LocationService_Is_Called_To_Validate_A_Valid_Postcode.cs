using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Application.IntegrationTests.TestClients;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_Validate_A_Valid_Postcode
    {
        private readonly (bool, string) _postcodeResultDto;

        public When_LocationService_Is_Called_To_Validate_A_Valid_Postcode()
        {
            const string requestPostcode = "Cv12wT";
            var httpClient = new TestPostcodesIoHttpClient().Get(requestPostcode);

            var locationService = new LocationApiClient(httpClient, new MatchingConfiguration
            {
                PostcodeRetrieverBaseUrl = "https://api.postcodes.io/"
            });
            _postcodeResultDto = locationService.IsValidPostcodeAsync(requestPostcode).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Should_Be_True()
        {
            _postcodeResultDto.Item1.Should().BeTrue();
            _postcodeResultDto.Item2.Should().Be("CV1 2WT");
        }
    }
}