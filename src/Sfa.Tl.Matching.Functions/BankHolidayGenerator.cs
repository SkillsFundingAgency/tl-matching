using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Api.Clients.Calendar;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Functions
{
    public class BankHolidayGenerator
    {
        [FunctionName("GenerateBankHolidays")]
        public async Task GenerateBankHolidaysAsync(
            [TimerTrigger("%BankHolidayGeneratorTrigger%")] TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            [Inject] ICalendarApiClient calendarApiClient,
            [Inject] IMapper mapper,
            [Inject] IBulkInsertRepository<BankHoliday> bankHolidayBulkInsertRepository,
            [Inject] IRepository<FunctionLog> functionLogRepository)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));
            try
            {
                logger.LogInformation($"Function {context.FunctionName} triggered");

                var stopwatch = Stopwatch.StartNew();
                var holidays = await calendarApiClient.GetBankHolidaysAsync();
                var createdRecords = await SaveHolidaysAsync(holidays, bankHolidayBulkInsertRepository, mapper);
                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {createdRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error loading Bank Holiday Data. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
                throw;
            }
        }

        // ReSharper disable once UnusedMember.Global
        [FunctionName("ManualGenerateBankHolidays")]
        public async Task<IActionResult> ManualGenerateBankHolidaysAsync(
#pragma warning disable IDE0060 // Remove unused parameter
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
#pragma warning restore IDE0060 // Remove unused parameter
            ExecutionContext context,
            ILogger logger,
            [Inject] ICalendarApiClient calendarApiClient,
            [Inject] IMapper mapper,
            [Inject] IBulkInsertRepository<BankHoliday> bankHolidayBulkInsertRepository,
            [Inject] IRepository<FunctionLog> functionLogRepository)
        {
            try
            {
                logger.LogInformation($"Function {context.FunctionName} triggered");

                var stopwatch = Stopwatch.StartNew();
                var holidays = await calendarApiClient.GetBankHolidaysAsync();
                var createdRecords = await SaveHolidaysAsync(holidays, bankHolidayBulkInsertRepository, mapper);
                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {createdRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new OkObjectResult($"{createdRecords} records created.");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error loading Bank Holiday Data. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });

                return new InternalServerErrorResult();
            }
        }

        private async Task<int> SaveHolidaysAsync(IList<BankHolidayResultDto> holidays,
            IBulkInsertRepository<BankHoliday> bankHolidayBulkInsertRepository,
            IMapper mapper)
        {
            var entities = mapper.Map<IList<BankHoliday>>(holidays).ToList();

            await bankHolidayBulkInsertRepository.BulkInsertAsync(entities);

            return entities.Count;
        }
    }
}