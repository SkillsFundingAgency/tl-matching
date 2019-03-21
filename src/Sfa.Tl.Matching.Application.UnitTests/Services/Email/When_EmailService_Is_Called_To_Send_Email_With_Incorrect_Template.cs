using System;
using System.Collections.Generic;
using NSubstitute;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using SFA.DAS.Notifications.Api.Client;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Email
{
    public class When_EmailService_Is_Called_To_Send_Email_With_Incorrect_Template
    {
        private readonly ILogger<EmailService> _logger;
        private readonly INotificationsApi _notificationsApi;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        private readonly string _subject;
        private readonly string _toAddress;
        private readonly string _replyToAddress;

        public When_EmailService_Is_Called_To_Send_Email_With_Incorrect_Template()
        {
            var configuration = new MatchingConfiguration
            {
                SendEmailEnabled = true
            };

            _notificationsApi = Substitute.For<INotificationsApi>();

            _logger = Substitute.For<ILogger<EmailService>>();

            _emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();
            _emailTemplateRepository
                .GetSingleOrDefault(Arg.Any<Expression<Func<EmailTemplate, bool>>>())
                .ReturnsNull();

            var emailService = new EmailService(configuration, _notificationsApi, _emailTemplateRepository, _logger);

            _subject = "A test email";
            _toAddress = "test@test.com";
            _replyToAddress = "reply@test.com";
            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };

            var templateName = "MissingTestTemplate";

            emailService.SendEmail(templateName, _toAddress, _subject, tokens, _replyToAddress).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_EmailTemplateRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _emailTemplateRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.EmailTemplate, bool>>>());
        }

        [Fact]
        public void Then_EmailTemplateRepository_Logger_Is_Called_Exactly_Once()
        {
            _logger.Received(1).Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Any<object>(),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }
        
        [Fact]
        public void Then_EmailTemplateRepository_Logger_Is_Called_With_Expected_Message()
        {
            _logger.Received(1).Log(
                LogLevel.Warning,
                Arg.Any<EventId>(),
                Arg.Is<object>(o => o.ToString().Contains("Email template MissingTestTemplate not found. No emails sent.")),
                null,
                Arg.Any<Func<object, Exception, string>>());
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Not_Called()
        {
            _notificationsApi.DidNotReceive().SendEmail(Arg.Any<SFA.DAS.Notifications.Api.Types.Email>());
        }

    }
}
