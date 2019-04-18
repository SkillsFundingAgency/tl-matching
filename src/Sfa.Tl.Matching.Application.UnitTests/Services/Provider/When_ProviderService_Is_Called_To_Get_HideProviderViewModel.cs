using System;
using System.Linq.Expressions;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Get_HideProviderViewModel
    {
        private readonly HideProviderViewModel _result;
        private readonly IRepository<Domain.Models.Provider> _providerRepository;

        public When_ProviderService_Is_Called_To_Get_HideProviderViewModel()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);
            _providerRepository = Substitute.For<IRepository<Domain.Models.Provider>>();

            _providerRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>())
                .Returns(new ValidProviderBuilder().Build());

            var service = new ProviderService(mapper, _providerRepository);

            _result = service.GetHideProviderViewModelAsync(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerRepository.Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>());
        }
        
        [Fact]
        public void Then_The_ProviderId_Is_As_Expected()
        {
            _result.ProviderId.Should().Be(1);
        }

        [Fact]
        public void Then_The_UkPrn_Is_As_Expected()
        {
            _result.UkPrn.Should().Be(10000546);
        }

        [Fact]
        public void Then_The_ProviderName_Is_As_Expected()
        {
            _result.ProviderName.Should().Be("Test Provider");
        }

        [Fact]
        public void Then_The_Provider_IsEnabledForSearch_Is_As_Expected()
        {
            _result.IsEnabledForSearch.Should().Be(true);
        }
    }
}
