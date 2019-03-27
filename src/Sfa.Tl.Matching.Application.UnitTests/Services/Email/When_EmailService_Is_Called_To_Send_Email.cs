using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using SFA.DAS.Notifications.Api.Client;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Email
{
    public class When_EmailService_Is_Called_To_Send_Email
    {
        private readonly INotificationsApi _notificationsApi;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        private readonly string _subject;
        private readonly string _toAddress;
        private readonly string _replyToAddress;

        public When_EmailService_Is_Called_To_Send_Email()
        {
            var configuration = new MatchingConfiguration
            {
                SendEmailEnabled = true
            };

            _notificationsApi = Substitute.For<INotificationsApi>();

            var logger = Substitute.For<ILogger<EmailService>>();

            var emailTemplate = new EmailTemplate
            {
                Id = 1,
                TemplateId = "TemplateId",
                TemplateName = "TemplateName"
            };

            _emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();
            _emailTemplateRepository.GetSingleOrDefault(Arg.Any<Expression<Func<EmailTemplate, bool>>>()).Returns(emailTemplate);

            var emailService = new EmailService(configuration, _notificationsApi, _emailTemplateRepository, logger);

            _subject = "A test email";
            _toAddress = "test@test.com";
            _replyToAddress = "reply@test.com";
            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };

            const string templateName = "TestTemplate";

            emailService.SendEmail(templateName, _toAddress, _subject, tokens, _replyToAddress).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_EmailTemplateRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _emailTemplateRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<EmailTemplate, bool>>>());
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Called_Exactly_Once()
        {
            _notificationsApi.Received(1).SendEmail(Arg.Any<SFA.DAS.Notifications.Api.Types.Email>());
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Called_With_Subject()
        {
            _notificationsApi.Received(1).SendEmail(Arg.Is<SFA.DAS.Notifications.Api.Types.Email>(e => e.Subject == _subject));
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Called_With_Send_To_Address()
        {
            _notificationsApi.Received(1).SendEmail(Arg.Is<SFA.DAS.Notifications.Api.Types.Email>(e => e.RecipientsAddress == _toAddress));
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Called_With_Reply_To_Address()
        {
            _notificationsApi.Received(1).SendEmail(Arg.Is<SFA.DAS.Notifications.Api.Types.Email>(e => e.ReplyToAddress == _replyToAddress));
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Called_With_SystemId_From_EmailService()
        {
            _notificationsApi.Received(1).SendEmail(Arg.Is<SFA.DAS.Notifications.Api.Types.Email>(e => e.SystemId == EmailService.SystemId));
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Called_With_First_Token_Key()
        {
            _notificationsApi.Received(1).SendEmail(Arg.Is<SFA.DAS.Notifications.Api.Types.Email>(e => e.Tokens.First().Key == "contactname"));
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Called_With_First_Token_Value()
        {
            _notificationsApi.Received(1).SendEmail(Arg.Is<SFA.DAS.Notifications.Api.Types.Email>(e => e.Tokens.First().Value == "name"));
        }
    }
}
