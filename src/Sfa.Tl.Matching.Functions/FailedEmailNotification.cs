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
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Functions
{
    public class FailedEmailNotification
    {
        [FunctionName("SendFailedEmailNotification")]
        public async Task SendFailedEmailNotification(
            [QueueTrigger(QueueName.FailedEmailQueue, Connection = "BlobStorageConnectionString")]
            SendFailedEmail failedEmailData,
            ExecutionContext context,
            ILogger logger,
            [Inject] MatchingConfiguration matchingConfiguration,
            [Inject] IEmailDeliveryStatusService emailDeliveryStatusService,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            if (!matchingConfiguration.SendEmailEnabled) return;

            var stopwatch = Stopwatch.StartNew();

            try
            {
                await emailDeliveryStatusService.SendEmailDeliveryStatusAsync(failedEmailData.NotificationId);
            }
            catch (Exception e)
            {
                var errormessage =
                    $"Error sending failed email notification for Notification Id: {failedEmailData.NotificationId}. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
            }

            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} sent email\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
        }
    }
}