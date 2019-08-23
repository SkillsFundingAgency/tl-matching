using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Functions.Extensions;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Functions
{
    public class EmployerFeedback
    {
        [FunctionName("SendEmployerFeedbackEmails")]
        public async Task SendEmployerFeedbackEmails(
            [TimerTrigger("%EmployerFeedbackTrigger%")]
            TimerInfo timer,
            ExecutionContext context,
            ILogger logger,
            [Inject] MatchingConfiguration configuration,
            [Inject] IDateTimeProvider dateTimeProvider,
            [Inject] IEmployerFeedbackService employerFeedbackService,
            [Inject] IRepository<BankHoliday> bankHolidayRepository,
            [Inject] IRepository<FunctionLog> functionlogRepository)
        {
            try
            {
                var stopwatch = Stopwatch.StartNew();

                var emailsSent = await SendEmployerFeedbackEmailsAsync(
                    configuration, dateTimeProvider,
                    employerFeedbackService, bankHolidayRepository);

                stopwatch.Stop();

                logger.LogInformation($"Function {context.FunctionName} sent {emailsSent} emails\n" +
                                      $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");
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
        }

        // ReSharper disable once UnusedMember.Global
        [FunctionName("ManualSendEmployerFeedbackEmails")]
        public async Task<IActionResult> ManualSendEmployerFeedbackEmails(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ExecutionContext context,
            ILogger logger,
            [Inject] MatchingConfiguration configuration,
            [Inject] IDateTimeProvider dateTimeProvider,
            [Inject] IEmployerFeedbackService employerFeedbackService,
            [Inject] IRepository<BankHoliday> bankHolidayRepository)
        {
            logger.LogInformation($"Function {context.FunctionName} triggered");

            var stopwatch = Stopwatch.StartNew();

            var emailsSent = await SendEmployerFeedbackEmailsAsync(
                configuration, dateTimeProvider,
                employerFeedbackService, bankHolidayRepository);

            stopwatch.Stop();

            logger.LogInformation($"Function {context.FunctionName} sent {emailsSent} emails\n" +
                                  $"\tTime taken: {stopwatch.ElapsedMilliseconds: #,###}ms");

            return new OkObjectResult($"{emailsSent} emails sent.");
        }

        private async Task<int> SendEmployerFeedbackEmailsAsync(
            MatchingConfiguration configuration,
            IDateTimeProvider dateTimeProvider,
            IEmployerFeedbackService employerFeedbackService,
            IRepository<BankHoliday> bankHolidayRepository)
        {
            var referralDate = await GetReferralDateAsync(configuration.EmployerFeedbackPeriodInWorkingDays,
                dateTimeProvider, bankHolidayRepository);

            var emailsSent = 0;
            if (referralDate != null)
            {
                emailsSent = await employerFeedbackService.SendEmployerFeedbackEmailsAsync(referralDate.Value, "System");
            }

            return emailsSent;
        }

        private async Task<DateTime?> GetReferralDateAsync(
            int employerFeedbackPeriodInWorkingDays,
            IDateTimeProvider dateTimeProvider,
            IRepository<BankHoliday> bankHolidayRepository)
        {
            var bankHolidays = await bankHolidayRepository.GetMany(
                    d => d.Date <= DateTime.Today)
                .Select(d => d.Date)
                .OrderBy(d => d.Date)
                .ToListAsync();

            if (dateTimeProvider.IsHoliday(dateTimeProvider.UtcNow().Date, bankHolidays))
                return null;

            var referralDate = dateTimeProvider
                .AddWorkingDays(
                    dateTimeProvider.UtcNow().Date,
                    -1 * (employerFeedbackPeriodInWorkingDays - 1),
                    bankHolidays)
                .AddSeconds(-1);

            return referralDate;
        }
    }
}
