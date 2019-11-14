using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Create_Provider
    {
        private readonly int _result;
        private readonly IRepository<Domain.Models.Provider> _repository;
        private const int ProviderId = 1;

        public When_ProviderService_Is_Called_To_Create_Provider()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(ProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<CreateProviderDetailViewModel, Domain.Models.Provider>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<CreateProviderDetailViewModel, Domain.Models.Provider>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<CreateProviderDetailViewModel, Domain.Models.Provider>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            _repository = Substitute.For<IRepository<Domain.Models.Provider>>();
            var referenceRepository = Substitute.For<IRepository<ProviderReference>>();

            _repository.CreateAsync(Arg.Any<Domain.Models.Provider>())
                .Returns(ProviderId);

            var providerService = new ProviderService(mapper, _repository, referenceRepository);

            var viewModel = new CreateProviderDetailViewModel
            {
                UkPrn = 123,
                Name = "ProviderName",
                DisplayName = "Display name"
            };

            _result = providerService.CreateProviderAsync(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderId_Is_Created()
        {
            _result.Should().Be(ProviderId);
        }
        
        [Fact]
        public void Then_ProviderRepository_Create_Is_Called_Exactly_Once_With_Expected_Values()
        {
            _repository.Received(1)
                .CreateAsync(Arg.Is<Domain.Models.Provider>(
                    p => p.UkPrn == 123 &&
                         p.Name == "ProviderName" &&
                         p.DisplayName == "Display Name"));
        }
    }
}