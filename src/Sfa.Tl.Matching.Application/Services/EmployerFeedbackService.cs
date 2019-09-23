using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services.FeedbackFactory;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmployerFeedbackService : FeedbackService
    {
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly ILogger<EmployerFeedbackService> _logger;

        public EmployerFeedbackService(
            IMapper mapper,
            MatchingConfiguration configuration,
            ILogger<EmployerFeedbackService> logger,
            IDateTimeProvider dateTimeProvider,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            IRepository<BankHoliday> bankHolidayRepository,
            IOpportunityRepository opportunityRepository,
            IRepository<OpportunityItem> opportunityItemRepository) : base(mapper, configuration, dateTimeProvider,
            emailService, emailHistoryService, bankHolidayRepository, opportunityItemRepository)
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
                emailsSent = await SendEmployerFeedbackEmailsAsync(referralDate.Value, userName);
            }

            return emailsSent;
        }

        private async Task<int> SendEmployerFeedbackEmailsAsync(DateTime referralDate, string userName)
        {
            var referrals = await _opportunityRepository.GetReferralsForEmployerFeedbackAsync(referralDate);

            try
            {
                foreach (var referral in referrals)
                {
                    var tokens = new Dictionary<string, string>
                    {
                        { "employer_contact_name", referral.EmployerContact.ToTitleCase() },
                    };

                    await SendEmail(EmailTemplateName.EmployerFeedback, referral.OpportunityId, referral.EmployerContactEmail,
                        "Your industry placement progress – ESFA", tokens, userName);
                }

                await SetOpportunityItemsEmployerFeedbackAsSent(
                    referrals.Select(r => r.OpportunityItemId),
                    userName);

                return referrals.Count;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending employer feedback emails. {ex.Message} ";

                _logger.LogError(ex, errorMessage);
                throw;
            }
        }
    }
}
