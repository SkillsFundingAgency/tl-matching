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
    public class ReferralEmails
    {
        private readonly IReferralEmailService _referralEmailService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public ReferralEmails(
            IReferralEmailService referralEmailService,
            IRepository<FunctionLog> functionLogRepository)
        {
            _referralEmailService = referralEmailService;
            _functionLogRepository = functionLogRepository;
        }

        [FunctionName("SendEmployerReferralEmails")]
        public async Task SendEmployerReferralEmailsAsync([QueueTrigger(QueueName.EmployerReferralEmailQueue, Connection = "BlobStorageConnectionString")]SendEmployerReferralEmail employerReferralEmailData, 
            ExecutionContext context,
            ILogger logger)
        {
            var stopwatch = Stopwatch.StartNew();

            var opportunityId = employerReferralEmailData.OpportunityId;
            var backgroundProcessHistoryId = employerReferralEmailData.BackgroundProcessHistoryId;
            var itemIds = employerReferralEmailData.ItemIds;

            try
            {
                await _referralEmailService.SendEmployerReferralEmailAsync(opportunityId, itemIds, backgroundProcessHistoryId, "System");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error sending employer referral email for opportunity id, {opportunityId}. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
            }

            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} sent emails\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
        }

        [FunctionName("SendProviderReferralEmails")]
        public async Task SendProviderReferralEmailsAsync([QueueTrigger(QueueName.ProviderReferralEmailQueue, Connection = "BlobStorageConnectionString")]SendProviderReferralEmail providerReferralEmailData,
            ExecutionContext context,
            ILogger logger)
        {
            var stopwatch = Stopwatch.StartNew();

            var opportunityId = providerReferralEmailData.OpportunityId;
            var backgroundProcessHistoryId = providerReferralEmailData.BackgroundProcessHistoryId;
            var itemIds = providerReferralEmailData.ItemIds;

            try
            {
                await _referralEmailService.SendProviderReferralEmailAsync(opportunityId, itemIds, backgroundProcessHistoryId, "System");
            }
            catch (Exception e)
            {
                var errorMessage = $"Error sending provider referral email for opportunity id, {opportunityId}. Internal Error Message {e}";

                logger.LogError(errorMessage);

                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
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
