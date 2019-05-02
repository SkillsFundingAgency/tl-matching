﻿using System;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
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
        private readonly IRepository<Domain.Models.ProviderVenue> _providerVenueRepository;

        public When_ProviderVenueService_Is_Called_To_Hide_Venue()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(ProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserNameResolver") ?
                        (object)new LoggedInUserNameResolver<HideProviderVenueViewModel, Domain.Models.ProviderVenue>(httpcontextAccesor) :
                        type.Name.Contains("UtcNowResolver") ?
                            new UtcNowResolver<HideProviderVenueViewModel, Domain.Models.ProviderVenue>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            var locationService = Substitute.For<ILocationService>();

            _providerVenueRepository = Substitute.For<IProviderVenueRepository>();

            _providerVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new ValidProviderVenueBuilder().Build());

            var service = new ProviderVenueService(mapper, _providerVenueRepository, locationService);

            var viewModel = new HideProviderVenueViewModel
            {
                ProviderId = 10,
                ProviderVenueId = 1,
                IsEnabledForSearch = false
            };
            service.UpdateVenueAsync(viewModel).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_ProviderVenueRepository_UpdateWithSpecifedColumnsOnly_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1)
                .UpdateWithSpecifedColumnsOnly(Arg.Any<Domain.Models.ProviderVenue>(),
                    Arg.Any<Expression<Func<Domain.Models.ProviderVenue, object>>[]>());
        }

        [Fact]
        public void Then_ProviderVenueRepository_UpdateWithSpecifedColumnsOnly_Is_Called_With_Expected_Values()
        {
            _providerVenueRepository.Received(1)
                .UpdateWithSpecifedColumnsOnly(Arg.Is<Domain.Models.ProviderVenue>(
                    p =>
                        p.Id == 1 &&
                        !p.IsEnabledForSearch
                        ),
                    Arg.Any<Expression<Func<Domain.Models.ProviderVenue, object>>[]>());
        }
    }
}
