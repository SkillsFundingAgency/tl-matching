using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Models.ViewModel;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderFeedbackService_Is_Called_To_UpdateProviderFeedback
    {
        private readonly IProviderRepository _providerRepository;

        public When_ProviderFeedbackService_Is_Called_To_UpdateProviderFeedback()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(ProviderMapper).Assembly));
            var mapper = new Mapper(config);

            _providerRepository = Substitute.For<IProviderRepository>();

            var service = new ProviderFeedbackService(mapper, _providerRepository);
            var viewModel = new SaveProviderFeedbackViewModel
            {
                Providers = new List<ProviderSearchResultItemViewModel>()
            };

            service.UpdateProviderFeedback(viewModel).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_UpdateManyWithSpecifedColumnsOnly_Is_Called_Exactly_Once()
        {
            _providerRepository.Received(1).UpdateManyWithSpecifedColumnsOnly(
                Arg.Any<List<Domain.Models.Provider>>(),
                Arg.Any<Expression<Func<Domain.Models.Provider, object>>>());
        }
    }
}