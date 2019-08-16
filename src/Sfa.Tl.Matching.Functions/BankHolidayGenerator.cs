using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Api.Clients.Calendar;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Functions
{
    public class BankHolidayGenerator
    {
        [FunctionName("GenerateBankHoliday")]
        public async Task GenerateBankHoliday(
            [TimerTrigger("0 0 0 1 1,6 *", RunOnStartup = true)]
            TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            [Inject] ICalendarApiClient calendarApiClient,
            [Inject] IMapper mapper,
            [Inject] IRepository<BankHoliday> bankHolidayRepository,
            [Inject] IRepository<FunctionLog> functionLogRepository)
        {
            try
            {
                logger.LogInformation($"Function {context.FunctionName} triggered");

                var stopwatch = Stopwatch.StartNew();
                var holidays = await calendarApiClient.GetBankHolidaysAsync();
                var createdRecords = await SaveHolidaysAsync(holidays, bankHolidayRepository, mapper);
                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {createdRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errormessage = $"Error loading Bank Holiday Data. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionLogRepository.Create(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
                throw;
            }
        }

        // ReSharper disable once UnusedMember.Global
        [FunctionName("ManualGenerateBankHoliday")]
        public async Task<IActionResult> ManualGenerateBankHoliday(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger,
            [Inject] ICalendarApiClient calendarApiClient,
            [Inject] IMapper mapper,
            [Inject] IRepository<BankHoliday> bankHolidayRepository)
        {
            logger.LogInformation($"Function {context.FunctionName} triggered");

            var stopwatch = Stopwatch.StartNew();
            var holidays = await calendarApiClient.GetBankHolidaysAsync();
            var createdRecords = await SaveHolidaysAsync(holidays, bankHolidayRepository, mapper);
            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                  $"\tRows saved: {createdRecords}\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

            return new OkObjectResult($"{createdRecords} records created.");
        }

        private async Task<int> SaveHolidaysAsync(IList<BankHolidayResultDto> holidays, IRepository<BankHoliday> bankHolidayRepository, IMapper mapper)
        {
            //await bankHolidayRepository.DeleteMany();
            var entities = mapper.Map<IList<BankHoliday>>(holidays).ToList();

            //TODO: Set CreatedBy = "System" - mapper?
            return await bankHolidayRepository.CreateMany(entities);
        }
    }
}