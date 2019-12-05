using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderProximity.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderProximity
{
    public class When_ProviderProximityService_Is_Called_To_Search_Providers_By_Postcode_Proximity
    {
        private const string Postcode = "SW1A 2AA";
        private const int SearchRadius = 5;
        private readonly IList<ProviderProximitySearchResultViewModelItem> _result;
        private readonly ICacheService _cacheService;
        private readonly ILocationService _locationService;
        private readonly ISearchProvider _searchProvider;

        public When_ProviderProximityService_Is_Called_To_Search_Providers_By_Postcode_Proximity()
        {
            var dto = new ProviderProximitySearchParametersDto
            {
                Postcode = Postcode,
                SearchRadius = SearchRadius
            };

            _searchProvider = Substitute.For<ISearchProvider>();
            _searchProvider
                .SearchProvidersByPostcodeProximityAsync(dto)
                .Returns(new SearchResultsBuilder().Build());

            _cacheService = Substitute.For<ICacheService>();
            _cacheService.Get<IList<ProviderProximitySearchResultViewModelItem>>(Arg.Any<string>()) 
                .Returns((IList<ProviderProximitySearchResultViewModelItem>)null);

            _locationService = Substitute.For<ILocationService>();
            _locationService.GetGeoLocationDataAsync(Postcode)
                .Returns(new PostcodeLookupResultDto
                {
                    Postcode = Postcode,
                    Longitude = "1.2",
                    Latitude = "1.2"
                });

            var service = new ProviderProximityService(_searchProvider, _locationService, _cacheService);

            _result = service.SearchProvidersByPostcodeProximityAsync(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Search_Results_Is_Returned()
        {
            _result.Count.Should().Be(2);
        }
        
        [Fact]
        public void Then_The_Search_Results_Should_Have_Expected_Values()
        {
            _result[0].ProviderVenueId.Should().Be(1);
            _result[1].ProviderVenueId.Should().Be(2);
        }
        
        [Fact]
        public void Then_The_CacheService_Get_Is_Called_Exactly_Once()
        {
            _cacheService.Received(1).Get<IList<ProviderProximitySearchResultViewModelItem>>(Arg.Any<string>());
        }

        [Fact]
        public void Then_The_CacheService_Set_Is_Called_Exactly_Once()
        {
            _cacheService.Received(1).Set(Arg.Any<string>(), Arg.Any<IList<ProviderProximitySearchResultViewModelItem>>(), Arg.Any<TimeSpan>());
        }

        [Fact]
        public void Then_The_LocationService_Is_Called_Exactly_Once()
        {
            _locationService.Received(1).GetGeoLocationDataAsync(Postcode);
        }

        [Fact]
        public void Then_The_ISearchProvider_Is_Called_Exactly_Once()
        {
            _searchProvider.Received(1).SearchProvidersByPostcodeProximityAsync(Arg.Any<ProviderProximitySearchParametersDto>());
        }
    }
}
