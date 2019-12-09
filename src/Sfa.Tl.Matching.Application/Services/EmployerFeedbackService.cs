using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmployerFeedbackService : IEmployerFeedbackService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly ILogger<EmployerFeedbackService> _logger;

        public EmployerFeedbackService(
            ILogger<EmployerFeedbackService> logger,
            IEmailService emailService,
            IOpportunityRepository opportunityRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _logger = logger;
            _emailService = emailService;
            _opportunityRepository = opportunityRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> SendEmployerFeedbackEmailsAsync(string userName)
        {
            try
            {
                var previousMonthDate = _dateTimeProvider.UtcNow().AddMonths(-1);
                var referrals = await _opportunityRepository.GetReferralsForEmployerFeedbackAsync(previousMonthDate);
                var previousMonth = previousMonthDate.ToString("MMMM");

                var referralsGroupedByEmployer = referrals.GroupBy(r => r.EmployerCrmId)
                    .ToDictionary(r => r.Key, r => r.OrderByDescending(e => e.ModifiedOn).ToList());

                foreach (var employerGroup in referralsGroupedByEmployer)
                {
                    var employerContactEmail = employerGroup.Value.First().EmployerContactEmail;
                    var employerContact = employerGroup.Value.First().EmployerContact;

                    var tokens = new Dictionary<string, string>
                    {
                        { "employer_contact_name", employerContact },
                        { "previous_month", previousMonth },
                        { "opportunity_list", BuildOpportunityList(employerGroup.Value) }
                    };

                    await _emailService.SendEmailAsync(null,
                        EmailTemplateName.EmployerFeedbackV2.ToString(),
                        employerContactEmail,
                        tokens,
                        userName);
                }

                return referrals.Count;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending employer feedback emails. {ex.Message} ";

                _logger.LogError(ex, errorMessage);
                throw;
            }
        }

        private static string BuildOpportunityList(IEnumerable<EmployerFeedbackDto> employerFeedbackDtos)
        {
            var opportunityListBuilder = new StringBuilder();
            foreach (var employeeFeedback in employerFeedbackDtos)
            {
                opportunityListBuilder.AppendLine($"* {employeeFeedback.PlacementsDetail} x " +
                                                  $"{employeeFeedback.JobRoleDetail} students at {employeeFeedback.Town} {employeeFeedback.Postcode}");
            }

            return opportunityListBuilder.ToString();
        }
    }
}