using System;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Unhide_Provider
    {
        private const int ProviderId = 1;

        private readonly IRepository<Domain.Models.Provider> _providerRepository;

        public When_ProviderService_Is_Called_To_Unhide_Provider()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);
            _providerRepository = Substitute.For<IRepository<Domain.Models.Provider>>();

            _providerRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>())
                .Returns(new ValidProviderBuilder().Build());

            var service = new ProviderService(mapper, _providerRepository);

            service.SetIsProviderEnabledAsync(ProviderId, true).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerRepository.Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>());
        }

        [Fact]
        public void Then_ProviderRepository_Update_Is_Called_Exactly_Once()
        {
            _providerRepository.Received(1)
                .Update(Arg.Any<Domain.Models.Provider>());
        }

        [Fact]
        public void Then_ProviderRepository_Update_Is_Called_With_Expected_Provider_Id()
        {
            _providerRepository.Received(1)
                .Update(Arg.Is<Domain.Models.Provider>(
                    p =>
                        p.Id == ProviderId));
        }

        [Fact]
        public void Then_ProviderRepository_Update_Is_Called_With_Expected_IsEnabledForSearch()
        {
            _providerRepository.Received(1)
                .Update(Arg.Is<Domain.Models.Provider>(
                    p => p.IsEnabledForSearch));
        }
    }
}
