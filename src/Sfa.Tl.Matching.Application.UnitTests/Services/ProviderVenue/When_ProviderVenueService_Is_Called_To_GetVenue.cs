using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue
{
    public class When_ProviderVenueService_Is_Called_To_GetVenue
    {
        private readonly IProviderVenueRepository _providerVenueRepository;
        private readonly ProviderVenueDetailViewModel _result;
        private const int ProviderId = 1;
        private const string Name = "Name";
        private const string Postcode = "CV1 2WT";

        public When_ProviderVenueService_Is_Called_To_GetVenue()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(ProviderVenueMapper).Assembly));
            var mapper = new Mapper(config);

            _providerVenueRepository = Substitute.For<IProviderVenueRepository>();
            _providerVenueRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new Domain.Models.ProviderVenue
                {
                    ProviderId = ProviderId,
                    Name = Name,
                    Postcode = Postcode
                });
            
            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var locationService = Substitute.For<ILocationApiClient>();

            var providerVenueService = new ProviderVenueService(mapper, _providerVenueRepository, locationService, googleMapApiClient);

            _result = providerVenueService.GetVenueAsync(ProviderId, Postcode).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1).GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>());
        }

        [Fact]
        public void Then_The_Fields_Are_As_Expected()
        {
            _result.ProviderId.Should().Be(ProviderId);
            _result.Name.Should().Be(Name);
        }
    }
}