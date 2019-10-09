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
    public class When_ProviderService_Is_Called_To_Search_Providers_By_UkPrn
    {
        private const long UkPrn = 10000546;

        private readonly ProviderSearchResultDto _result;
        private readonly IRepository<Domain.Models.Provider> _providerRepository;

        public When_ProviderService_Is_Called_To_Search_Providers_By_UkPrn()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);
            _providerRepository = Substitute.For<IRepository<Domain.Models.Provider>>();

            _providerRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>())
                .Returns(new ValidProviderBuilder().Build());

            var providerReferenceRepository = Substitute.For<IRepository<ProviderReference>>();

            var service = new ProviderService(mapper, _providerRepository, providerReferenceRepository);

            _result = service.SearchAsync(UkPrn).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerRepository.Received(1)
                .GetSingleOrDefaultAsync(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>());
        }
        
        [Fact]
        public void Then_The_Fields_Are_As_Expected()
        {
            _result.Id.Should().Be(1);
            _result.UkPrn.Should().Be(UkPrn);
            _result.Name.Should().Be("Test Provider");
        }
    }
}
