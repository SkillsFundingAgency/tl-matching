using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.EmployerFeedback.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmployerFeedback
{
    public class When_EmployerFeedbackService_Is_Called_To_Send_Employer_Feedback_Emails_With_Multiple_Employers
        : IClassFixture<EmployerFeedbackFixture>
    {
        private readonly EmployerFeedbackFixture _testFixture;
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly int _result;

        public When_EmployerFeedbackService_Is_Called_To_Send_Employer_Feedback_Emails_With_Multiple_Employers(
            EmployerFeedbackFixture testFixture)
        {
            _testFixture = testFixture;

            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider
                .UtcNow()
                .Returns(new DateTime(2019, 12, 13));
            dateTimeProvider
                .GetNthWorkingDayDate(Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<IList<DateTime>>())
                .Returns(new DateTime(2019, 12, 13));

            _emailService = Substitute.For<IEmailService>();

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityRepository.GetReferralsForEmployerFeedbackAsync(Arg.Any<DateTime>())
                .Returns(new EmployerFeedbackDtoListBuilder().AddAnotherEmployer()
                    .Build());

            var bankHolidays = new BankHolidayListBuilder().Build().AsQueryable();

            var mockSet = Substitute.For<DbSet<BankHoliday>, IAsyncEnumerable<BankHoliday>, IQueryable<BankHoliday>>();

            // ReSharper disable once SuspiciousTypeConversion.Global
            ((IAsyncEnumerable<BankHoliday>)mockSet).GetEnumerator()
                .Returns(new FakeAsyncEnumerator<BankHoliday>(bankHolidays.GetEnumerator()));
            ((IQueryable<BankHoliday>)mockSet).Provider.Returns(
                new FakeAsyncQueryProvider<BankHoliday>(bankHolidays.Provider));
            ((IQueryable<BankHoliday>)mockSet).Expression.Returns(bankHolidays.Expression);
            ((IQueryable<BankHoliday>)mockSet).ElementType.Returns(bankHolidays.ElementType);
            ((IQueryable<BankHoliday>)mockSet).GetEnumerator().Returns(bankHolidays.GetEnumerator());

            var contextOptions = new DbContextOptions<MatchingDbContext>();
            var mockContext = Substitute.For<MatchingDbContext>(contextOptions);
            mockContext.Set<BankHoliday>().Returns(mockSet);

            IRepository<BankHoliday> bankHolidayRepository =
                new GenericRepository<BankHoliday>(NullLogger<GenericRepository<BankHoliday>>.Instance, mockContext);

            var employerFeedbackService = new EmployerFeedbackService(
                _testFixture.Configuration,
                _testFixture.Logger,
                _emailService,
                bankHolidayRepository,
                _opportunityRepository,
                dateTimeProvider);

            _result = employerFeedbackService
                .SendEmployerFeedbackEmailsAsync("TestUser")
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_OpportunityRepository_GetReferralsForEmployerFeedbackAsync_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetReferralsForEmployerFeedbackAsync(Arg.Any<DateTime>());
        }

        [Fact]
        public void Then_EmailService_SendEmail_Is_Called_With_Expected_Tokens()
        {
            const string expectedOpportunityListForEmail1 = "* 1 x Route student at Town CV1 2WT on 12 December 2019\r\n"
                                                        + "* 3 x Job Role at Another Town CV2 3WT on 01 December 2019";

            var expectedTokensForEmail1 = new Dictionary<string, string>
            {
                { "employer_contact_name",  "Employer Contact" },
                { "previous_month", "November" },
                { "opportunity_list", expectedOpportunityListForEmail1 },
                { "opportunity_item_id_1", "1" },
                { "opportunity_item_id_2", "2" }
            };

            _emailService
                .Received(1)
                .SendEmailAsync(null, null,
                    Arg.Is<string>(templateName => templateName == EmailTemplateName.EmployerFeedbackV2.ToString()),
                    Arg.Is<string>(toAddress => toAddress == "employer.contact@employer.co.uk"),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => _testFixture.DoTokensContainExpectedValues(tokens, expectedTokensForEmail1)),
                    Arg.Is<string>(createdBy => createdBy == "TestUser"));

            const string expectedOpportunityListForEmail2 = "* 3 x Another Job Role at Another Town CV1 4WT on 01 December 2019";

            var expectedTokensForEmail2 = new Dictionary<string, string>
            {
                { "employer_contact_name",  "Another Employer Contact" },
                { "previous_month", "November" },
                { "opportunity_list", expectedOpportunityListForEmail2 },
                { "opportunity_item_id_1", "3" }
            };

            _emailService
                .Received(1)
                .SendEmailAsync(null, null,
                    Arg.Is<string>(templateName => templateName == EmailTemplateName.EmployerFeedbackV2.ToString()),
                    Arg.Is<string>(toAddress => toAddress == "another.employer.contact@employer.co.uk"),
                    Arg.Is<IDictionary<string, string>>(
                        tokens => _testFixture.DoTokensContainExpectedValues(tokens, expectedTokensForEmail2)),
                    Arg.Is<string>(createdBy => createdBy == "TestUser"));
        }

        [Fact]
        public void Then_Result_Has_Expected_Value()
        {
            _result.Should().Be(2);
        }
    }
}