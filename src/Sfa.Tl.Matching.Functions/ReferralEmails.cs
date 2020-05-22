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
    public class ReferralEmails
    {
        [FunctionName("SendEmployerReferralEmails")]
        public async Task SendEmployerReferralEmailsAsync([QueueTrigger(QueueName.EmployerReferralEmailQueue, Connection = "BlobStorageConnectionString")]SendEmployerReferralEmail employerReferralEmailData, 
            ExecutionContext context,
            ILogger logger,
            [Inject] IReferralEmailService referralEmailService,
            [Inject] IRepository<FunctionLog> functionLogRepository)
        {
            var stopwatch = Stopwatch.StartNew();

            var opportunityId = employerReferralEmailData.OpportunityId;
            var backgroundProcessHistoryId = employerReferralEmailData.BackgroundProcessHistoryId;
            var itemIds = employerReferralEmailData.ItemIds;

            try
            {
                await referralEmailService.SendEmployerReferralEmailAsync(opportunityId, itemIds, backgroundProcessHistoryId, "System");
            }
            catch (Exception e)
            {
                var errormessage = $"Error sending employer referral email for opportunity id, {opportunityId}. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionLogRepository.CreateAsync(new FunctionLog
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

        [FunctionName("SendProviderReferralEmails")]
        public async Task SendProviderReferralEmailsAsync([QueueTrigger(QueueName.ProviderReferralEmailQueue, Connection = "BlobStorageConnectionString")]SendProviderReferralEmail providerReferralEmailData,
            ExecutionContext context,
            ILogger logger,
            [Inject] IReferralEmailService referralEmailService,
            [Inject] IRepository<FunctionLog> functionLogRepository)
        {
            var stopwatch = Stopwatch.StartNew();

            var opportunityId = providerReferralEmailData.OpportunityId;
            var backgroundProcessHistoryId = providerReferralEmailData.BackgroundProcessHistoryId;
            var itemIds = providerReferralEmailData.ItemIds;

            try
            {
                await referralEmailService.SendProviderReferralEmailAsync(opportunityId, itemIds, backgroundProcessHistoryId, "System");
            }
            catch (Exception e)
            {
                var errormessage = $"Error sending provider referral email for opportunity id, {opportunityId}. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionLogRepository.CreateAsync(new FunctionLog
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
