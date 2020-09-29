using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Command;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Functions
{
    public class ProviderQuarterlyUpdateEmail
    {
        private readonly IProviderQuarterlyUpdateEmailService _providerQuarterlyUpdateEmailService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public ProviderQuarterlyUpdateEmail(
            IProviderQuarterlyUpdateEmailService providerQuarterlyUpdateEmailService,
            IRepository<FunctionLog> functionLogRepository)
        {
            _providerQuarterlyUpdateEmailService = providerQuarterlyUpdateEmailService;
            _functionLogRepository = functionLogRepository;
        }

        [FunctionName("SendProviderQuarterlyUpdateEmails")]
        public async Task SendProviderQuarterlyUpdateEmailsAsync([QueueTrigger(QueueName.ProviderQuarterlyRequestQueue, Connection = "BlobStorageConnectionString")]SendProviderQuarterlyUpdateEmail providerRequestData, 
            ExecutionContext context,
            ILogger logger)
        {
            var backgroundProcessHistoryId = providerRequestData.BackgroundProcessHistoryId;

            try
            {
                var stopwatch = Stopwatch.StartNew();

                var emailsSent = await _providerQuarterlyUpdateEmailService.SendProviderQuarterlyUpdateEmailsAsync(backgroundProcessHistoryId, "System");

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} sent {emailsSent} emails\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error sending quarterly update emails for feedback id {backgroundProcessHistoryId}. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
            }
        }
    }
}