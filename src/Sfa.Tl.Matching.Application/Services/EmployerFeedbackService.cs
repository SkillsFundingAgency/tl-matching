using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmployerFeedbackService : IEmployerFeedbackService
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IRepository<BankHoliday> _bankHolidayRepository;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly ILogger<EmployerFeedbackService> _logger;

        public EmployerFeedbackService(
            MatchingConfiguration configuration,
            ILogger<EmployerFeedbackService> logger,
            IEmailService emailService,
            IRepository<BankHoliday> bankHolidayRepository,
            IOpportunityRepository opportunityRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
            _bankHolidayRepository = bankHolidayRepository;
            _opportunityRepository = opportunityRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> SendEmployerFeedbackEmailsAsync(string userName)
        {
            try
            {
                if (!IsNthWorkingDay(_configuration.EmployerFeedbackWorkingDayInMonth))
                {
                    _logger.LogInformation("Employer feedback service exited because today is not a valid day for processing.");
                    return 0;
                }

                var previousMonthDate = _dateTimeProvider.UtcNow().AddMonths(-1);
                var previousMonth = previousMonthDate.ToString("MMMM");
                
                var referrals = await _opportunityRepository.GetReferralsForEmployerFeedbackAsync(previousMonthDate);
                var referralsGroupedByEmployer = referrals.GroupBy(r => r.EmployerCrmId)
                    .ToDictionary(r => r.Key, r => r.OrderByDescending(e => e.ModifiedOn).ToList());
                
                foreach (var (_, value) in referralsGroupedByEmployer)
                {
                    var tokens = CreateTokens(value, previousMonth);

                    await _emailService.SendEmailAsync(null, null,
                        EmailTemplateName.EmployerFeedbackV2.ToString(),
                        value.First().EmployerContactEmail,
                        tokens,
                        userName);
                }

                return referralsGroupedByEmployer.Count;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending employer feedback emails. {ex.Message} ";

                _logger.LogError(ex, errorMessage);
                throw;
            }
        }

        public bool IsNthWorkingDay(int workingDay)
        {
            var today = _dateTimeProvider.UtcNow().Date;
            var holidays = _bankHolidayRepository
                .GetManyAsync(h => h.Date.Month == today.Month)
                .Select(h => h.Date)
                .ToList();
            return today == _dateTimeProvider.GetNthWorkingDayDate(today, workingDay, holidays);
        }

        private static IDictionary<string, string> CreateTokens(IReadOnlyCollection<EmployerFeedbackDto> employerFeedbackDtos,
            string previousMonth)
        {
            var latestEmployer = employerFeedbackDtos.First();

            var tokens = new Dictionary<string, string>
            {
                { "employer_contact_name", latestEmployer.EmployerContact },
                { "previous_month", previousMonth },
                { "opportunity_list", BuildOpportunityList(employerFeedbackDtos) }
            };

            var opportunityItemIds = employerFeedbackDtos.Select(ef => ef.OpportunityItemId.ToString()).Distinct().ToList();
            for (var i = 0; i < opportunityItemIds.Count; i++)
                tokens.Add($"opportunity_item_id_{i+1}", opportunityItemIds[i]);

            return tokens;
        }

        private static string BuildOpportunityList(IEnumerable<EmployerFeedbackDto> employerFeedbackDtos)
        {
            var opportunityList = employerFeedbackDtos.Select(employeeFeedback => 
                $"* {employeeFeedback.PlacementsDetail} x " + $"{employeeFeedback.JobRoleDetail} " +
                $"at {employeeFeedback.Town} " + $"{employeeFeedback.Postcode} " +
                $"on {employeeFeedback.ModifiedOn:dd MMMM yyyy}").ToList();

            return string.Join("\r\n", opportunityList);
        }
    }
}