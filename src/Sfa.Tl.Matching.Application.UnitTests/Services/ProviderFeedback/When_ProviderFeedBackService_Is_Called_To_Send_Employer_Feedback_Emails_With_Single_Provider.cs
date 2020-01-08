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
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.ProviderFeedback
{
    public class When_ProviderFeedBackService_Is_Called_To_Send_Employer_Feedback_Emails_With_Single_Provider
        : IClassFixture<ProviderFeedbackFixture>
    {
        private readonly ProviderFeedbackFixture _testFixture;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly int _result;
        private readonly IDictionary<string, string> _otherEmails = new Dictionary<string, string>();

        public When_ProviderFeedBackService_Is_Called_To_Send_Employer_Feedback_Emails_With_Single_Provider(
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

            _emailService
                .When(x => x.SendEmailAsync(Arg.Any<int?>(),
                    Arg.Any<string>(),
                    Arg.Any<string>(),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Any<string>()))
                .Do(x =>
                {
                    var address = x.ArgAt<string>(3);
                    var tokens = x.Arg<Dictionary<string, string>>();
                    if (tokens.TryGetValue("other_email_details", out var contact))
                    {
                        _otherEmails[address] = contact;
                    }
                });

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            
            _opportunityRepository
                .GetReferralsForProviderFeedbackAsync(Arg.Any<DateTime>())
                .Returns(new ValidProviderFeedbackDtoListBuilder()
                    .AddSecondaryContact()
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
            _emailService
                .Received(2)
                .SendEmailAsync(Arg.Any<int?>(),
                    Arg.Is<string>(
                    templateName => templateName == "ProviderFeedbackV2"),
                    Arg.Any<string>(),
                    Arg.Any<IDictionary<string, string>>(),
                    Arg.Is<string>(createdBy => createdBy == "TestUser"));
        }
        
        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Expected_Other_Emails_Tokens_For_Primary_Contact()
        {
            var expected = "We also sent this email to " +
                           "Provider Secondary Contact " +
                           "who we have as Provider display name’s " +
                           "secondary contact for industry placements. " +
                           "Please coordinate your response with them.\r\n";
            _otherEmails.Should().ContainKey("primary.contact@provider.co.uk");
            _otherEmails["primary.contact@provider.co.uk"].Should().Be(expected);
        }
        
        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Expected_Other_Emails_Tokens_For_Secondary_Contact()
        {
            var expected = "We also sent this email to " +
                           "Provider Contact " +
                           "who we have as Provider display name’s " +
                           "primary contact for industry placements. " +
                           "Please coordinate your response with them.\r\n";

            _otherEmails.Should().ContainKey("secondary.contact@provider.co.uk");
            _otherEmails["secondary.contact@provider.co.uk"].Should().Be(expected);
        }

        [Fact]
        public void Then_Result_Has_Expected_Value()
        {
            _result.Should().Be(2);
        }
    }
}

