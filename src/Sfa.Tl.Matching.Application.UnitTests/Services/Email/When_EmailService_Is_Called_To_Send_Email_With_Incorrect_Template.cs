using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Notify.Interfaces;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Email
{
    public class When_EmailService_Is_Called_To_Send_Email_With_Incorrect_Template
    {
        private readonly ILogger<EmailService> _logger;
        private readonly IAsyncNotificationClient _notificationsApi;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        public When_EmailService_Is_Called_To_Send_Email_With_Incorrect_Template()
        {
            var emailHistoryRepository = Substitute.For<IRepository<EmailHistory>>();
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmailHistoryMapper).Assembly));
            var mapper = new Mapper(config);
            var configuration = new MatchingConfiguration
            {
                SendEmailEnabled = true
            };

            _notificationsApi = Substitute.For<IAsyncNotificationClient>();

            _logger = Substitute.For<ILogger<EmailService>>();

            _emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();
            _emailTemplateRepository
                .GetSingleOrDefaultAsync(Arg.Any<Expression<Func<EmailTemplate, bool>>>())
                .ReturnsNull();

            var functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var emailService = new EmailService(configuration, _notificationsApi, _emailTemplateRepository, emailHistoryRepository, functionLogRepository, mapper, _logger);
            const string toAddress = "test@test.com";
            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };

            const string templateName = "MissingTestTemplate";

            emailService.SendEmailAsync(templateName, toAddress, null, null, tokens, "System").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmailTemplateRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _emailTemplateRepository.Received(1).GetSingleOrDefaultAsync(Arg.Any<Expression<Func<EmailTemplate, bool>>>());
        }

        [Fact]
        public void Then_EmailTemplateRepository_Logger_Is_Called_Exactly_Once_With_Expected_Message()
        {
            _logger.ShouldHaveExactMessage(LogLevel.Warning, "Email template MissingTestTemplate not found. No emails sent.");
        }

        [Fact]
        public void Then_NotificationsApi_SendEmail_Is_Not_Called()
        {
            _notificationsApi.DidNotReceive().SendEmailAsync(Arg.Any<string>(), Arg.Any<string>(),
                Arg.Any<Dictionary<string, dynamic>>());
        }
    }
}