using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderVenueService_Is_Called_To_HaveUniqueVenue_Exists
    {
        private readonly IRepository<ProviderVenue> _providerVenueRepository;
        private readonly bool _isUniqueVenue;
        private const long UkPrn = 123;
        private const string Postcode = "CV1 2WT";

        public When_ProviderVenueService_Is_Called_To_HaveUniqueVenue_Exists()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);
            _providerVenueRepository = Substitute.For<IProviderVenueRepository>();

            _providerVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<ProviderVenue, bool>>>())
                .Returns(new ProviderVenue());

            var locationService = Substitute.For<ILocationService>();
            var providerVenueService = new ProviderVenueService(mapper, _providerVenueRepository,
                locationService);

            _isUniqueVenue = providerVenueService.HaveUniqueVenueAsync(UkPrn, Postcode).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<ProviderVenue, bool>>>());
        }

        [Fact]
        public void Then_IsUniqueVenue_Is_False()
        {
            _isUniqueVenue.Should().BeFalse();
        }
    }
}