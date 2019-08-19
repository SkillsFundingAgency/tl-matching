using System;
using System.Collections.Generic;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmployerFeedback
{
    public class When_EmployerFeedbackService_Is_Called_To_Send_Employer_Feedback_Emails
        : IClassFixture<EmployerFeedbackFixture>
    {
        internal readonly IDateTimeProvider DateTimeProvider;
        private readonly EmployerFeedbackFixture _testFixture;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

        public When_EmployerFeedbackService_Is_Called_To_Send_Employer_Feedback_Emails(EmployerFeedbackFixture testFixture)
        {
            _testFixture = testFixture;

            _emailService = Substitute.For<IEmailService>();
            _emailHistoryService = Substitute.For<IEmailHistoryService>();

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityRepository.GetReferralsForEmployerFeedbackAsync(Arg.Any<DateTime>())
                .Returns(new List<EmployerReferralDto>
                {
                    new EmployerReferralDto
                    {
                        OpportunityId = 1,
                        EmployerContact = "Employer Contact",
                        EmployerContactEmail = "primary.contact@employer.co.uk"
                    }
                });
            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();

            var employerFeedbackService = new EmployerFeedbackService(
                _testFixture.Configuration, _testFixture.Logger,
                    _emailService, _emailHistoryService,
                    _opportunityRepository, _opportunityItemRepository,
                    _testFixture.DateTimeProvider);

            employerFeedbackService
                .SendEmployerFeedbackEmailsAsync("TestUser")
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityRepository_GetEmployerReferrals_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetReferralsForEmployerFeedbackAsync(
                    Arg.Any<DateTime>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Exactly_Once()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Expected_Parameters()
        {
            _emailService
                .Received(1)
                .SendEmail(Arg.Is<string>(
                        templateName => templateName == "EmployerFeedback"),
                    Arg.Is<string>(toAddress => toAddress == "primary.contact@employer.co.uk"),
                    Arg.Is<string>(subject => subject == "Your industry placement progress – ESFA"),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Is<string>(replyToAddress => replyToAddress == ""));
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Expected_Tokens()
        {
            var expectedResults = new Dictionary<string, string>
            {
                { "employer_contact_name",  "Employer Contact" },
            };

            _emailService
                .Received(1)
                .SendEmail(Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => _testFixture.DoTokensContainExpectedValues(tokens, expectedResults)),
                    Arg.Any<string>());
        }

        [Fact]
        public void Then_EmailHistoryService_SaveEmailHistory_Is_Called_Exactly_Once()
        {
            _emailHistoryService
                .Received(1)
                .SaveEmailHistory(Arg.Any<string>(), Arg.Any<IDictionary<string, string>>(), Arg.Any<int?>(), Arg.Any<string>(), Arg.Any<string>());
        }


        [Fact]
        public void Then_EmailHistoryService_SaveEmailHistory_Is_Called_With_Expected_Parameters()
        {
            _emailHistoryService
                .Received(1)
                .SaveEmailHistory(
                    Arg.Is<string>(templateName => templateName == "EmployerFeedback"),

                    Arg.Any<IDictionary<string, string>>(), 
                    Arg.Any<int?>(), 
                    Arg.Any<string>(), 
                    Arg.Is<string>(s => s == "TestUser"));
        }

    }
}
