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
    public class When_EmailDeliveryStatusService_Is_Called_To_Send_Email_With_Inbox_Full
    {
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly Guid _notificationId = new Guid("a8de2d8c-23ae-4c2f-980a-0a8a3231938f");
        private const int OpportunityId = 1;
        private const int OpportunityItemId = 2;
        private const string SupportEmailAddress = "support@service.com";

        public When_EmailDeliveryStatusService_Is_Called_To_Send_Email_With_Inbox_Full()
        {
            var configuration = new MatchingConfiguration
            {
                MatchingServiceSupportEmailAddress = "support@service.com"
            };

            var logger = Substitute.For<ILogger<Application.Services.EmailDeliveryStatusService>>();

            _emailService = Substitute.For<IEmailService>();
            _emailService.GetEmailBodyFromNotifyClientAsync(_notificationId).Returns(
                new EmailDeliveryStatusDto
                {
                    Body = "Body",
                    Subject = "Subject",
                    EmailDeliveryStatusType = EmailDeliveryStatusType.TemporaryFailure
                });

            _emailService.GetEmailHistoryAsync(_notificationId).Returns(
                new EmailHistoryDto
                {
                    NotificationId = _notificationId,
                    OpportunityId = OpportunityId,
                    OpportunityItemId = OpportunityItemId,
                    SentTo = "sent-to@email.com",
                    Status = "unknown-failure",
                    EmailTemplateId = 7,
                    EmailTemplateName = "ProviderReferralV5",
                    CreatedBy = "CreatedBy"
                });

            var messageQueueService = Substitute.For<IMessageQueueService>();

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityRepository.GetEmailDeliveryStatusForProviderAsync(OpportunityId, "sent-to@email.com").Returns(
                new EmailBodyDtoBuilder().AddProviderEmail().Build());

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
        public void Then_OpportunityRepository_GetDeliveryStatusOpportunityEmailAsync_Is_Called_Exactly_Once()
        {
            _opportunityRepository.Received(1).GetEmailDeliveryStatusForProviderAsync(OpportunityId, "sent-to@email.com");
        }

        [Fact]
        public void Then_EmailService_SendEmailAsync_Is_Called_Exactly_Once()
        {
            _emailService.Received(1).SendEmailAsync(EmailTemplateName.EmailDeliveryStatus.ToString(), SupportEmailAddress, OpportunityId, OpportunityItemId, 
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("summary") && tokens["summary"] == "We cannot determine whether or not the following email was sent."
                                                  && tokens.ContainsKey("email_type") && tokens["email_type"] == "provider referral"
                                                  && tokens.ContainsKey("body") && tokens["body"] == "Provider name: Provider Venue Name\r\nProvider primary contact: primary-contact@email.com\r\nProvider secondary contact: secondary-contact@email.com\r\n"
                                                  && tokens.ContainsKey("reason") && tokens["reason"] == "Inbox not accepting messages right now"
                                                  && tokens.ContainsKey("sender_username") && tokens["sender_username"] == "CreatedBy"
                                                  && tokens.ContainsKey("email_body") && tokens["email_body"] == "Body"), Arg.Any<string>());
        }
    }
}