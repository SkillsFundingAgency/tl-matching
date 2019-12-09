using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderFeedbackService : IProviderFeedbackService
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly ILogger<ProviderFeedbackService> _logger;

        public ProviderFeedbackService(
            ILogger<ProviderFeedbackService> logger,
            IEmailService emailService,
            IOpportunityRepository opportunityRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _logger = logger;
            _emailService = emailService;
            _opportunityRepository = opportunityRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> SendProviderFeedbackEmailsAsync(string userName)
        {
            try
            {
                var previousMonthDate = _dateTimeProvider.UtcNow().AddMonths(-1);
                var referrals = await _opportunityRepository.GetReferralsForProviderFeedbackAsync(previousMonthDate);
                var previousMonth = previousMonthDate.ToString("MMMM");

                var referralsGroupedByProvider = referrals.GroupBy(r => r.ProviderName)
                    .ToDictionary(r => r.Key, r => r.ToList());

                foreach (var providerGroup in referralsGroupedByProvider)
                {
                    var provider = providerGroup.Value.First();
                    var providerName = provider.ProviderName;
                    var primaryContactEmail = provider.PrimaryContactEmail;
                    var primaryContact = provider.PrimaryContact;
                    var secondaryContactEmail = provider.SecondaryContactEmail;
                    var secondaryContact = provider.SecondaryContact;

                    var tokens = new Dictionary<string, string>
                    {
                        { "provider_name", providerName },
                        { "provider_contact_name", primaryContact },
                        { "previous_month", previousMonth },
                        { "employers_list", "" },
                        { "other_email_details", "" },
                    };

                    await _emailService.SendEmailAsync(null,
                        EmailTemplateName.ProviderFeedbackV2.ToString(),
                        primaryContactEmail,
                        tokens,
                        userName);
                }

                return referralsGroupedByProvider.Count;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending provider feedback emails. {ex.Message} ";

                _logger.LogError(ex, errorMessage);
                throw;
            }
        }
    }
}
