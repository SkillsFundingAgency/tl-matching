using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback
{
    public class When_ProviderFeedbackService_Is_Called_To_Send_Provider_Quarterly_Update_Emails_And_Status_Is_Processing
    {
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IProviderRepository _providerRepository;
        private readonly IRepository<ProviderFeedbackRequestHistory> _providerFeedbackRequestHistoryRepository;

        public When_ProviderFeedbackService_Is_Called_To_Send_Provider_Quarterly_Update_Emails_And_Status_Is_Processing()
        {
            var configuration = new MatchingConfiguration
            {
                SendEmailEnabled = true,
                NotificationsSystemId = "TLevelsIndustryPlacement"
            };

            _emailService = Substitute.For<IEmailService>();
            _emailHistoryService = Substitute.For<IEmailHistoryService>();

            var messageQueueService = Substitute.For<IMessageQueueService>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            var logger = Substitute.For<ILogger<ProviderFeedbackService>>();

            _providerRepository = Substitute.For<IProviderRepository>();
            _providerRepository
                .GetProvidersWithFundingAsync()
                .Returns(new ValidProviderWithFundingDtoListBuilder().Build());

            var providerFeedbackRequestHistory = new ProviderFeedbackRequestHistoryBuilder().Build();
            providerFeedbackRequestHistory.Status = (int) ProviderFeedbackRequestStatus.Processing;

            _providerFeedbackRequestHistoryRepository = Substitute.For<IRepository<ProviderFeedbackRequestHistory>>();
            _providerFeedbackRequestHistoryRepository
                .GetSingleOrDefault(Arg.Any<Expression<Func<ProviderFeedbackRequestHistory, bool>>>())
                .Returns(providerFeedbackRequestHistory);

            var providerFeedbackService = new ProviderFeedbackService(
                    configuration, logger, 
                    _emailService, _emailHistoryService,
                    _providerRepository, _providerFeedbackRequestHistoryRepository,
                    messageQueueService, dateTimeProvider);

            providerFeedbackService
                .SendProviderQuarterlyUpdateEmailsAsync(1, "TestUser")
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderRepository_GetProvidersWithFundingAsync_Is_Not_Called()
        {
            _providerRepository
                .DidNotReceive()
                .GetProvidersWithFundingAsync();
        }

        [Fact]
        public void Then_ProviderFeedbackRequestHistoryRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _providerFeedbackRequestHistoryRepository
                .Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<ProviderFeedbackRequestHistory, bool>>>());
        }
        
        [Fact]
        public void Then_ProviderFeedbackRequestHistoryRepository_Update_Is_Not_Called()
        {
            _providerFeedbackRequestHistoryRepository
                .DidNotReceiveWithAnyArgs()
                .Update(Arg.Any<ProviderFeedbackRequestHistory>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Not_Called()
        {
            _emailService
                .DidNotReceiveWithAnyArgs()
                .SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailHistoryService_SaveEmailHistory_Is_Not_Called()
        {
            _emailHistoryService
                .DidNotReceiveWithAnyArgs()
                .SaveEmailHistory(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<int?>(), Arg.Any<string>(), Arg.Any<string>());
        }
    }
}
