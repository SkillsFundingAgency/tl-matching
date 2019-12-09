using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;

namespace Sfa.Tl.Matching.Functions
{
    public class ProviderFeedback
    {
        [FunctionName("SendProviderFeedbackEmails")]
        public async Task SendProviderFeedbackEmails(
            [TimerTrigger("%ProviderFeedbackTrigger%")]
            TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            [Inject] IProviderFeedbackService providerFeedbackService,
            [Inject] IDateTimeProvider dateTimeProvider,
            [Inject] IRepository<BankHoliday> bankHolidayRepository,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            try
            {
                if (!IsNthWorkingDay(dateTimeProvider, bankHolidayRepository))
                {
                    logger.LogInformation($"Function {context.FunctionName} exited because today is not a valid day for processing.");
                    return;
                }

                var stopwatch = Stopwatch.StartNew();

                var emailsSent = await providerFeedbackService.SendProviderFeedbackEmailsAsync("System");

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} sent {emailsSent} emails\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
            }
            catch (Exception e)
            {
                var errormessage = $"Error sending provider feedback emails. Internal Error Message {e}";

                logger.LogError(errormessage);

                await functionlogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errormessage,
                    FunctionName = context.FunctionName,
                    RowNumber = -1
                });
            }
        }

        // ReSharper disable once UnusedMember.Global
        [FunctionName("ManualSendProviderFeedbackEmails")]
        public async Task<IActionResult> ManualSendProviderFeedbackEmails(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger,
            [Inject] IProviderFeedbackService providerFeedbackService)
        {
            logger.LogInformation($"Function {context.FunctionName} triggered");

            var stopwatch = Stopwatch.StartNew();

            var emailsSent = await providerFeedbackService.SendProviderFeedbackEmailsAsync("System");

            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} sent {emailsSent} emails\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

            return new OkObjectResult($"{emailsSent} emails sent.");
        }
        public bool IsNthWorkingDay(IDateTimeProvider dateTimeProvider,
            IRepository<BankHoliday> bankHolidayRepository)
        {
            var workingDay = 10;
            var today = dateTimeProvider.UtcNow().Date;
            var holidays = bankHolidayRepository
                .GetManyAsync(h => h.Date.Month == today.Month)
                .Select(h => h.Date)
                .ToList();

            return today == dateTimeProvider.GetNthWorkingDayDate(today, workingDay, holidays);
        }
    }
}