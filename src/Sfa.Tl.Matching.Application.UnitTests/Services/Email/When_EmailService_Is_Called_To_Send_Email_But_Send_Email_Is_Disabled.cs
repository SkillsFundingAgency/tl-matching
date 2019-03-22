using System;
using System.Collections.Generic;
using NSubstitute;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using SFA.DAS.Notifications.Api.Client;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Email
{
    public class When_EmailService_Is_Called_To_Send_Email_But_Send_Email_Is_Disabled
    {
        private readonly INotificationsApi _notificationsApi;

        public When_EmailService_Is_Called_To_Send_Email_But_Send_Email_Is_Disabled()
        {
            var configuration = new MatchingConfiguration
            {
                SendEmailEnabled = false
            };

            _notificationsApi = Substitute.For<INotificationsApi>();

            var logger = Substitute.For<ILogger<EmailService>>();

            var emailTemplate = new EmailTemplate
            {
                Id = 1,
                TemplateId = "TemplateId",
                TemplateName = "TemplateName"
            };

            var emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();
            emailTemplateRepository.GetSingleOrDefault(Arg.Any<Expression<Func<EmailTemplate, bool>>>()).Returns(emailTemplate);

            var emailService = new EmailService(configuration, _notificationsApi, emailTemplateRepository, logger);

            var subject = "A test email";
            var toAddress = "test@test.com";
            var replyToAddress = "reply@test.com";
            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };

            var templateName = "TestTemplate";

            emailService.SendEmail(templateName, toAddress, subject, tokens, replyToAddress).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Not_Called()
        {
            _notificationsApi.DidNotReceive().SendEmail(Arg.Any<SFA.DAS.Notifications.Api.Types.Email>());
        }
    }
}
