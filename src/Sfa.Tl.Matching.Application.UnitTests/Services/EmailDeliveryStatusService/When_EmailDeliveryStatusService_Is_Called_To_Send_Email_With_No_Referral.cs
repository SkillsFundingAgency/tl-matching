using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmailDeliveryStatusService
{
    public class When_EmailDeliveryStatusService_Is_Called_To_Send_Email_With_No_Referral
    {
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly Guid _notificationId = new("a8de2d8c-23ae-4c2f-980a-0a8a3231938f");
        public const int OpportunityId = 1;
        public const int OpportunityItemId = 2;
        private const string SupportEmailAddress = "support@service.com";

        public When_EmailDeliveryStatusService_Is_Called_To_Send_Email_With_No_Referral()
        {
            var configuration = new MatchingConfiguration
            {
                MatchingServiceSupportEmailAddress = SupportEmailAddress
            };

            var logger = Substitute.For<ILogger<Application.Services.EmailDeliveryStatusService>>();

            _emailService = Substitute.For<IEmailService>();
            _emailService.GetEmailBodyFromNotifyClientAsync(_notificationId).Returns(
                new EmailDeliveryStatusDto
                {
                    Body = "Body",
                    Subject = "Subject",
                    EmailDeliveryStatusType = EmailDeliveryStatusType.PermanentFailure
                });

            _emailService.GetEmailHistoryAsync(_notificationId).Returns(
                new EmailHistoryDto
                {
                    NotificationId = _notificationId,
                    OpportunityId = OpportunityId,
                    OpportunityItemId = OpportunityItemId,
                    SentTo = "sent-to@email.com",
                    Status = "permanent-failure",
                    EmailTemplateId = 14,
                    EmailTemplateName = "EmployerReferralV6",
                    CreatedBy = "CreatedBy"
                });

            var messageQueueService = Substitute.For<IMessageQueueService>();

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityRepository.GetEmailDeliveryStatusForEmployerAsync(1, "sent-to@email.com").ReturnsNull();

            var emailDeliveryStatusService = new Application.Services.EmailDeliveryStatusService(configuration,
                _emailService,
                _opportunityRepository,
                messageQueueService,
                logger);

            emailDeliveryStatusService.SendEmailDeliveryStatusAsync(_notificationId).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmailService_GetEmailDeliveryStatusAsync_Is_Called_Exactly_Once()
        {
            _emailService.Received(1).GetEmailBodyFromNotifyClientAsync(Arg.Any<Guid>());
        }

        [Fact]
        public void Then_EmailHistoryService_GetEmailDeliveryStatusAsync_Is_Called_Exactly_Once()
        {
            _emailService.Received(1).GetEmailHistoryAsync(_notificationId);
        }

        [Fact]
        public void Then_OpportunityRepository_GetEmailDeliveryStatusForEmployerAsync_Is_Called_Exactly_Once()
        {
            _opportunityRepository.Received(1).GetEmailDeliveryStatusForEmployerAsync(OpportunityId, "sent-to@email.com");
        }

        [Fact]
        public void Then_EmailService_SendEmailAsync_Is_Called_Exactly_Once()
        {
            _emailService.Received(1).SendEmailAsync(EmailTemplateName.EmailDeliveryStatus.ToString(), SupportEmailAddress, OpportunityId, OpportunityItemId, 
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("summary") && tokens["summary"] == "There was an error sending an email from the industry placement matching service."
                                                  && tokens.ContainsKey("email_type") && tokens["email_type"] == "employer referral confirmation"
                                                  && tokens.ContainsKey("body") && tokens["body"] == string.Empty
                                                  && tokens.ContainsKey("reason") && tokens["reason"] == "Email address does not exist"
                                                  && tokens.ContainsKey("sender_username") && tokens["sender_username"] == "CreatedBy"
                                                  && tokens.ContainsKey("email_body") && tokens["email_body"] == "Body"), Arg.Any<string>());
        }
    }
}