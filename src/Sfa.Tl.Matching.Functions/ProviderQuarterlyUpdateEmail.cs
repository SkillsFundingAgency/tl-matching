using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Functions
{
    public class ProviderQuarterlyUpdateEmail
    {
        [FunctionName("SendProviderQuarterlyUpdateEmails")]
        public async Task SendProviderQuarterlyUpdateEmailsAsync([QueueTrigger(QueueName.ProviderQuarterlyRequestQueue, Connection = "BlobStorageConnectionString")]SendProviderQuarterlyUpdateEmail providerRequestData, 
            ExecutionContext context,
            ILogger logger,
            [Inject] IProviderQuarterlyUpdateEmailService providerQuarterlyUpdateEmailService,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            var backgroundProcessHistoryId = providerRequestData.BackgroundProcessHistoryId;

            try
            {
                var stopwatch = Stopwatch.StartNew();

                var emailsSent = await providerQuarterlyUpdateEmailService.SendProviderQuarterlyUpdateEmailsAsync(backgroundProcessHistoryId, "System");

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} sent {emailsSent} emails\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errormessage = $"Error sending quarterly update emails for feedback id {backgroundProcessHistoryId}. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
            }
        }
    }
}