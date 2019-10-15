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

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmailHistory
{
    public class When_EmailHistoryService_Is_Called_To_Save_Email_History
    {
        private readonly IRepository<Domain.Models.EmailHistory> _emailHistoryRepository;

        public When_EmailHistoryService_Is_Called_To_Save_Email_History()
        {
            var configuration = new MatchingConfiguration
            {
                SendEmailEnabled = true,
                GovNotifyApiKey = "TestApiKey"
            };

            var logger = Substitute.For<ILogger<EmailService>>();
            var notificationsApi = Substitute.For<IAsyncNotificationClient>();
            var emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmailHistoryMapper).Assembly));
            var mapper = new Mapper(config);

            _emailHistoryRepository = Substitute.For<IRepository<Domain.Models.EmailHistory>>();
            var emailTemplate = new EmailTemplate
            {
                Id = 1,
                TemplateId = "1599768C-7D3D-43AB-8548-82A4E5349468",
                TemplateName = "TemplateName"
            };

            emailTemplateRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<EmailTemplate, bool>>>()).Returns(emailTemplate);

            var emailService = new EmailService(configuration, notificationsApi, emailTemplateRepository, _emailHistoryRepository, mapper, logger);

            var toAddress = "test@test.com";
            var createdBy = "CreatedBy";
            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };

            emailService.SendEmailAsync(2, "Test Template", toAddress, tokens, createdBy).GetAwaiter().GetResult();

        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_Exactly_Once()
        {
            _emailHistoryRepository
                .Received(1)
                .CreateAsync(Arg.Any<Domain.Models.EmailHistory>());
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_With_OpportunityId()
        {
            _emailHistoryRepository
                .Received(1)
                .CreateAsync(Arg.Is<Domain.Models.EmailHistory>(email =>
                    email.OpportunityId == 2));
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_With_EmailTemplateId()
        {
            _emailHistoryRepository
                .Received(1)
                .CreateAsync(Arg.Is<Domain.Models.EmailHistory>(email =>
                    email.EmailTemplateId == 1));
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_With_SentTo()
        {
            _emailHistoryRepository
                .Received(1)
                .CreateAsync(Arg.Is<Domain.Models.EmailHistory>(email =>
                    email.SentTo == "test@test.com"));
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_With_CreatedBy()
        {
            _emailHistoryRepository
                .Received(1)
                .CreateAsync(Arg.Is<Domain.Models.EmailHistory>(email =>
                    email.CreatedBy == "CreatedBy"));
        }
    }
}
