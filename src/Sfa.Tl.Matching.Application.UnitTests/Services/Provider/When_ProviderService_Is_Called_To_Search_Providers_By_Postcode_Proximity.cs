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
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Search_Providers_By_Postcode_Proximity
    {
        //private readonly IRepository<Domain.Models.Provider> _repository;
        private readonly string _postcode = "SW1A 2AA";
        private readonly int _searchRadius = 5;
        private readonly int _routeId = 2;
        private readonly IEnumerable<ProviderVenueSearchResultDto> _result;

        public When_ProviderService_Is_Called_To_Search_Providers_By_Postcode_Proximity()
        {
            var config = new MapperConfiguration(c => c.AddProfile<ProviderMapper>());
            var mapper = new Mapper(config);
            var searchResultconfig = new MapperConfiguration(c => c.AddProfile<ProviderVenueSearchResultMapper>());
            var searchResultMapper = new Mapper(searchResultconfig);

            var fileReader = Substitute.For<IFileReader<ProviderFileImportDto, ProviderDto>>();
            var repository = Substitute.For<IRepository<Domain.Models.Provider>>();
            var searchProvider = Substitute.For<ISearchProvider>();

            searchProvider
                .SearchProvidersByPostcodeProximity(_postcode, _searchRadius, _routeId)
                .Returns(new SearchResultsBuilder().Build());

            var service = new ProviderService(mapper, fileReader, repository, searchResultMapper, searchProvider);

            _result = service.SearchProvidersByPostcodeProximity(_postcode, _searchRadius, _routeId).GetAwaiter().GetResult();
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
