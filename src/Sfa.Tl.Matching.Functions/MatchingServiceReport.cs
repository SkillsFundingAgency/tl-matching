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
using Sfa.Tl.Matching.Functions.Extensions;
// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Functions
{
    public static class MatchingServiceReport
    {
        [FunctionName("GetMatchingServiceOpportunityReport")]
        public static async Task<IActionResult> GetMatchingServiceOpportunityReportAsync(
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

                var result = await opportunityRepository.GetMatchingServiceOpportunityReportAsync();

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

                await functionlogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
                throw;
            }
        }

        [FunctionName("GetMatchingServiceProviderOpportunityReport")]
        public static async Task<IActionResult> GetMatchingServiceProviderOpportunityReportAsync(
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

                var result = await opportunityRepository.GetMatchingServiceProviderOpportunityReportAsync();

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

                await functionlogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
                throw;
            }
        }

        [FunctionName("GetMatchingServiceEmployerReport")]
        public static async Task<IActionResult> GetMatchingServiceEmployerReportAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger,
            [Inject] IRepository<Domain.Models.Employer> employerRepository,
            [Inject] IRepository<FunctionLog> functionlogRepository
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
                var errormessage = $"Error Executing {context.FunctionName}. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
                throw;
            }
        }

        [FunctionName("GetMatchingServiceProviderEmployerReport")]
        public static async Task<IActionResult> GetMatchingServiceProviderEmployerReportAsync(
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

                var result = await opportunityRepository.GetMatchingServiceProviderEmployerReportAsync();

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

                await functionlogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
                throw;
            }
        }
    }
}
