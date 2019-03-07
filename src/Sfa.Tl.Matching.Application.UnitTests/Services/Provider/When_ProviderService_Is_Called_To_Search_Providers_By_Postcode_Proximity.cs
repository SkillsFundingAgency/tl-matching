using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Search_Providers_By_Postcode_Proximity
    {
        private const string Postcode = "SW1A 2AA";
        private const int SearchRadius = 5;
        private const int RouteId = 2;
        private readonly IEnumerable<ProviderVenueSearchResultDto> _result;

        public When_ProviderService_Is_Called_To_Search_Providers_By_Postcode_Proximity()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);

            var searchProvider = Substitute.For<ISearchProvider>();
            var locationService = Substitute.For<ILocationService>();
            var dto = new ProviderSearchParametersDto { Postcode = Postcode, SearchRadius = SearchRadius, SelectedRouteId = RouteId };
            searchProvider
                .SearchProvidersByPostcodeProximity(dto)
                .Returns(new SearchResultsBuilder().Build());

            var service = new ProviderService(mapper, searchProvider, locationService);

            _result = service.SearchProvidersByPostcodeProximity(dto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Search_Results_Is_Returned()
        {
            _result.Count().Should().Be(2);
        }

        [Fact]
        public void Then_The_First_Search_Result_Postcode_Is_As_Expected()
        {
            _result.First().Postcode.Should().BeEquivalentTo("NW1 3HB");
        }

        [Fact]
        public void Then_The_First_Search_Result_Distance_Is_As_Expected()
        {
            _result.First().Distance.Should().Be(2.5M);
        }

        [Fact]
        public void Then_The_First_Search_Result_Provider_Id_Is_As_Expected()
        {
            _result.First().ProviderId.Should().Be(1);
        }

        [Fact]
        public void Then_The_First_Search_Result_Provider_Name_Is_As_Expected()
        {
            _result.First().ProviderName.Should().BeEquivalentTo("The WKCIC Group");
        }

        [Fact]
        public void Then_The_First_Search_Result_QualificationShortTitles_Has_The_Expected_Number_Of_Items()
        {
            _result.First().QualificationShortTitles.Count().Should().Be(2);
        }

        [Fact]
        public void Then_The_First_Search_Result_First_QualificationShortTitle_Is_As_Expected()
        {
            _result.First().QualificationShortTitles.First().Should().BeEquivalentTo("applied science");
        }
    }
}
