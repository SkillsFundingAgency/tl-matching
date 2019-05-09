using System;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Unhide_Provider
    {
        private readonly IRepository<Domain.Models.Provider> _providerRepository;

        public When_ProviderService_Is_Called_To_Unhide_Provider()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(ProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserNameResolver") ?
                        (object)new LoggedInUserNameResolver<HideProviderViewModel, Domain.Models.Provider>(httpcontextAccesor) :
                        type.Name.Contains("UtcNowResolver") ?
                            new UtcNowResolver<HideProviderViewModel, Domain.Models.Provider>(new DateTimeProvider()) :
                            null);
            });
            var mapper = new Mapper(config);

            _providerRepository = Substitute.For<IRepository<Domain.Models.Provider>>();

            _providerRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>())
                .Returns(new ValidProviderBuilder().Build());

            var service = new ProviderService(mapper, _providerRepository);

            var viewModel = new HideProviderViewModel
            {
                ProviderId = 1,
                IsCdfProvider = true
            };
            service.UpdateProviderAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_UpdateWithSpecifedColumnsOnly_Is_Called_Exactly_Once()
        {
            _providerRepository.Received(1)
                .UpdateWithSpecifedColumnsOnly(Arg.Any<Domain.Models.Provider>(),
                    Arg.Any<Expression<Func<Domain.Models.Provider, object>>[]>());
        }
        
        [Fact]
        public void Then_ProviderRepository_UpdateWithSpecifedColumnsOnly_Is_Called_With_Expected_Values()
        {
            _providerRepository.Received(1)
                .UpdateWithSpecifedColumnsOnly(Arg.Is<Domain.Models.Provider>(
                        p =>
                            p.Id == 1 &&
                            p.IsCdfProvider
                    ),
                    Arg.Any<Expression<Func<Domain.Models.Provider, object>>[]>());
        }
    }
}
