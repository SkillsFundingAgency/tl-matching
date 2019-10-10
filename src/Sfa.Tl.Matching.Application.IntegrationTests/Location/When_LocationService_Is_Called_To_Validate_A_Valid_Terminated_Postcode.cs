using System.Net.Http;
using FluentAssertions;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_Validate_A_Valid_Terminated_Postcode
    {
        private readonly (bool, string) _result;

        public When_LocationService_Is_Called_To_Validate_A_Valid_Terminated_Postcode()
        {
            var locationService = new LocationApiClient(new HttpClient(), 
                new MatchingConfiguration
                {
                    PostcodeRetrieverBaseUrl = "https://api.postcodes.io/"
                });
            _result = locationService.IsValidPostcodeAsync("bb5 2aw", true).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Result_Should_Be_True()
        {
            _result.Item1.Should().BeTrue();
            _result.Item2.Should().Be("BB5 2AW");
        }
    }
}