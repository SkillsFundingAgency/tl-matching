using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.FailedEmail
{
    public class When_FailedEmailService_Is_Called_To_Send_Email
    {
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;

        private readonly Guid _notificationId = new Guid("a8de2d8c-23ae-4c2f-980a-0a8a3231938f");

        public When_FailedEmailService_Is_Called_To_Send_Email()
        {
            var configuration = new MatchingConfiguration
            {
                MatchingServiceSupportEmailAddress = "support@service.com"
            };

            var logger = Substitute.For<ILogger<FailedEmailService>>();

            _emailService = Substitute.For<IEmailService>();
            _emailService.GetFailedEmailAsync(_notificationId).Returns(
                new FailedEmailDto
                {
                    Body = "Body",
                    Reason = "Reason",
                    Subject = "Subject"
                });

            _emailService.GetEmailHistoryAsync(_notificationId).Returns(
                new EmailHistoryDto
                {
                    NotificationId = _notificationId,
                    OpportunityId = 1,
                    SentTo = "sent-to@email.com",
                    EmailTemplateId = (int)EmailTemplateName.EmployerReferralV3,
                    CreatedBy = "CreatedBy"
                });

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityRepository.GetFailedOpportunityEmailAsync(1, "sent-to@email.com").Returns(
                new FailedEmailBodyDto
                {
                    PrimaryContactEmail = "primary-contact@email.com",
                    SecondaryContactEmail = "secondary-contact@email.com",
                    ProviderDisplayName = "Provider Display Name",
                    ProviderVenueName = "Provider Venue Name",
                    EmployerEmail = "employer@email.com",
                    ProviderVenuePostcode = "AB1 1AA"
                });

            var sendFailedEmailData = new SendFailedEmail
            {
                NotificationId = _notificationId
            };

            var failedEmailService = new FailedEmailService(configuration,
                _emailService,
                _opportunityRepository,
                logger);

            failedEmailService.SendFailedEmailAsync(sendFailedEmailData).GetAwaiter().GetResult();
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
        public void Then_OpportunityRepository_GetFailedOpportunityEmailAsync_Is_Called_Exactly_Once()
        {
            _opportunityRepository.Received(1).GetFailedOpportunityEmailAsync(1, "sent-to@email.com");
        }

        [Fact]
        public void Then_EmailService_SendEmailAsync_Is_Called_Exactly_Once()
        {
            _emailService.Received(1).SendEmailAsync(Arg.Any<int?>(), Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<IDictionary<string, string>>(tokens =>
                    tokens.ContainsKey("email_type") && tokens["email_type"] == "Employer referral v 3"
                    && tokens.ContainsKey("reason") && tokens["reason"] == "Reason"
                    && tokens.ContainsKey("sender_username") && tokens["sender_username"] == "CreatedBy"
                    && tokens.ContainsKey("failed_email_body") && tokens["failed_email_body"] == "Body")
                , Arg.Any<string>());
        }
    }
}