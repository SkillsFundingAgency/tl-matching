using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue
{
    public class When_ProviderVenueService_IsValidPostcodeAsync_Is_Called_With_Terminated_Postcode
    {
        private const string Postcode = "AB1 0RU";

        private readonly ILocationApiClient _locationApiClient;
        private readonly (bool, string) _result;

        public When_ProviderVenueService_IsValidPostcodeAsync_Is_Called_With_Terminated_Postcode()
        {
            var mapper = Substitute.For<IMapper>();
            var providerVenueRepository = Substitute.For<IProviderVenueRepository>();
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            
            _locationApiClient = Substitute.For<ILocationApiClient>();
            _locationApiClient.IsValidPostcodeAsync(Postcode)
                .Returns((false, null));
            _locationApiClient.IsValidPostcodeAsync(Postcode, false)
                .Returns((false, null));
            _locationApiClient.IsValidPostcodeAsync(Postcode, true)
                .Returns((true, Postcode));

            var providerVenueService = new ProviderVenueService(mapper, providerVenueRepository, _locationApiClient, googleMapApiClient);

            _result = providerVenueService.IsValidPostcodeAsync(Postcode).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_LocationApiClient_IsValidPostcodeAsync_Without_Terminated_Postcode_Parameter_Is_Not_Called()
        {
            _locationApiClient
                .DidNotReceive()
                .IsValidPostcodeAsync(Arg.Any<string>());
        }

        [Fact]
        public void Then_LocationApiClient_IsValidPostcodeAsync_Is_Called_Exactly_Once()
        {
            _locationApiClient
                .Received(1)
                .IsValidPostcodeAsync(Postcode, true);
        }
        
        [Fact]
        public void Then_Result_Should_As_Expected()
        {
            _result.Item1.Should().BeTrue();
            _result.Item2.Should().Be(Postcode);
        }
    }
}