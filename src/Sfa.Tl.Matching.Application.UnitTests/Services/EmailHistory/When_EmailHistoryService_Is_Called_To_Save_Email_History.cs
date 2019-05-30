using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmailHistory
{
    public class When_EmailHistoryService_Is_Called_To_Save_Email_History
    {
        private readonly IRepository<Domain.Models.EmailHistory> _emailHistoryRepository;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;

        public When_EmailHistoryService_Is_Called_To_Save_Email_History()
        {
            var logger = Substitute.For<ILogger<EmailHistoryService>>();

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmailHistoryMapper).Assembly));
            var mapper = new Mapper(config);

            var emailTemplate = new EmailTemplate
            {
                Id = 1,
                TemplateId = "1599768C-7D3D-43AB-8548-82A4E5349468",
                TemplateName = "TemplateName"
            };

            _emailHistoryRepository = Substitute.For<IRepository<Domain.Models.EmailHistory>>();
            _emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();

            _emailTemplateRepository.GetSingleOrDefault(Arg.Any<Expression<Func<EmailTemplate, bool>>>()).Returns(emailTemplate);

            var emailHistoryService = new EmailHistoryService(_emailHistoryRepository, _emailTemplateRepository, mapper, logger);

            var toAddress = "test@test.com";
            var createdBy = "CreatedBy";
            var tokens = new Dictionary<string, string>
            {
                { "contactname",  "name" }
            };
            
            emailHistoryService
                .SaveEmailHistory("TemplateName", tokens, 2, toAddress, createdBy)
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmailTemplateRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _emailTemplateRepository.Received(1).GetSingleOrDefault(Arg.Any<Expression<Func<EmailTemplate, bool>>>());
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_Exactly_Once()
        {
            _emailHistoryRepository
                .Received(1)
                .Create(Arg.Any<Domain.Models.EmailHistory>());
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_With_OpportunityId()
        {
            _emailHistoryRepository
                .Received(1)
                .Create(Arg.Is<Domain.Models.EmailHistory>(email =>
                    email.OpportunityId == 2));
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_With_EmailTemplateId()
        {
            _emailHistoryRepository
                .Received(1)
                .Create(Arg.Is<Domain.Models.EmailHistory>(email =>
                    email.EmailTemplateId == 1));
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_With_SentTo()
        {
            _emailHistoryRepository
                .Received(1)
                .Create(Arg.Is<Domain.Models.EmailHistory>(email =>
                    email.SentTo == "test@test.com"));
        }

        [Fact]
        public void Then_EmailHistoryRepository_Create_Is_Called_With_CreatedBy()
        {
            _emailHistoryRepository
                .Received(1)
                .Create(Arg.Is<Domain.Models.EmailHistory>(email =>
                    email.CreatedBy == "CreatedBy"));
        }
    }
}
