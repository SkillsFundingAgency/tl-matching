using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Search_Providers_From_ReferenceData
    {
        private const long UkPrn = 10000546;

        private readonly ProviderSearchResultDto _result;
        private readonly IRepository<ProviderReference> _providerReferenceRepository;

        public When_ProviderService_Is_Called_To_Search_Providers_From_ReferenceData()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);
            var providerRepository = Substitute.For<IRepository<Domain.Models.Provider>>();

            _providerReferenceRepository = Substitute.For<IRepository<ProviderReference>>();
            _providerReferenceRepository.GetSingleOrDefault(Arg.Any<Expression<Func<ProviderReference, bool>>>())
                .Returns(new ValidProviderReferenceBuilder().Build());

            var service = new ProviderService(mapper, providerRepository, _providerReferenceRepository);

            _result = service.SearchReferenceDataAsync(UkPrn).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderReferenceRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerReferenceRepository.Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<ProviderReference, bool>>>());
        }
        
        [Fact]
        public void Then_The_Provider_Id_Is_As_Expected()
        {
            _result.Id.Should().Be(1);
        }

        [Fact]
        public void Then_The_Provider_UkPrn_Is_As_Expected()
        {
            _result.UkPrn.Should().Be(UkPrn);
        }

        [Fact]
        public void Then_The_Provider_Name_Is_As_Expected()
        {
            _result.Name.Should().Be("Test Provider");
        }
    }
}
