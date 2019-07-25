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
    public class EmployerReferralEmail
    {
        [FunctionName("SendEmployerReferralEmails")]
        public async Task SendEmployerReferralEmails([QueueTrigger(QueueName.EmployerReferralEmailQueue, Connection = "BlobStorageConnectionString")]SendEmployerReferralEmail employerReferralEmailData, 
            ExecutionContext context,
            ILogger logger,
            [Inject] IReferralEmailService referralEmailService,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            var stopwatch = Stopwatch.StartNew();

            var opportunityId = employerReferralEmailData.OpportunityId;
            var opportunityItemIds = employerReferralEmailData.OpportunityItemIds;
            var backgroundProcessHistoryId = employerReferralEmailData.BackgroundProcessHistoryId;

            try
            {
                await referralEmailService.SendEmployerReferralEmailAsync(opportunityId, opportunityItemIds,
                    backgroundProcessHistoryId, "system");
            }
            catch (Exception e)
            {
                var errormessage = $"Error sending employer referral email for opportunity id, {opportunityId}. Internal Error Message {e}";

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
