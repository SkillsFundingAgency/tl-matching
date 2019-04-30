using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Functions
{
    public class ProviderFeedback
    {
        [FunctionName("SendProviderQuarterlyUpdateEmails")]
        public async Task SendProviderQuarterlyUpdateEmails([QueueTrigger(QueueName.ProviderQuarterlyRequestQueue, Connection = "BlobStorageConnectionString")]SendProviderFeedbackEmail providerRequestData, 
            ExecutionContext context,
            ILogger logger,
            [Inject] IProviderFeedbackService providerFeedbackService,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            var stopwatch = Stopwatch.StartNew();

            var providerFeedbackRequestHistoryId = providerRequestData.ProviderFeedbackRequestHistoryId;

            try
            {
                await providerFeedbackService.SendProviderQuarterlyUpdateEmailsAsync(providerFeedbackRequestHistoryId, "System");
            }
            catch (Exception e)
            {
                var errormessage = $"Error sending quarterly update emails for feedback id {providerFeedbackRequestHistoryId}. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.Create(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
                throw;
            }

            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} sent emails\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

        }
    }
}
