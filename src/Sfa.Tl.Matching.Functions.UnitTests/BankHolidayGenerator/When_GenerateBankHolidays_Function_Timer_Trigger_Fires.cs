﻿using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Timers;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Api.Clients.BankHolidays;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Functions.UnitTests.BankHolidayGenerator
{
    public class When_GenerateBankHolidays_Function_Timer_Trigger_Fires
    {
        private readonly IBankHolidaysApiClient _bankHolidaysApiClient;
        private readonly IBulkInsertRepository<BankHoliday> _bankHolidayBulkInsertRepository;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_GenerateBankHolidays_Function_Timer_Trigger_Fires()
        {
            var dto = new List<BankHolidayResultDto>
            {
                new()
                {
                    Title = "August Summer Holiday",
                    Date = DateTime.Parse("2019-08-26")
                },
                new()
                {
                    Title = "New Year",
                    Date = DateTime.Parse("2020-01-01")
                }
            };

            _bankHolidaysApiClient = Substitute.For<IBankHolidaysApiClient>();
            _bankHolidaysApiClient.GetBankHolidaysAsync()
                .Returns(dto);

            var timerSchedule = Substitute.For<TimerSchedule>();

            var config = new MapperConfiguration(c => c.AddMaps(typeof(BankHolidayMapper).Assembly));
            var mapper = new Mapper(config);

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            _bankHolidayBulkInsertRepository = Substitute.For<IBulkInsertRepository<BankHoliday>>();

            var bankHolidayGeneratorFunctions = new Functions.BankHolidayGenerator(_bankHolidaysApiClient, mapper, _bankHolidayBulkInsertRepository, _functionLogRepository);
            bankHolidayGeneratorFunctions.GenerateBankHolidaysAsync(
                    new TimerInfo(timerSchedule, new ScheduleStatus()),
                    new ExecutionContext(),
                    new NullLogger<Functions.BankHolidayGenerator>())
                .GetAwaiter().GetResult();
        }

        [Fact]
        public void GetBankHolidays_Is_Called_Exactly_Once()
        {
            _bankHolidaysApiClient
                .Received(1)
                .GetBankHolidaysAsync();
        }

        [Fact]
        public void BulkInsert_Is_Called_Exactly_Once()
        {
            _bankHolidayBulkInsertRepository
                .Received(1)
                .BulkInsertAsync(Arg.Any<IList<BankHoliday>>());
        }

        [Fact]
        public void FunctionLogRepository_Create_Is_Not_Called()
        {
            _functionLogRepository
                .DidNotReceiveWithAnyArgs()
                .CreateAsync(Arg.Any<FunctionLog>());
        }
    }
}