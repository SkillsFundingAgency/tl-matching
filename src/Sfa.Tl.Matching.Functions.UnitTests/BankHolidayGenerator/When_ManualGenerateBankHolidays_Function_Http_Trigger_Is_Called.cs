using System;
using System.Collections.Generic;
using System.Net.Http;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs;
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
    public class When_ManualGenerateBankHolidays_Function_Http_Trigger_Is_Called
    {
        private readonly IBankHolidaysApiClient _bankHolidaysApiClient;
        private readonly IBulkInsertRepository<BankHoliday> _bankHolidayBulkInsertRepository;
        private readonly IRepository<FunctionLog> _functionLogRepository;
        
        public When_ManualGenerateBankHolidays_Function_Http_Trigger_Is_Called()
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

            var config = new MapperConfiguration(c => c.AddMaps(typeof(BankHolidayMapper).Assembly));
            var mapper = new Mapper(config);

            _bankHolidayBulkInsertRepository = Substitute.For<IBulkInsertRepository<BankHoliday>>();

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var httpContext = new DefaultHttpContext();
            var request = httpContext.Request;
            request.Method = HttpMethod.Get.ToString();

            var bankHolidayGeneratorFunctions = new Functions.BankHolidayGenerator(_bankHolidaysApiClient, mapper, _bankHolidayBulkInsertRepository, _functionLogRepository);
            bankHolidayGeneratorFunctions.ManualGenerateBankHolidaysAsync(
                    request,
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