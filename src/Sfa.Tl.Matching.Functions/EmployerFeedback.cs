using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public class EmployerFeedback
    {
        [FunctionName("SendEmployerFeedbackEmails")]
        public async Task SendEmployerFeedbackEmails(
            [TimerTrigger("%EmployerFeedbackTrigger%", RunOnStartup = true)]
            TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            //[Inject] IEmployerFeedbackService employerFeedbackService,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                //await employerFeedbackService.SendFeedbackEmailsAsync("System");
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

            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} sent emails\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
        }
    }
}
