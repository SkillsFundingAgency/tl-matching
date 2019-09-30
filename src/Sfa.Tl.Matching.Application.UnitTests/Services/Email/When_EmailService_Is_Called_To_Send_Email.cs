using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Email
{
    public class When_EmailService_Is_Called_To_Send_Email
    {
        private readonly IAsyncNotificationClient _notificationsApi;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        private readonly string _toAddress;
        
        public When_EmailService_Is_Called_To_Send_Email()
        {
            var configuration = new MatchingConfiguration
            {
                SendEmailEnabled = true,
                GovNotifyApiKey = "TestApiKey"
            };

            _notificationsApi = Substitute.For<IAsyncNotificationClient>();

            var logger = Substitute.For<ILogger<EmailService>>();

            var emailTemplate = new EmailTemplate
            {
                Id = 1,
                TemplateId = "1599768C-7D3D-43AB-8548-82A4E5349468",
                TemplateName = "TemplateName"
            };

            _emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();
            _emailTemplateRepository.GetSingleOrDefault(Arg.Any<Expression<Func<EmailTemplate, bool>>>()).Returns(emailTemplate);

            var emailService = new EmailService(configuration, _notificationsApi, _emailTemplateRepository, logger);

            _toAddress = "test@test.com";
            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };

            const string templateName = "TestTemplate";

            emailService.SendEmail(templateName, _toAddress, tokens).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmailTemplateRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _emailTemplateRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<EmailTemplate, bool>>>());
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Called_Exactly_Once()
        {
            _notificationsApi.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<Dictionary<string, dynamic>>());
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Called_With_Send_To_Address()
        {
            _notificationsApi.Received(1).SendEmailAsync(Arg.Is<string>(e => e == _toAddress), Arg.Any<string>(), Arg.Any<Dictionary<string, dynamic>>());
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Called_With_Template_Id()
        {
            _notificationsApi.Received(1).SendEmailAsync(Arg.Any<string>(),
                Arg.Is<string>(templateId => templateId == "1599768C-7D3D-43AB-8548-82A4E5349468"),
                Arg.Any<Dictionary<string, dynamic>>());
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Called_With_First_Token_Key()
        {
            _notificationsApi.Received(1).SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Is<Dictionary<string, dynamic>>(dict => dict.First().Key == "contactname"));
        }
    }
}
