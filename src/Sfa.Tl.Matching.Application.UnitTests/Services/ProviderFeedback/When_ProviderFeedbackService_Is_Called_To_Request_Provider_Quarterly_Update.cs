using System.Collections.Generic;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback
{
    public class When_ProviderFeedbackService_Is_Called_To_Request_Provider_Quarterly_Update
    {
        public When_ProviderFeedbackService_Is_Called_To_Request_Provider_Quarterly_Update()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmailHistoryMapper).Assembly));
            var mapper = new Mapper(config);
            var emailService = Substitute.For<IEmailService>();
            var logger = Substitute.For<ILogger<ProviderFeedbackService>>();
            var messageQueueService = Substitute.For<IMessageQueueService>();

            var providerRepository = Substitute.For<IProviderRepository>();
            var providerFeedbackRequestHistoryRepository = Substitute.For<IRepository<ProviderFeedbackRequestHistory>>();

            providerRepository
                .GetProvidersWithFundingAsync()
                .Returns(new ValidProviderWithFundingDtoListBuilder().Build());

            var providerFeedbackService = new ProviderFeedbackService(emailService,
                providerRepository, providerFeedbackRequestHistoryRepository,
                messageQueueService, mapper, logger);

            providerFeedbackService.RequestProviderQuarterlyUpdateAsync("TestUser").GetAwaiter().GetResult();
        }
    }
}
