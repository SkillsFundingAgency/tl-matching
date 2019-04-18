using System;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue
{
    public class When_ProviderVenueService_Is_Called_To_CreateVenue
    {
        private const string Postcode = "CV1 2WT";

        private readonly IProviderVenueRepository _providerVenueRepository;
        private readonly ILocationService _locationService;

        public When_ProviderVenueService_Is_Called_To_CreateVenue()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);
            _providerVenueRepository = Substitute.For<IProviderVenueRepository>();

            _providerVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new Domain.Models.ProviderVenue());

            _locationService = Substitute.For<ILocationService>();
            _locationService.GetGeoLocationData(Postcode).Returns(new PostCodeLookupResultDto
            {
                Postcode = Postcode,
                Longitude = "1.2",
                Latitude = "1.2"
            });
            var providerVenueService = new ProviderVenueService(mapper, _providerVenueRepository,
                _locationService);

            var dto = new ProviderVenueDto
            {
                Postcode = Postcode
            };

            providerVenueService.CreateVenueAsync(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_LocationService_GetGeoLocationData_Is_Called_Exactly_Once()
        {
            _locationService.Received(1).GetGeoLocationData(Postcode);
        }

        [Fact]
        public void Then_ProviderVenueRepository_Create_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1).Create(Arg.Any<Domain.Models.ProviderVenue>());
        }
    }
}