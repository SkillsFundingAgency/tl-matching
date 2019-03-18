using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoMapper;
using NSubstitute;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Application.UnitTests.Services.Referral.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Referral
{
    public class When_ReferralService_Is_Called_To_Send_Provider_Email
    {
        private readonly IEmailService _emailService;
        private readonly IRepository<EmailHistory> _emailHistoryRepository;
        private readonly IRepository<EmailTemplate> _emailTemplateRepository;
        private readonly IOpportunityRepository _opportunityRepository;

        public When_ReferralService_Is_Called_To_Send_Provider_Email()
        {
            _emailService = Substitute.For<IEmailService>();

            var logger = Substitute.For<ILogger<ReferralService>>();

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmailHistoryMapper).Assembly));
            var mapper = new Mapper(config);

            _emailHistoryRepository = Substitute.For<IRepository<EmailHistory>>();
            _emailTemplateRepository = Substitute.For<IRepository<EmailTemplate>>();
            _opportunityRepository = Substitute.For<IOpportunityRepository>();

            _emailTemplateRepository
                .GetSingleOrDefault(Arg.Any<Expression<Func<EmailTemplate, bool>>>())
                .Returns(new ValidEmailTemplateBuilder().Build());

            _opportunityRepository
                .GetProviderOpportunities(
                    Arg.Any<int>())
                .Returns(new ValidOpportunityReferralDtoListBuilder().Build());

            var referralService = new ReferralService(_emailService,
                _emailHistoryRepository, _emailTemplateRepository, _opportunityRepository,
                mapper, logger);

            referralService.SendProviderReferralEmail(1).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Exactly_Once()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
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
                .Create(Arg.Any<EmailHistory>());
        }

        [Fact]
        public void Then_OpportunityRepository_GetSingleOrDefault_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetProviderOpportunities(Arg.Any<int>());
        }
    }
}
