using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Email
{
    public class When_EmailService_Is_Called_To_Send_Email_But_Send_Email_Is_Disabled
    {
        private readonly IAsyncNotificationClient _notificationsApi;

        public When_EmailService_Is_Called_To_Send_Email_But_Send_Email_Is_Disabled()
        {
            var configuration = new MatchingConfiguration
            {
                SendEmailEnabled = false
            };

            _notificationsApi = Substitute.For<IAsyncNotificationClient>();

            var logger = Substitute.For<ILogger<EmailService>>();
            var emailHistoryRepository = Substitute.For<IRepository<Domain.Models.EmailHistory>>();
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmailHistoryMapper).Assembly));
            var mapper = new Mapper(config);

            var emailTemplate = new EmailTemplate
            {
                Id = 1,
                TemplateId = "TemplateId",
                TemplateName = "TemplateName"
            };

            var emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();
            emailTemplateRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<EmailTemplate, bool>>>()).Returns(emailTemplate);

            var emailService = new EmailService(configuration, _notificationsApi, emailTemplateRepository, emailHistoryRepository, mapper, logger);

            const string toAddress = "test@test.com";
            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };

            const string templateName = "TestTemplate";

            emailService.SendEmailAsync(null, templateName, toAddress, tokens, "System").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Not_Called()
        {
            _notificationsApi.DidNotReceive().SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<Dictionary<string, dynamic>>());
        }
    }
}
