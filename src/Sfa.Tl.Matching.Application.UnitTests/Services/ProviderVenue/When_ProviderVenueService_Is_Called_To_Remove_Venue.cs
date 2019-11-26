using System;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue
{
    public class When_ProviderVenueService_Is_Called_To_Hide_Venue
    {
        private readonly IProviderVenueRepository _providerVenueRepository;

        public When_ProviderVenueService_Is_Called_To_Hide_Venue()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(ProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserNameResolver") ?
                        (object)new LoggedInUserNameResolver<RemoveProviderVenueViewModel, Domain.Models.ProviderVenue>(httpcontextAccesor) :
                        type.Name.Contains("UtcNowResolver") ?
                            new UtcNowResolver<RemoveProviderVenueViewModel, Domain.Models.ProviderVenue>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var locationService = Substitute.For<ILocationApiClient>();

            _providerVenueRepository = Substitute.For<IProviderVenueRepository>();
            _providerVenueRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new ValidProviderVenueBuilder().Build());

            var service = new ProviderVenueService(mapper, _providerVenueRepository, locationService, googleMapApiClient);

            var viewModel = new RemoveProviderVenueViewModel
            {
                ProviderId = 1,
                ProviderVenueId = 1
            };
            service.UpdateVenueAsync(viewModel).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_ProviderVenueRepository_UpdateWithSpecifedColumnsOnly_Is_Called_Exactly_Once_With_Expected_Values()
        {
            _providerVenueRepository
                .Received(1)
                .UpdateWithSpecifedColumnsOnlyAsync(Arg.Is<Domain.Models.ProviderVenue>(
                    pv =>
                        pv.Id == 1 &&
                        pv.IsRemoved
                        ),
                    Arg.Any<Expression<Func<Domain.Models.ProviderVenue, object>>[]>());
        }
    }
}
