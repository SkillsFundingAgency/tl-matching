using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue
{
    public class When_ProviderVenueService_Is_Called_To_Get_HideProviderVenueViewModel
    {
        private readonly HideProviderVenueViewModel _result;
        private readonly IProviderVenueRepository _providerVenueRepository;

        public When_ProviderVenueService_Is_Called_To_Get_HideProviderVenueViewModel()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderVenueMapper).Assembly));
            var mapper = new Mapper(config);
            var locationService = Substitute.For<ILocationService>();
            _providerVenueRepository = Substitute.For<IProviderVenueRepository>();

            _providerVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new ValidProviderVenueBuilder().Build());

            var service = new ProviderVenueService(mapper, _providerVenueRepository, locationService);

            _result = service.GetHideProviderVenueViewModelAsync(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>());
        }

        [Fact]
        public void Then_The_ProviderVenueId_Is_As_Expected()
        {
            _result.ProviderVenueId.Should().Be(1);
        }

        [Fact]
        public void Then_The_Postcode_Is_As_Expected()
        {
            _result.Postcode.Should().Be("CV1 2WT");
        }

        [Fact]
        public void Then_The_ProviderVenue_Name_Is_As_Expected()
        {
            _result.ProviderVenueName.Should().Be("Test Provider Venue");
        }

        [Fact]
        public void Then_The_Provider_IsEnabledForSearch_Is_As_Expected()
        {
            _result.IsRemoved.Should().Be(true);
        }
    }
}
