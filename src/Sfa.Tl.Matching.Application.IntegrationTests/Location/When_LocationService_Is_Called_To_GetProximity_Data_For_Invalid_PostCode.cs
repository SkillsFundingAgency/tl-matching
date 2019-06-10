using System;
using System.Net.Http;
using FluentAssertions;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Location
{
    public class When_LocationService_Is_Called_To_GetProximity_Data_For_Invalid_PostCode
    {
        private readonly LocationService _locationService;

        public When_LocationService_Is_Called_To_GetProximity_Data_For_Invalid_PostCode()
        {
            _locationService = new LocationService(new HttpClient(), new MatchingConfiguration { PostcodeRetrieverBaseUrl = "https://api.postcodes.io/postcodes" });
        }

        [Fact]
        public void Then_service_Throws_Exeption()
        {
            Action action = () => _locationService.GetGeoLocationData("CV1234").GetAwaiter().GetResult();

            action.Should().ThrowExactly<HttpRequestException>();
        }
    }
}