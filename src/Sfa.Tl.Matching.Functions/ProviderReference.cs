using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Functions
{
    public class ProviderReference
    {
        [FunctionName("ImportProviderReference")]
        public async Task ImportProviderReferenceAsync(
#pragma warning disable IDE0060 // Remove unused parameter
            [TimerTrigger("%ProviderReferenceTrigger%")] TimerInfo timer,
#pragma warning restore IDE0060 // Remove unused parameter
            ExecutionContext context,
            ILogger logger,
            [Inject] IReferenceDataService referenceDataService,
            [Inject] IDateTimeProvider dateTimeProvider,
            [Inject] IRepository<FunctionLog> functionLogRepository)
        {
            try
            {
                logger.LogInformation($"Function {context.FunctionName} triggered");

                var stopwatch = Stopwatch.StartNew();
                var createdRecords = await referenceDataService.SynchronizeProviderReferenceAsync(dateTimeProvider.MinValue());
                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {createdRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error loading ProviderReference Data. Internal Error Message {e}";

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
        [FunctionName("ManualImportProviderReference")]
        public async Task<IActionResult> ManualImportProviderReferenceAsync(
#pragma warning disable IDE0060 // Remove unused parameter
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
#pragma warning restore IDE0060 // Remove unused parameter
            ExecutionContext context,
            ILogger logger,
            [Inject] IReferenceDataService referenceDataService,
            [Inject] IDateTimeProvider dateTimeProvider,
            [Inject] IRepository<FunctionLog> functionLogRepository)
        {
            try
            {
                logger.LogInformation($"Function {context.FunctionName} triggered");

                var stopwatch = Stopwatch.StartNew();
                var createdRecords = await referenceDataService.SynchronizeProviderReferenceAsync(dateTimeProvider.MinValue());
                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {createdRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new OkObjectResult($"{createdRecords} records created.");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error loading ProviderReference Data. Internal Error Message {e}";

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
    }
}