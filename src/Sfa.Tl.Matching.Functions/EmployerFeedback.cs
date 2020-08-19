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
    public class EmployerFeedback
    {
        private readonly IEmployerFeedbackService _employerFeedbackService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public EmployerFeedback(
            IEmployerFeedbackService employerFeedbackService,
            IRepository<FunctionLog> functionLogRepository)
        {
            _employerFeedbackService = employerFeedbackService;
            _functionLogRepository = functionLogRepository;
        }

        [FunctionName("SendEmployerFeedbackEmails")]
        public async Task SendEmployerFeedbackEmails(
#pragma warning disable IDE0060 // Remove unused parameter
            [TimerTrigger("%EmployerFeedbackTrigger%")] TimerInfo timer,
#pragma warning restore IDE0060 // Remove unused parameter
            ExecutionContext context,
            ILogger logger)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                var emailsSent = await _employerFeedbackService.SendEmployerFeedbackEmailsAsync("System");

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} sent {emailsSent} emails\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error sending employer feedback emails. Internal Error Message {e}";

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
        [FunctionName("ManualSendEmployerFeedbackEmails")]
        public async Task<IActionResult> ManualSendEmployerFeedbackEmails(
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

                var emailsSent = await _employerFeedbackService.SendEmployerFeedbackEmailsAsync("System");

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} sent {emailsSent} emails\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

                return new OkObjectResult($"{emailsSent} emails sent.");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error sending employer feedback emails. Internal Error Message {e}";

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