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
using Sfa.Tl.Matching.Api.Clients.BankHolidays;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Functions
{
    public class BankHolidayGenerator
    {
        private readonly IBankHolidaysApiClient _bankHolidaysApiClient;
        private readonly IMapper _mapper;
        private readonly IBulkInsertRepository<BankHoliday> _bankHolidayBulkInsertRepository;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public BankHolidayGenerator(
                IBankHolidaysApiClient bankHolidaysApiClient,
                IMapper mapper,
                IBulkInsertRepository<BankHoliday> bankHolidayBulkInsertRepository,
                IRepository<FunctionLog> functionLogRepository)
        {
            _bankHolidaysApiClient = bankHolidaysApiClient;
            _mapper = mapper;
            _bankHolidayBulkInsertRepository = bankHolidayBulkInsertRepository;
            _functionLogRepository = functionLogRepository;
        }

        [FunctionName("GenerateBankHolidays")]
        public async Task GenerateBankHolidaysAsync(
            [TimerTrigger("%BankHolidayGeneratorTrigger%")] TimerInfo timer,
            ExecutionContext context,
            ILogger logger)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));
            try
            {
                logger.LogInformation($"Function {context.FunctionName} triggered");

                var stopwatch = Stopwatch.StartNew();
                var holidays = await _bankHolidaysApiClient.GetBankHolidaysAsync();
                var createdRecords = await SaveHolidaysAsync(holidays);
                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {createdRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error loading Bank Holiday Data. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
                throw;
            }
        }

        [FunctionName("ManualGenerateBankHolidays")]
        public async Task<IActionResult> ManualGenerateBankHolidaysAsync(
#pragma warning disable IDE0060 // Remove unused parameter
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
#pragma warning restore IDE0060 // Remove unused parameter
            ExecutionContext context,
            ILogger logger)
        {
            try
            {
                logger.LogInformation($"Function {context.FunctionName} triggered");

                var stopwatch = Stopwatch.StartNew();
                var holidays = await _bankHolidaysApiClient.GetBankHolidaysAsync();
                var createdRecords = await SaveHolidaysAsync(holidays);
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

                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });

                return new InternalServerErrorResult();
            }
        }

        private async Task<int> SaveHolidaysAsync(IList<BankHolidayResultDto> holidays)
        {
            var entities = _mapper.Map<IList<BankHoliday>>(holidays).ToList();

            await _bankHolidayBulkInsertRepository.BulkInsertAsync(entities);

            return entities.Count;
        }
    }
}