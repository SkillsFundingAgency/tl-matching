using System;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
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
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(ProviderVenueMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<AddProviderVenueViewModel, ProviderVenue>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<AddProviderVenueViewModel, ProviderVenue>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowCreatedResolver") ?
                                new UtcNowCreatedResolver<AddProviderVenueViewModel, ProviderVenue>(new DateTimeProvider()) :
                                null);
            });
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

            var viewModel = new AddProviderVenueViewModel
            {
                Postcode = Postcode
            };

            providerVenueService.CreateVenueAsync(viewModel).GetAwaiter().GetResult();
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