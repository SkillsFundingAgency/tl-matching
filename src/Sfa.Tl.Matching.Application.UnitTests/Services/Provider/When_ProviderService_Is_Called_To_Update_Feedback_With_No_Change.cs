using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Extensions;
using Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Update_Feedback_With_No_Change
    {
        private readonly IRepository<Domain.Models.Provider> _providerRepository;

        public When_ProviderService_Is_Called_To_Update_Feedback_With_No_Change()
        {
            var httpcontextAccesor = Substitute.For<IHttpContextAccessor>();

            var config = new MapperConfiguration(c =>
            {
                c.AddProfiles(typeof(ProviderMapper).Assembly);
                c.ConstructServicesUsing(type =>
                    type.Name.Contains("LoggedInUserNameResolver") ?
                        (object)new LoggedInUserNameResolver<ProviderSearchResultItemViewModel, Domain.Models.Provider>(httpcontextAccesor) :
                        type.Name.Contains("UtcNowResolver") ?
                            new UtcNowResolver<ProviderSearchResultItemViewModel, Domain.Models.Provider>(new DateTimeProvider()) :
                            null);
            });
            var mapper = new Mapper(config);

            var providers = new FakeAsyncEnumerable<Domain.Models.Provider>(new List<Domain.Models.Provider> { new ValidProviderBuilder().Build() });

            _providerRepository = Substitute.For<IRepository<Domain.Models.Provider>>();
            _providerRepository.GetMany(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>()).Returns(providers);

            var service = new ProviderService(mapper, _providerRepository);

            var viewModel = new SaveProviderFeedbackViewModel
            {
                Providers = new List<ProviderSearchResultItemViewModel>
                {
                    new ProviderSearchResultItemViewModel
                    {
                        ProviderId = 1,
                        //IsFundedForNextYear = true
                    }
                }
            };

            service.UpdateProvider(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_UpdateWithSpecifedColumnsOnly_Is_Not_Called()
        {
            _providerRepository.DidNotReceive().UpdateManyWithSpecifedColumnsOnly(
                Arg.Any<List<Domain.Models.Provider>>(),
                Arg.Any<Expression<Func<Domain.Models.Provider, object>>[]>());
        }
    }
}