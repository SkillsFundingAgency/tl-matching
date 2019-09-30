using System;
using System.Collections.Generic;
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
    public class EmployerAupaBlankEmail
    {
        private const string Subject = "Employer with a blank AUPA status";

        [FunctionName("SendEmployerAupaBlankEmail")]
        public async Task SendEmployerAupaBlankEmail([QueueTrigger(QueueName.EmployerAupaBlankEmailQueue, Connection = "BlobStorageConnectionString")]SendEmployerAupaBlankEmail employerAupaBlankEmail,
            ExecutionContext context,
            ILogger logger,
            [Inject] MatchingConfiguration matchingConfiguration,
            [Inject] IEmailService emailService,
            [Inject] IEmailHistoryService emailHistoryService,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            if (!matchingConfiguration.SendEmailEnabled) return;

            var stopwatch = Stopwatch.StartNew();

            var crmId = employerAupaBlankEmail.CrmId;

            try
            {
                var tokens = new Dictionary<string, string>
                {
                    { "employer_business_name", employerAupaBlankEmail.Name },
                    { "employer_owner", employerAupaBlankEmail.Owner },
                    { "crm_id", crmId.ToString() }
                };

                var supportInboxEmail = matchingConfiguration.SupportInboxEmail;
                await emailService.SendEmail(EmailTemplateName.EmployerAupaBlank.ToString(),
                    supportInboxEmail,
                    Subject,
                    tokens,
                    "");

                await emailHistoryService.SaveEmailHistory(EmailTemplateName.EmployerAupaBlank.ToString(),
                    tokens,
                    null,
                    supportInboxEmail,
                    "System");
            }
            catch (Exception e)
            {
                var errormessage = $"Error sending employer Aupa email for crm id, {crmId}. Internal Error Message {e}";

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