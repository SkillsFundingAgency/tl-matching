using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback
{
    public class When_ProviderFeedbackService_Is_Called_To_Request_Provider_Quarterly_Update
    {
        private readonly IEmailService _emailService;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IRepository<ProviderFeedbackRequestHistory> _providerFeedbackRequestHistoryRepository;

        public When_ProviderFeedbackService_Is_Called_To_Request_Provider_Quarterly_Update()
        {
            _emailService = Substitute.For<IEmailService>();
            var emailHistoryService = Substitute.For<IEmailHistoryService>();

            _messageQueueService = Substitute.For<IMessageQueueService>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            var logger = Substitute.For<ILogger<ProviderFeedbackService>>();

            var providerRepository = Substitute.For<IProviderRepository>();
            _providerFeedbackRequestHistoryRepository = Substitute.For<IRepository<ProviderFeedbackRequestHistory>>();

            _providerFeedbackRequestHistoryRepository
                .Create(Arg.Any<ProviderFeedbackRequestHistory>())
                .Returns(1);

            providerRepository
                .GetProvidersWithFundingAsync()
                .Returns(new ValidProviderWithFundingDtoListBuilder().Build());

            var providerFeedbackService = new ProviderFeedbackService(
                logger, _emailService, emailHistoryService,
                providerRepository, _providerFeedbackRequestHistoryRepository,
                _messageQueueService, dateTimeProvider);

            providerFeedbackService
                .RequestProviderQuarterlyUpdateAsync("TestUser")
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ProviderFeedbackRequestHistoryRepository_Create_Is_Called_Exactly_Once()
        {
            _providerFeedbackRequestHistoryRepository
                .Received(1)
                .Create(Arg.Is<ProviderFeedbackRequestHistory>(request =>
                    request.ProviderCount == 0 &&
                    request.Status == 1 && 
                    request.CreatedBy == "TestUser"));
        }
        
        [Fact]
        public void Then_MessageQueueService_PushProviderQuarterlyRequestMessageAsync_Is_Called_Exactly_Once()
        {
            _messageQueueService
                .Received(1)
                .PushProviderQuarterlyRequestMessageAsync(Arg.Is<SendProviderFeedbackEmail>(message =>
                    message.ProviderFeedbackRequestHistoryId == 1));
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Not_Called()
        {
            _emailService
                .DidNotReceive()
                .SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }
    }
}
