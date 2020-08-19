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
    public class ProviderFeedback
    {
        private readonly IProviderFeedbackService _providerFeedbackService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public ProviderFeedback(
            IProviderFeedbackService providerFeedbackService,
            IRepository<FunctionLog> functionLogRepository)
        {
            _providerFeedbackService = providerFeedbackService;
            _functionLogRepository = functionLogRepository;
        }

        [FunctionName("SendProviderFeedbackEmails")]
        public async Task SendProviderFeedbackEmails(
#pragma warning disable IDE0060 // Remove unused parameter
            [TimerTrigger("%ProviderFeedbackTrigger%")] TimerInfo timer,
#pragma warning restore IDE0060 // Remove unused parameter
            ExecutionContext context,
            ILogger logger)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                var emailsSent = await _providerFeedbackService.SendProviderFeedbackEmailsAsync("System");

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} sent {emailsSent} emails\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error sending provider feedback emails. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
            }
        }

        // ReSharper disable once UnusedMember.Global
        [FunctionName("ManualSendProviderFeedbackEmails")]
        public async Task<IActionResult> ManualSendProviderFeedbackEmails(
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

                var emailsSent = await _providerFeedbackService.SendProviderFeedbackEmailsAsync("System");

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} sent {emailsSent} emails\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new OkObjectResult($"{emailsSent} emails sent.");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error sending provider feedback emails. Internal Error Message {e}";

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
    }
}