using System.Collections.Generic;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback
{
    public class When_ProviderQuarterlyUpdateEmailService_Is_Called_To_Request_Provider_Quarterly_Update
        : IClassFixture<ProviderQuarterlyUpdateEmailFixture>
    {
        private readonly IEmailService _emailService;
        private readonly IMessageQueueService _messageQueueService;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;

        public When_ProviderQuarterlyUpdateEmailService_Is_Called_To_Request_Provider_Quarterly_Update(ProviderQuarterlyUpdateEmailFixture testFixture)
        {
            _emailService = Substitute.For<IEmailService>();
            var emailHistoryService = Substitute.For<IEmailHistoryService>();

            _messageQueueService = Substitute.For<IMessageQueueService>();

            var providerRepository = Substitute.For<IProviderRepository>();
            _backgroundProcessHistoryRepository = Substitute.For<IRepository<BackgroundProcessHistory>>();

            _backgroundProcessHistoryRepository
                .Create(Arg.Any<BackgroundProcessHistory>())
                .Returns(1);

            providerRepository
                .GetProvidersWithFundingAsync()
                .Returns(new ValidProviderWithFundingDtoListBuilder().Build());

            var providerFeedbackService = new ProviderQuarterlyUpdateEmailService(
                testFixture.Configuration, testFixture.Logger, 
                _emailService, emailHistoryService,
                providerRepository, _backgroundProcessHistoryRepository,
                _messageQueueService, testFixture.DateTimeProvider);

            providerFeedbackService
                .RequestProviderQuarterlyUpdateAsync("TestUser")
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_BackgroundProcessHistoryRepository_Create_Is_Called_Exactly_Once()
        {
            _backgroundProcessHistoryRepository
                .Received(1)
                .Create(Arg.Is<BackgroundProcessHistory>(request =>
                    request.RecordCount == 0 &&
                    request.Status == BackgroundProcessHistoryStatus.Pending.ToString() && 
                    request.CreatedBy == "TestUser"));
        }
        
        [Fact]
        public void Then_MessageQueueService_PushProviderQuarterlyRequestMessageAsync_Is_Called_Exactly_Once()
        {
            _messageQueueService
                .Received(1)
                .PushProviderQuarterlyRequestMessageAsync(Arg.Is<SendProviderFeedbackEmail>(message =>
                    message.BackgroundProcessHistoryId == 1));
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
