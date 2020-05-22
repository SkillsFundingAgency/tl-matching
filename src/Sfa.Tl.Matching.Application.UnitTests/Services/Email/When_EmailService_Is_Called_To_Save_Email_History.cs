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
    public class When_EmailService_Is_Called_To_Save_Email_History
    {
        private readonly IRepository<EmailHistory> _emailHistoryRepository;

        public When_EmailService_Is_Called_To_Save_Email_History()
        {
            var configuration = new MatchingConfiguration
            {
                SendEmailEnabled = true,
                GovNotifyApiKey = "TestApiKey"
            };

            var logger = Substitute.For<ILogger<EmailService>>();
            var notificationsApi = Substitute.For<IAsyncNotificationClient>();
            var emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();
            var functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmailHistoryMapper).Assembly));
            var mapper = new Mapper(config);

            _emailHistoryRepository = Substitute.For<IRepository<EmailHistory>>();
            var emailTemplate = new EmailTemplate
            {
                Id = 1,
                TemplateId = "1599768C-7D3D-43AB-8548-82A4E5349468",
                TemplateName = "TemplateName"
            };

            emailTemplateRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<EmailTemplate, bool>>>()).Returns(emailTemplate);

            var emailService = new EmailService(configuration, notificationsApi, emailTemplateRepository, _emailHistoryRepository, functionLogRepository, mapper, logger);

            var toAddress = "test@test.com";
            var createdBy = "CreatedBy";
            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };

            emailService.SendEmailAsync(2, "Test Template", toAddress, tokens, createdBy).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_Exactly_Once_With_Expected_Values()
        {
            _emailHistoryRepository
                .Received(1)
                .CreateAsync(Arg.Is<EmailHistory>(email =>
                    email.OpportunityId == 2 &&
                    email.EmailTemplateId == 1 &&
                    email.SentTo == "test@test.com" &&
                    email.CreatedBy == "CreatedBy"));
        }
    }
}
