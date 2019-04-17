using System;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderVenueService_Is_Called_To_UpdateVenue
    {
        private readonly IProviderVenueRepository _providerVenueRepository;

        public When_ProviderVenueService_Is_Called_To_UpdateVenue()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);
            _providerVenueRepository = Substitute.For<IProviderVenueRepository>();

            _providerVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<ProviderVenue, bool>>>())
                .Returns(new ProviderVenue());

            var locationService = Substitute.For<ILocationService>();
            var providerVenueService = new ProviderVenueService(mapper, _providerVenueRepository,
                locationService);

            providerVenueService.UpdateVenueAsync(new UpdateProviderVenueDto()).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<ProviderVenue, bool>>>());
        }

        [Fact]
        public void Then_ProviderVenueRepository_Update_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1).Update(Arg.Any<ProviderVenue>());
        }
    }
}