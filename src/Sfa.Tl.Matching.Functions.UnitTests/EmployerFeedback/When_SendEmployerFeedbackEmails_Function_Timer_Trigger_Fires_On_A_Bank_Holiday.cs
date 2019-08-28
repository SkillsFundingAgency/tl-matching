using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.UnitTests.EmployerFeedback.Builders;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Tests.Common.Extensions;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.EmployerFeedback
{
    public class When_SendEmployerFeedbackEmails_Function_Timer_Trigger_Fires_On_A_Bank_Holiday
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmployerFeedbackService _employerFeedbackService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_SendEmployerFeedbackEmails_Function_Timer_Trigger_Fires_On_A_Bank_Holiday()
        {
            var timerSchedule = Substitute.For<TimerSchedule>();

            var configuration = new MatchingConfiguration
            {
                EmployerFeedbackTimeSpan = "-10.00:00:00"
            };

            _dateTimeProvider = Substitute.For<IDateTimeProvider>();
            _dateTimeProvider
                .UtcNow()
                .Returns(new DateTime(2019, 8, 26));
           _dateTimeProvider
                .AddWorkingDays(Arg.Any<DateTime>(), Arg.Any<TimeSpan>(), Arg.Any<IList<DateTime>>())
                .Returns(DateTime.Parse("2019-8-15 23:59:59"));
            _dateTimeProvider
                 .IsHoliday(Arg.Any<DateTime>(), Arg.Any<IList<DateTime>>())
                 .Returns(true);

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

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();
            
            _employerFeedbackService = Substitute.For<IEmployerFeedbackService>();

            var employerFeedback = new Functions.EmployerFeedback();
            employerFeedback.SendEmployerFeedbackEmails(
                new TimerInfo(timerSchedule, new ScheduleStatus()),
                new ExecutionContext(),
                new NullLogger<Functions.EmployerFeedback>(),
                configuration,
                _dateTimeProvider,
                _employerFeedbackService,
                bankHolidayRepository,
                _functionLogRepository).GetAwaiter().GetResult();
        }
        
        [Fact]
        public void Then_DateTimeProvider_IsHoliday_Is_Called_Exactly_Once()
        {
            _dateTimeProvider
                .Received(1)
                .IsHoliday(Arg.Any<DateTime>(), Arg.Any<IList<DateTime>>());
        }

        [Fact]
        public void Then_DateTimeProvider_AddWorkingDays_Is_Not_Called()
        {
            _dateTimeProvider
                .DidNotReceive()
                .AddWorkingDays(Arg.Any<DateTime>(), Arg.Any<TimeSpan>(), Arg.Any<IList<DateTime>>());
        }

        [Fact]
        public void SendFeedbackEmailsAsync_Is_Not_Called()
        {
            _employerFeedbackService
                .DidNotReceive()
                .SendEmployerFeedbackEmailsAsync(
                    Arg.Any<DateTime>(),
                    Arg.Any<string>());
        }

        [Fact]
        public void FunctionLogRepository_Create_Is_Not_Called()
        {
            _functionLogRepository
                .DidNotReceiveWithAnyArgs()
                .Create(Arg.Any<FunctionLog>());
        }
    }
}