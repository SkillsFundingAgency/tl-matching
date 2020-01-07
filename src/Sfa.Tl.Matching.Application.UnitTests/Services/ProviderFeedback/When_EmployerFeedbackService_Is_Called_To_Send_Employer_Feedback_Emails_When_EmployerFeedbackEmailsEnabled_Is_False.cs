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
    public class When_ProviderFeedbackService_Is_Called_To_Send_Provider_Feedback_Emails_When_ProviderFeedbackEmailsEnabled_Is_False
        : IClassFixture<ProviderFeedbackFixture>
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IRepository<BankHoliday> _bankHolidayRepository;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly int _result;

        public When_ProviderFeedbackService_Is_Called_To_Send_Provider_Feedback_Emails_When_ProviderFeedbackEmailsEnabled_Is_False(
            ProviderFeedbackFixture testFixture)
        {
            _dateTimeProvider = Substitute.For<IDateTimeProvider>();

            _emailService = Substitute.For<IEmailService>();

            _bankHolidayRepository = Substitute.For< IRepository<BankHoliday>>();
            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityRepository.GetReferralsForProviderFeedbackAsync(Arg.Any<DateTime>())
                .Returns(new ValidProviderFeedbackDtoListBuilder().Build());
            
            testFixture.Configuration.ProviderFeedbackEmailsEnabled = false;

            var providerFeedbackService = new ProviderFeedbackService(
                testFixture.Configuration,
                testFixture.Logger,
                _emailService,
                _bankHolidayRepository,
                _opportunityRepository,
                _dateTimeProvider);

            _result = providerFeedbackService
                .SendProviderFeedbackEmailsAsync("TestUser")
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void BankHolidayRepository_GetManyAsync_Is_Not_Called()
        {
            _bankHolidayRepository
                .DidNotReceive()
                .GetManyAsync(Arg.Any<Expression<Func<BankHoliday, bool>>>());
        }

        [Fact]
        public void Then_OpportunityRepository_GetReferralsForProviderFeedbackAsync_Is_Not_Called()
        {
            _opportunityRepository
                .DidNotReceive()
                .GetReferralsForProviderFeedbackAsync(Arg.Any<DateTime>());
        }

        [Fact]
        public void Then_DateTimeProvider_UtcNow_Is_Not_Called()
        {
            _dateTimeProvider
                .DidNotReceive()
                .UtcNow();
        }

        [Fact]
        public void Then_DateTimeProvider_IsHoliday_Is_Not_Called()
        {
            _dateTimeProvider
                .DidNotReceive()
                .IsHoliday(Arg.Any<DateTime>(), Arg.Any<IList<DateTime>>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Not_Called()
        {
            _emailService
                 .DidNotReceive()
                 .SendEmailAsync(Arg.Any<int>(), Arg.Any<int>(),
                     Arg.Any<string>(),
                     Arg.Any<string>(),
                     Arg.Any<IDictionary<string, string>>(),
                     Arg.Any<string>());
        }

        [Fact]
        public void Then_Result_Has_Expected_Value()
        {
            _result.Should().Be(0);
        }
    }
}