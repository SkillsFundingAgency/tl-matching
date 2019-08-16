using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.Calendar;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.BankHolidayGenerator
{
    public class When_BankHolidayGenerator_Function_Timer_Trigger_Fires
    {
        private readonly ICalendarApiClient _calendarApiClient;
        private readonly IRepository<FunctionLog> _functionLogRepository;
        private readonly DateTime _minValue = new DateTime(0001, 1, 1, 0, 0, 0);
        private readonly IList<BankHoliday> _results;

        public When_BankHolidayGenerator_Function_Timer_Trigger_Fires()
        {
            var dto = new List<BankHolidayResultDto>
            {
                new BankHolidayResultDto
                {
                    Title = "August Summer Holiday",
                    Date = DateTime.Parse("2019-08-26")
                },
                new BankHolidayResultDto
                {
                    Title = "New Year",
                    Date = DateTime.Parse("2020-01-01")
                }
            };

            _calendarApiClient = Substitute.For<ICalendarApiClient>();
            _calendarApiClient.GetBankHolidaysAsync()
                .Returns(dto);

            var timerSchedule = Substitute.For<TimerSchedule>();
            var dateTimeProvider = Substitute.For<IDateTimeProvider>();
            dateTimeProvider.MinValue().Returns(_minValue);

            var config = new MapperConfiguration(c => c.AddMaps(typeof(BankHolidayMapper).Assembly));
            var mapper = new Mapper(config);
            var logger = Substitute.For<ILogger<GenericRepository<BankHoliday>>>();

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            using (var dbContext = InMemoryDbContext.Create())
            {
                var bankHolidayRepository = new GenericRepository<BankHoliday>(logger, dbContext);

                var bankHolidayGenerator = new Functions.BankHolidayGenerator();
                bankHolidayGenerator.GenerateBankHoliday(
                    new TimerInfo(timerSchedule, new ScheduleStatus()),
                    new ExecutionContext(),
                    new NullLogger<Functions.BankHolidayGenerator>(),
                    _calendarApiClient,
                    mapper,
                    bankHolidayRepository,
                    _functionLogRepository).GetAwaiter().GetResult();

                _results = dbContext.BankHoliday.ToList();
            }
        }

        [Fact]
        public void GetBankHolidays_Is_Called_Exactly_Once()
        {
            _calendarApiClient
                .Received(1)
                .GetBankHolidaysAsync();
        }

        [Fact]
        public void FunctionLogRepository_Create_Is_Not_Called()
        {
            _functionLogRepository
                .DidNotReceiveWithAnyArgs()
                .Create(Arg.Any<FunctionLog>());
        }

        [Fact]
        public void BankHolidayRepository_Results_Should_Have_Expected_Values()
        {
            _results.Count.Should().Be(2);

            _results[0].Date.Should().BeSameDateAs(new DateTime(2019, 8, 26));
            _results[0].Title.Should().Be("August Summer Holiday");
            _results[0].CreatedBy.Should().Be("System");

            _results[1].Date.Should().BeSameDateAs(new DateTime(2020, 1, 1));
            _results[1].Title.Should().Be("New Year");
            _results[1].CreatedBy.Should().Be("System");
        }
    }
}