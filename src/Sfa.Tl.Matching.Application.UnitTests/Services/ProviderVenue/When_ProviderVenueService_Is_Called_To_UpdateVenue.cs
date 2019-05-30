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
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue
{
    public class When_ProviderVenueService_Is_Called_To_UpdateVenue
    {
        private readonly IProviderVenueRepository _providerVenueRepository;

        public When_ProviderVenueService_Is_Called_To_UpdateVenue()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(ProviderVenueMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<ProviderVenueDetailViewModel, Domain.Models.ProviderVenue>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<ProviderVenueDetailViewModel, Domain.Models.ProviderVenue>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<ProviderVenueDetailViewModel, Domain.Models.ProviderVenue>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);
            _providerVenueRepository = Substitute.For<IProviderVenueRepository>();

            _providerVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new Domain.Models.ProviderVenue());

            var locationService = Substitute.For<ILocationService>();
            var providerVenueService = new ProviderVenueService(mapper, _providerVenueRepository,
                locationService);

            providerVenueService.UpdateVenueAsync(new ProviderVenueDetailViewModel()).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>());
        }

        [Fact]
        public void Then_ProviderVenueRepository_Update_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1).Update(Arg.Any<Domain.Models.ProviderVenue>());
        }
    }
}