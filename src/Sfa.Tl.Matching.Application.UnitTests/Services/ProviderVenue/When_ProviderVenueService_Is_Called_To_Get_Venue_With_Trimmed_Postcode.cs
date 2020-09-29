using System;
using System.Collections.Generic;
using System.Linq;
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
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderVenue
{
    public class When_ProviderVenueService_Is_Called_To_Get_Venue_With_Trimmed_Postcode
    {
        private readonly IProviderVenueRepository _providerVenueRepository;
        private readonly ProviderVenueDetailViewModel _result;
        private const int ProviderId = 1;
        private const int ProviderVenueId = 10;
        private const string Name = "Name";
        private const string Postcode = "CV12WT";
        private const bool IsEnabledForReferral = true;
        private const bool IsRemoved = true;
        private const string DataSource = "TEST";

        public When_ProviderVenueService_Is_Called_To_Get_Venue_With_Trimmed_Postcode()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(ProviderVenueMapper).Assembly));
            var mapper = new Mapper(config);

            var mockProviderVenueDbSet = new List<Domain.Models.ProviderVenue>
                {
                    new Domain.Models.ProviderVenue
                    {
                        Id = ProviderVenueId,
                        ProviderId = ProviderId,
                        Name = Name,
                        Postcode = Postcode,
                        IsRemoved = IsRemoved,
                        IsEnabledForReferral = IsEnabledForReferral,
                        Source = DataSource
                    }
                }
                .AsQueryable()
                .BuildMockDbSet();
            
            _providerVenueRepository = Substitute.For<IProviderVenueRepository>();
            _providerVenueRepository.GetManyAsync(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(mockProviderVenueDbSet);

            var googleMapApiClient = Substitute.For<IGoogleMapApiClient>();
            var locationService = Substitute.For<ILocationApiClient>();

            var providerVenueService = new ProviderVenueService(mapper, _providerVenueRepository, locationService, googleMapApiClient);

            _result = providerVenueService.GetVenueWithTrimmedPostcodeAsync(ProviderId, Postcode).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderVenueRepository_GetMany_Is_Called_Exactly_Once()
        {
            _providerVenueRepository.Received(1).GetManyAsync(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>());
        }
        
        [Fact]
        public void Then_The_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(ProviderVenueId);
            _result.ProviderId.Should().Be(ProviderId);
            _result.Postcode.Should().Be(Postcode);
            _result.Name.Should().Be(Name);
            _result.IsEnabledForReferral.Should().Be(IsEnabledForReferral);
            _result.IsRemoved.Should().Be(IsRemoved);
            _result.Source.Should().Be(DataSource);
        }
    }
}