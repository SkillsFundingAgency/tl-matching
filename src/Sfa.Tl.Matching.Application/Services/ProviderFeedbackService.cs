using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services.FeedbackFactory;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{

    public class ProviderFeedbackService  : FeedbackService
    {
        private readonly ILogger<ProviderFeedbackService> _logger;
        private readonly IOpportunityRepository _opportunityRepository;

        public ProviderFeedbackService(
            IMapper mapper,
            MatchingConfiguration configuration,
            ILogger<ProviderFeedbackService> logger,
            IDateTimeProvider dateTimeProvider,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            IRepository<BankHoliday> bankHolidayRepository,
            IOpportunityRepository opportunityRepository,
            IRepository<OpportunityItem> opportunityItemRepository
            ) : base(mapper, configuration, dateTimeProvider, emailService, emailHistoryService, bankHolidayRepository, opportunityItemRepository)
        {
            _logger = logger;
            _opportunityRepository = opportunityRepository;
        }

        public override async Task<int> SendFeedbackEmailsAsync(string userName)
        {
            var referralDate = await GetReferralDateAsync();

            var emailsSent = 0;
            if (referralDate != null)
            {
                emailsSent = await SendProviderFeedbackEmailsAsync(referralDate.Value, userName);
            }

            return emailsSent;
        }

        private async Task<int> SendProviderFeedbackEmailsAsync(DateTime referralDate, string userName)
        {
            var data = await _opportunityRepository.GetAllReferralsForProviderFeedbackAsync(referralDate);
            var referrals = _opportunityRepository.GetDistinctReferralsForProviderFeedbackAsync(data);

            try
            {
                foreach (var referral in referrals)
                {
                    var tokens = new Dictionary<string, string>
                    {
                        { "contact_name", referral.Displayname },
                        { "company_name", referral.Companyname}
                    };

                    await SendEmail(EmailTemplateName.ProviderFeedback, referral.OpportunityId,
                        referral.ProviderPrimaryContactEmail, "Your industry placement progress – ESFA", tokens,
                        userName);

                    if (!string.IsNullOrWhiteSpace(referral.ProviderSecondaryContactEmail))
                        await SendEmail(EmailTemplateName.ProviderFeedback, referral.OpportunityId,
                            referral.ProviderSecondaryContactEmail, "Your industry placement progress – ESFA", tokens,
                            userName);
                }

                await SetOpportunityItemsProviderFeedbackAsSent(data.Select(r => r.OpportunityItemId),userName);

                return referrals.Count;
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
