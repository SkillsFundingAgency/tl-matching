using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Functions
{
    public static class MatchingServiceReport
    {
        [FunctionName("GetMatchingServiceOpportunityReport")]
        public static async Task<IActionResult> GetMatchingServiceOpportunityReportAsync(
#pragma warning disable IDE0060 // Remove unused parameter
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
#pragma warning restore IDE0060 // Remove unused parameter
        ExecutionContext context,
        ILogger logger,
        [Inject] IOpportunityRepository opportunityRepository,
        [Inject] IRepository<FunctionLog> functionLogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var result = await opportunityRepository.GetMatchingServiceOpportunityReportAsync();

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {result.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                var errorMessage = $"Error Executing {context.FunctionName}. Internal Error Message {e}";

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

        [FunctionName("GetMatchingServiceProviderOpportunityReport")]
        public static async Task<IActionResult> GetMatchingServiceProviderOpportunityReportAsync(
#pragma warning disable IDE0060 // Remove unused parameter
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
#pragma warning restore IDE0060 // Remove unused parameter
        ExecutionContext context,
        ILogger logger,
        [Inject] IOpportunityRepository opportunityRepository,
        [Inject] IRepository<FunctionLog> functionLogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var result = await opportunityRepository.GetMatchingServiceProviderOpportunityReportAsync();

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {result.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                var errorMessage = $"Error Executing {context.FunctionName}. Internal Error Message {e}";

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

        [FunctionName("GetMatchingServiceEmployerReport")]
        public static async Task<IActionResult> GetMatchingServiceEmployerReportAsync(
#pragma warning disable IDE0060 // Remove unused parameter
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
#pragma warning restore IDE0060 // Remove unused parameter
            ExecutionContext context,
            ILogger logger,
            [Inject] IRepository<Domain.Models.Employer> employerRepository,
            [Inject] IRepository<FunctionLog> functionLogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var result = await employerRepository.GetManyAsync().CountAsync();

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {result}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                var errorMessage = $"Error Executing {context.FunctionName}. Internal Error Message {e}";

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

        [FunctionName("GetMatchingServiceProviderEmployerReport")]
        public static async Task<IActionResult> GetMatchingServiceProviderEmployerReportAsync(
#pragma warning disable IDE0060 // Remove unused parameter
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
#pragma warning restore IDE0060 // Remove unused parameter
            ExecutionContext context,
            ILogger logger,
            [Inject] IOpportunityRepository opportunityRepository,
            [Inject] IRepository<FunctionLog> functionLogRepository
        )
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                logger.LogInformation($"Function {context.FunctionName} triggered");

                var result = await opportunityRepository.GetMatchingServiceProviderEmployerReportAsync();

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} finished processing\n" +
                                      $"\tRows saved: {result.Count}\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new JsonResult(result);
            }
            catch (Exception e)
            {
                var errorMessage = $"Error Executing {context.FunctionName}. Internal Error Message {e}";

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
    }
}
