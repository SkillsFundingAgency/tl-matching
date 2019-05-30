using System;
using System.Linq.Expressions;
using AutoMapper;
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
    public class When_ProviderService_Is_Called_To_Update_Provider_Detail
    {
        private const int ProviderId = 1;
        private readonly IRepository<Domain.Models.Provider> _repository;

        public When_ProviderService_Is_Called_To_Update_Provider_Detail()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(ProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserEmailResolver") ?
                        new LoggedInUserEmailResolver<ProviderDetailViewModel, Domain.Models.Provider>(httpcontextAccesor) :
                        type.Name.Contains("LoggedInUserNameResolver") ?
                            (object)new LoggedInUserNameResolver<ProviderDetailViewModel, Domain.Models.Provider>(httpcontextAccesor) :
                            type.Name.Contains("UtcNowResolver") ?
                                new UtcNowResolver<ProviderDetailViewModel, Domain.Models.Provider>(new DateTimeProvider()) :
                                null);
            });
            var mapper = new Mapper(config);

            _repository = Substitute.For<IRepository<Domain.Models.Provider>>();
            var referenceRepository = Substitute.For<IRepository<ProviderReference>>();

            _repository.Create(Arg.Any<Domain.Models.Provider>())
                .Returns(ProviderId);

            var providerService = new ProviderService(mapper, _repository, referenceRepository);

            var viewModel = new ProviderDetailViewModel
            {
                UkPrn = 123,
                Name = "ProviderName"
            };

            providerService.UpdateProviderDetail(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _repository.Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>());
        }

        [Fact]
        public void Then_ProviderRepository_Update_Is_Called_Exactly_Once()
        {
            _repository.Received(1)
                .Update(Arg.Any<Domain.Models.Provider>());
        }
    }
}