using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback
{
    public class When_ProviderFeedBackService_Is_Called_To_Send_Employer_Feedback_Emails_To_Primary_Contact_Only
        : IClassFixture<ProviderFeedbackFixture>
    {
        private readonly ProviderFeedbackFixture _testFixture;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly int _result;

        public When_ProviderFeedBackService_Is_Called_To_Send_Employer_Feedback_Emails_To_Primary_Contact_Only(
            ProviderFeedbackFixture testFixture)
        {
            _testFixture = testFixture;

            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _dateTimeProvider
                .UtcNow()
                .Returns(new DateTime(2019, 12, 13));
            _dateTimeProvider
                .GetNthWorkingDayDate(Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<IList<DateTime>>())
                .Returns(new DateTime(2019, 12, 13));

            _emailService = Substitute.For<IEmailService>();
            
            _opportunityRepository = Substitute.For<IOpportunityRepository>();

            _opportunityRepository
                .GetReferralsForProviderFeedbackAsync(Arg.Any<DateTime>())
                .Returns(new ValidProviderFeedbackDtoListBuilder()
                    .Build());

            var providerFeedbackService = new ProviderFeedbackService(
                _testFixture.Configuration,
                _testFixture.Logger,
                _emailService,
                _testFixture.BankHolidayRepository,
                _opportunityRepository,
                _dateTimeProvider);

            _result = providerFeedbackService.SendProviderFeedbackEmailsAsync("TestUser").GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_DateTimeProvider_GetNthWorkingDayDate_Is_Called_Exactly_Once()
        {
            _dateTimeProvider
                .Received(1)
                .GetNthWorkingDayDate(
                    new DateTime(2019, 12, 13),
                    _testFixture.Configuration.ProviderFeedbackWorkingDayInMonth,
                Arg.Any<IList<DateTime>>());
        }

        [Fact]
        public void Then_BankHolidayRepository_GetMany_Is_Called_Exactly_Once()
        {
            _testFixture.BankHolidayRepository
                .Received(1)
                .GetManyAsync(Arg.Any<Expression<Func<BankHoliday, bool>>>());
        }

        [Fact]
        public void Then_OpportunityRepository_GetMany_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetReferralsForProviderFeedbackAsync(
                    Arg.Is<DateTime>(
                        d =>
                            d == new DateTime(2019, 11, 13)));
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_Exactly_Twice_With_ProviderFeedback_Template()
        {
            const string expectedEmployersList =
                "* Company\r\n"
                + "for students studying test route 1 courses at Town AA1 1AA\r\n"
                + "and for students studying test route 2 courses at Town AA1 1AA\r\n"
                + "\r\n";

            var expectedTokens = new Dictionary<string, string>
            {
                {"contact_name", "Provider Contact"},
                {"previous_month", "November"},
                {"provider_name", "Provider display name"},
                {"employers_list", expectedEmployersList},
                {"other_email_details", ""},
            };

            _emailService
                .Received(1)
                .SendEmailAsync(null,
                    Arg.Is<string>(templateName => templateName == EmailTemplateName.ProviderFeedbackV2.ToString()),
                    Arg.Is<string>(toAddress => toAddress == "primary.contact@provider.co.uk"),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => _testFixture.DoTokensContainExpectedValues(tokens, expectedTokens)),
                    Arg.Is<string>(createdBy => createdBy == "TestUser"));
        }

        [Fact]
        public void Then_Result_Has_Expected_Value()
        {
            _result.Should().Be(1);
        }
    }
}

