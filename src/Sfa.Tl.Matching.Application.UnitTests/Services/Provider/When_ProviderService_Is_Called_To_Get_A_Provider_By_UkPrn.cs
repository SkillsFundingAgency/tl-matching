using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Get_A_Provider_By_UkPrn
    {
        private const long UkPrn = 10000546;

        private readonly ProviderDto _result;
        private readonly IRepository<Domain.Models.Provider> _providerRepository;

        public When_ProviderService_Is_Called_To_Get_A_Provider_By_UkPrn()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);
            _providerRepository = Substitute.For<IRepository<Domain.Models.Provider>>();

            _providerRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>())
                .Returns(new ValidProviderBuilder().Build());

            var service = new ProviderService(mapper, _providerRepository);

            _result = service.GetProviderByUkPrnAsync(UkPrn).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerRepository.Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>());
        }
        
        [Fact]
        public void Then_The_Provider_Id_Is_As_Expected()
        {
            _result.Id.Should().Be(1);
        }

        [Fact]
        public void Then_The_Provider_UkPrn_Is_As_Expected()
        {
            _result.UkPrn.Should().Be(10000546);
        }

        [Fact]
        public void Then_The_Provider_Name_Is_As_Expected()
        {
            _result.Name.Should().Be("Test Provider");
        }

        [Fact]
        public void Then_The_Provider_IsEnabledForReferral_Is_As_Expected()
        {
            _result.IsEnabledForReferral.Should().Be(true);
        }

        [Fact]
        public void Then_The_Provider_IsEnabledForSearch_Is_As_Expected()
        {
            _result.IsEnabledForSearch.Should().Be(true);
        }

        [Fact]
        public void Then_The_Provider_Status_Is_As_Expected()
        {
            _result.Status.Should().Be(true);
        }
    }
}
