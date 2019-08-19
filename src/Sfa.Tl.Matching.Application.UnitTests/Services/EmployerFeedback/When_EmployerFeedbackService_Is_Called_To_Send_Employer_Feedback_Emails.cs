using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Extensions;
using Sfa.Tl.Matching.Application.UnitTests.Services.EmployerFeedback.Builders;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.EmployerFeedback
{
    public class When_EmployerFeedbackService_Is_Called_To_Send_Employer_Feedback_Emails
        : IClassFixture<EmployerFeedbackFixture>
    {
        private readonly EmployerFeedbackFixture _testFixture;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;

        public When_EmployerFeedbackService_Is_Called_To_Send_Employer_Feedback_Emails(EmployerFeedbackFixture testFixture)
        {
            _testFixture = testFixture;

            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _dateTimeProvider
                .UtcNow()
                .Returns(new DateTime(2019, 9, 1));
            _dateTimeProvider
                .AddWorkingDays(Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<IList<DateTime>>())
                .Returns(new DateTime(2019, 8, 16));

            _emailService = Substitute.For<IEmailService>();
            _emailHistoryService = Substitute.For<IEmailHistoryService>();

            var bankHolidays = new BankHolidayListBuilder().Build().AsQueryable();

            var mockSet = Substitute.For<DbSet<BankHoliday>, IAsyncEnumerable<BankHoliday>, IQueryable<BankHoliday>>();

            // ReSharper disable once SuspiciousTypeConversion.Global
            ((IAsyncEnumerable<BankHoliday>)mockSet).GetEnumerator().Returns(new FakeAsyncEnumerator<BankHoliday>(bankHolidays.GetEnumerator()));
            ((IQueryable<BankHoliday>)mockSet).Provider.Returns(new FakeAsyncQueryProvider<BankHoliday>(bankHolidays.Provider));
            ((IQueryable<BankHoliday>)mockSet).Expression.Returns(bankHolidays.Expression);
            ((IQueryable<BankHoliday>)mockSet).ElementType.Returns(bankHolidays.ElementType);
            ((IQueryable<BankHoliday>)mockSet).GetEnumerator().Returns(bankHolidays.GetEnumerator());

            var contextOptions = new DbContextOptions<MatchingDbContext>();
            var mockContext = Substitute.For<MatchingDbContext>(contextOptions);
            mockContext.Set<BankHoliday>().Returns(mockSet);

            IRepository<BankHoliday> bankHolidayRepository = new GenericRepository<BankHoliday>(NullLogger<GenericRepository<BankHoliday>>.Instance, mockContext);

            _opportunityRepository = Substitute.For<IOpportunityRepository>();
            _opportunityRepository.GetReferralsForEmployerFeedbackAsync(Arg.Any<DateTime>())
                .Returns(new List<EmployerFeedbackDto>
                {
                    new EmployerFeedbackDto
                    {
                        OpportunityId = 1,
                        OpportunityItemId = 2,
                        EmployerContact = "Employer Contact",
                        EmployerContactEmail = "primary.contact@employer.co.uk"
                    }
                });
            _opportunityItemRepository = Substitute.For<IRepository<OpportunityItem>>();

            var employerFeedbackService = new EmployerFeedbackService(
                _testFixture.Configuration, _testFixture.Logger,
                _emailService, _emailHistoryService,
                _opportunityRepository, _opportunityItemRepository,
                bankHolidayRepository,
                _dateTimeProvider);

            employerFeedbackService
                .SendEmployerFeedbackEmailsAsync("TestUser")
                .GetAwaiter().GetResult();
        }

        //[Fact]
        //public void Then_BankHolidayRepository_GetMany_Is_Called_Exactly_Once()
        //{
        //    _bankHolidayRepository
        //        .Received(1)
        //        .GetMany(Arg.Any<Expression<Func<BankHoliday, bool>>>());
        //}

        [Fact]
        public void Then_OpportunityRepository_GetReferralsForEmployerFeedbackAsync_Is_Called_Exactly_Once()
        {
            _opportunityRepository
                .Received(1)
                .GetReferralsForEmployerFeedbackAsync(Arg.Any<DateTime>());
        }

        [Fact]
        public void Then_DateTimeProvider_AddWorkingDays_Is_Called_Exactly_Once()
        {
            _dateTimeProvider
                .Received(1)
                .AddWorkingDays(Arg.Any<DateTime>(), Arg.Any<int>(), Arg.Any<IList<DateTime>>());
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
