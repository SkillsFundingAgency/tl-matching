using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.GeoLocations;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue
{
    public class When_ProviderVenueService_Is_Called_To_Get_RemoveProviderVenueViewModel
    {
        private readonly RemoveProviderVenueViewModel _result;
        private readonly IProviderVenueRepository _providerVenueRepository;

        public When_ProviderVenueService_Is_Called_To_Get_RemoveProviderVenueViewModel()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(ProviderVenueMapper).Assembly));
            var mapper = new Mapper(config);

            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var locationService = Substitute.For<ILocationApiClient>();

            _providerVenueRepository = Substitute.For<IProviderVenueRepository>();
            _providerVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new ValidProviderVenueBuilder().Build());

            var service = new ProviderVenueService(mapper, _providerVenueRepository, locationService, googleMapApiClient);

            _result = service.GetRemoveProviderVenueViewModelAsync(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>());
        }

        [Fact]
        public void Then_The_View_Model_Fields_Are_As_Expected()
        {
            _result.ProviderVenueId.Should().Be(1);
            _result.Postcode.Should().Be("CV1 2WT");
        }
    }
}
