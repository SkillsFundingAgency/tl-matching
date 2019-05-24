using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NSubstitute;
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
        : IClassFixture<ProviderFeedbackFixture>
    {
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IProviderRepository _providerRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;

        public When_ProviderFeedbackService_Is_Called_To_Send_Provider_Quarterly_Update_Emails_And_Status_Is_Processing(ProviderFeedbackFixture testFixture)
        {
            _emailService = Substitute.For<IEmailService>();
            _emailHistoryService = Substitute.For<IEmailHistoryService>();

            var messageQueueService = Substitute.For<IMessageQueueService>();

            _providerRepository = Substitute.For<IProviderRepository>();
            _providerRepository
                .GetProvidersWithFundingAsync()
                .Returns(new ValidProviderWithFundingDtoListBuilder().Build());

            var backgroundProcessHistory = new BackgroundProcessHistoryBuilder().Build();
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Processing.ToString();

            _backgroundProcessHistoryRepository = Substitute.For<IRepository<BackgroundProcessHistory>>();
            _backgroundProcessHistoryRepository
                .GetSingleOrDefault(Arg.Any<Expression<Func<BackgroundProcessHistory, bool>>>())
                .Returns(backgroundProcessHistory);

            var providerFeedbackService = new ProviderFeedbackService(
                testFixture.Configuration, testFixture.Logger,
                    _emailService, _emailHistoryService,
                    _providerRepository, _backgroundProcessHistoryRepository,
                    messageQueueService, testFixture.DateTimeProvider);

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
        public void Then_BackgroundProcessHistoryRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _backgroundProcessHistoryRepository
                .Received(1)
                .GetSingleOrDefault(Arg.Any<Expression<Func<BackgroundProcessHistory, bool>>>());
        }
        
        [Fact]
        public void Then_BackgroundProcessHistoryRepository_Update_Is_Not_Called()
        {
            _backgroundProcessHistoryRepository
                .DidNotReceiveWithAnyArgs()
                .Update(Arg.Any<BackgroundProcessHistory>());
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
