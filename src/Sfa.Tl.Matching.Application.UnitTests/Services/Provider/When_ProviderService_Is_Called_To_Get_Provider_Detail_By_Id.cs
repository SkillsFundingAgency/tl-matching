using System;
using System.Collections.Generic;
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
    public class When_ProviderService_Is_Called_To_Get_Provider_Detail_By_Id
    {
        private readonly ProviderDetailViewModel _result;
        private readonly IRepository<Domain.Models.Provider> _providerRepository;

        public When_ProviderService_Is_Called_To_Get_Provider_Detail_By_Id()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);
            _providerRepository = Substitute.For<IRepository<Domain.Models.Provider>>();

            _providerRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>())
                .Returns(new ValidProviderBuilder().Build());

            var providers = new FakeAsyncEnumerable<Domain.Models.Provider>(new List<Domain.Models.Provider> { new ValidProviderBuilder().Build() });
            _providerRepository.GetMany(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>()).Returns(providers);

            var service = new ProviderService(mapper, _providerRepository);

            _result = service.GetProviderDetailByIdAsync(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_GetMany_Is_Called_Exactly_Once()
        {
            _providerRepository.Received(1).GetMany(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>());
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
        public void Then_The_Provider_DisplayName_Is_As_Expected()
        {
            _result.DisplayName.Should().Be("Test Provider Display Name");
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
        public void Then_The_Provider_PrimaryContact_Is_As_Expected()
        {
            _result.PrimaryContact.Should().Be("Test");
        }

        [Fact]
        public void Then_The_Provider_PrimaryContactEmail_Is_As_Expected()
        {
            _result.PrimaryContactEmail.Should().Be("Test@test.com");
        }

        [Fact]
        public void Then_The_Provider_PrimaryContactPhone_Is_As_Expected()
        {
            _result.PrimaryContactPhone.Should().Be("0123456789");
        }

        [Fact]
        public void Then_The_Provider_SecondaryContact_Is_As_Expected()
        {
            _result.SecondaryContact.Should().Be("Test 2");
        }

        [Fact]
        public void Then_The_Provider_SecondaryContactEmail_Is_As_Expected()
        {
            _result.SecondaryContactEmail.Should().Be("Test2@test.com");
        }

        [Fact]
        public void Then_The_Provider_SecondaryContactPhone_Is_As_Expected()
        {
            _result.SecondaryContactPhone.Should().Be("0123456789");
        }
    }
}
