using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public class ProviderReference
    {
        [FunctionName("ImportProviderReference")]
        public async Task ImportProviderReference(
            [TimerTrigger("%ProviderReferenceTrigger%")] TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            [Inject] IReferenceDataService referenceDataService,
            [Inject] IDateTimeProvider dateTimeProvider,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            try
            {
                logger.LogInformation($"Function {context.FunctionName} triggered");

                var stopwatch = Stopwatch.StartNew();
                var createdRecords = await referenceDataService.SynchronizeProviderReference(dateTimeProvider.MinValue());
                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {createdRecords}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errormessage = $"Error loading ProviderReference Data. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.Create(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = nameof(ProviderReference),
                    RowNumber = -1
                });
                throw;
            }
        }

        [FunctionName("ManualImportProviderReference")]
        public async Task<IActionResult> ManualImportProviderReference(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger,
            [Inject] IReferenceDataService referenceDataService,
            [Inject] IDateTimeProvider dateTimeProvider)
        {
            logger.LogInformation($"Function {context.FunctionName} triggered");

            var stopwatch = Stopwatch.StartNew();
            var createdRecords = await referenceDataService.SynchronizeProviderReference(dateTimeProvider.MinValue());
            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                  $"\tRows saved: {createdRecords}\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

            return new OkObjectResult($"{createdRecords} records created.");
        }
    }
}