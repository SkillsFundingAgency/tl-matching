using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public static class MatchingServiceReport
    {
        [FunctionName("GetServiceOpportunityReport")]
        public static async Task<IActionResult> GetServiceOpportunityReport(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, 
        ExecutionContext context,
        ILogger logger, 
        [Inject] IOpportunityRepository opportunityRepository,
        [Inject] IRepository<FunctionLog> functionlogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var result = await opportunityRepository.GetServiceOpportunityReportAsync();

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {result.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                var errormessage = $"Error Executing {context.FunctionName}. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.Create(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = "MatchingServiceReport",
                    RowNumber = -1
                });
                throw;
            }        }

        [FunctionName("GetProviderOpportunityReport")]
        public static async Task<IActionResult> GetProviderOpportunityReport(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req, 
        ExecutionContext context,
        ILogger logger, 
        [Inject] IOpportunityRepository opportunityRepository,
        [Inject] IRepository<FunctionLog> functionlogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var result = await opportunityRepository.GetProviderOpportunityReportAsync();

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {result.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                var errormessage = $"Error Executing {context.FunctionName}. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.Create(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = "MatchingServiceReport",
                    RowNumber = -1
                });
                throw;
            }        }
    }
}
