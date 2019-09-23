using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces.FeedbackFactory;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.Services.FeedbackFactory;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public class ProviderFeedback
    {
        [FunctionName("SendProviderFeedbackEmails")]
        public async Task SendEmployerFeedbackEmails(
            [TimerTrigger("%EmployerFeedbackTrigger%")]
            TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            [Inject] IFeedbackServiceFactory<ProviderFeedbackService> serviceFactory,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                var emailsSent = await serviceFactory.CreateInstanceOf(FeedbackEmailTypes.ProviderFeedbackEmail)
                    .SendFeedbackEmailsAsync("System");

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} sent {emailsSent} emails\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errormessage = $"Error sending employer feedback emails. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.Create(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
            }
        }

        // ReSharper disable once UnusedMember.Global
        [FunctionName("ManualSendProviderFeedbackEmails")]
        public async Task<IActionResult> ManualSendEmployerFeedbackEmails(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger,
            [Inject] IFeedbackServiceFactory<ProviderFeedbackService> serviceFactory)
        {
            logger.LogInformation($"Function {context.FunctionName} triggered");

            var stopwatch = Stopwatch.StartNew();

            var emailsSent = await serviceFactory.CreateInstanceOf(FeedbackEmailTypes.ProviderFeedbackEmail)
                .SendFeedbackEmailsAsync("System");

            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} sent {emailsSent} emails\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

            return new OkObjectResult($"{emailsSent} emails sent.");
        }
    }
}
