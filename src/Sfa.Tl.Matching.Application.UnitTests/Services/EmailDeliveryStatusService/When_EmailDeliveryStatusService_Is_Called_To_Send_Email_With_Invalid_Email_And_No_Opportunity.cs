using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.UnitTests.Services.EmailDeliveryStatusService.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmailDeliveryStatusService
{
    public class When_EmailDeliveryStatusService_Is_Called_To_Send_Email_With_Invalid_Email_And_No_Opportunity
    {
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly Guid _notificationId = new Guid("a8de2d8c-23ae-4c2f-980a-0a8a3231938f");
        private const string SupportEmailAddress = "support@service.com";

        public When_EmailDeliveryStatusService_Is_Called_To_Send_Email_With_Invalid_Email_And_No_Opportunity()
        {
            var configuration = new MatchingConfiguration
            {
                MatchingServiceSupportEmailAddress = SupportEmailAddress
            };

            var logger = Substitute.For<ILogger<Application.Services.EmailDeliveryStatusService>>();

            _emailService = Substitute.For<IEmailService>();
            _emailService.GetFailedEmailAsync(_notificationId).Returns(
                new FailedEmailDto
                {
                    Body = "Body",
                    Subject = "Subject",
                    FailedEmailType = FailedEmailType.PermanentFailure
                });

            _emailService.GetEmailHistoryAsync(_notificationId).Returns(
                new EmailHistoryDto
                {
                    NotificationId = _notificationId,
                    OpportunityId = null,
                    SentTo = "sent-to@email.com",
                    Status = "permanent-failure",
                    EmailTemplateId = 11,
                    EmailTemplateName = "EmployerAupaBlank",
                    CreatedBy = "CreatedBy"
                });

            var messageQueueService = Substitute.For<IMessageQueueService>();

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityRepository.GetFailedEmployerEmailAsync(1, "sent-to@email.com").Returns(
                new EmailBodyDtoBuilder()
                    .AddEmployerEmail().Build());

            var emailDeliveryStatusService = new Application.Services.EmailDeliveryStatusService(configuration,
                _emailService,
                _opportunityRepository,
                messageQueueService,
                logger);

            emailDeliveryStatusService.SendEmailDeliveryStatusAsync(_notificationId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmailService_GetFailedEmailAsync_Is_Called_Exactly_Once()
        {
            _emailService.Received(1).GetFailedEmailAsync(Arg.Any<Guid>());
        }

        [Fact]
        public void Then_EmailHistoryService_GetFailedEmailAsync_Is_Called_Exactly_Once()
        {
            _emailService.Received(1).GetEmailHistoryAsync(_notificationId);
        }

        [Fact]
        public void Then_OpportunityRepository_GetFailedEmployerEmailAsync_Is_Called_Exactly_Once()
        {
            _opportunityRepository.DidNotReceive().GetFailedEmployerEmailAsync(Arg.Any<int>(),
                Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmailAsync_Is_Called_Exactly_Once()
        {
            _emailService.Received(1).SendEmailAsync(null,
                EmailTemplateName.FailedEmailV2.ToString(),
                SupportEmailAddress,
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("summary") && tokens["summary"] == "There was an error sending an email from the industry placement matching service."
                    && tokens.ContainsKey("email_type") && tokens["email_type"] == "employer aupa blank"
                    && tokens.ContainsKey("reason") && tokens["reason"] == "Email address does not exist"
                    && tokens.ContainsKey("sender_username") && tokens["sender_username"] == "CreatedBy"
                    && tokens.ContainsKey("failed_email_body") && tokens["failed_email_body"] == "Body")
                , Arg.Any<string>());
        }
    }
}