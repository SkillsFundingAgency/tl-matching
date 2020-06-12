using System;
using System.Collections.Generic;
using System.Linq;
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
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmailHistoryMapper).Assembly));
            var mapper = new Mapper(config);

            var emailTemplate = new EmailTemplate
            {
                Id = 1,
                TemplateId = "1599768C-7D3D-43AB-8548-82A4E5349468",
                TemplateName = "TemplateName"
            };

            _emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();
            var emailHistoryRepository = Substitute.For<IRepository<EmailHistory>>();

            _emailTemplateRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<EmailTemplate, bool>>>()).Returns(emailTemplate);

            var functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var emailService = new EmailService(configuration, _notificationsApi, _emailTemplateRepository, emailHistoryRepository, functionLogRepository, mapper, logger);

            _toAddress = "test@test.com";
            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };

            const string templateName = "TestTemplate";

            emailService.SendEmailAsync(templateName, _toAddress, null, null, tokens, "System").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmailTemplateRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _emailTemplateRepository.Received(1).GetSingleOrDefaultAsync(Arg.Any<Expression<Func<EmailTemplate, bool>>>());
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Called_Exactly_Once_With_Expected_Values()
        {
            _notificationsApi
                .Received(1)
                .SendEmailAsync(Arg.Is<string>(emailAddress =>
                        emailAddress == _toAddress), 
                    Arg.Is<string>(templateId => 
                        templateId == "1599768C-7D3D-43AB-8548-82A4E5349468"),
                    Arg.Is<Dictionary<string, dynamic>>(dict => 
                        dict.First().Key == "contactname"));
        }
    }
}