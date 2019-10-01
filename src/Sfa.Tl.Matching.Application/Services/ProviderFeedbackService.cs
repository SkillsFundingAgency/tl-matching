using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderFeedbackService : FeedbackService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProviderFeedbackService> _logger;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IRepository<Provider> _providerRepository;

        public ProviderFeedbackService(
            IMapper mapper,
            MatchingConfiguration configuration,
            ILogger<ProviderFeedbackService> logger,
            IDateTimeProvider dateTimeProvider,
            IEmailService emailService,
            IEmailHistoryService emailHistoryService,
            IRepository<BankHoliday> bankHolidayRepository,
            IOpportunityRepository opportunityRepository,
            IRepository<Provider> providerRepository,
            IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository
            ) : base(configuration, emailService, emailHistoryService, dateTimeProvider, bankHolidayRepository, backgroundProcessHistoryRepository)
        {
            _mapper = mapper;
            _logger = logger;
            _opportunityRepository = opportunityRepository;
            _providerRepository = providerRepository;
        }

        public override async Task<int> SendFeedbackEmailsAsync(string userName)
        {
            var emailsSent = 0;
            if (ReferralDate != null)
            {
                emailsSent = await SendProviderFeedbackEmailsAsync(ReferralDate.Value, userName);
            }

            return emailsSent;
        }

        private async Task<int> SendProviderFeedbackEmailsAsync(DateTime referralDate, string userName)
        {
            var referrals = await _opportunityRepository.GetAllReferralsForProviderFeedbackAsync(referralDate);

            try
            {
                var historyId = await CreateBackgroundProcessHistory(BackgroundProcessType.ProviderFeedbackEmail);

                foreach (var referral in referrals)
                {
                    var tokens = new Dictionary<string, string>
                    {
                        { "contact_name", referral.ProviderPrimaryContactName },
                        { "company_name", referral.Companyname}
                    };

                    await SendEmail(EmailTemplateName.ProviderFeedback, referral.OpportunityId,
                        referral.ProviderPrimaryContactEmail, tokens,
                        userName);

                    if (!string.IsNullOrWhiteSpace(referral.ProviderSecondaryContactEmail) && !string.IsNullOrWhiteSpace(referral.ProviderSecondaryContactName))
                    {
                        tokens["contact_name"] = referral.ProviderSecondaryContactName;

                        await SendEmail(EmailTemplateName.ProviderFeedback, referral.OpportunityId,
                            referral.ProviderSecondaryContactEmail, tokens,
                            userName);
                    }
                }

                await SetProviderFeedbackSentOnDateAsync(referrals.Select(r => r.ProviderId), userName);

                await UpdateBackgroundProcessHistory(historyId, referrals.Count);

                return referrals.Count;
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending provider feedback emails. {ex.Message} ";
                _logger.LogError(ex, errorMessage);

                throw;
            }
        }

        private async Task SetProviderFeedbackSentOnDateAsync(IEnumerable<int> providerIds, string userName)
        {
            var itemsToBeCompleted = providerIds.Select(id => new UsernameForFeedbackSentDto
            {
                Id = id,
                Username = userName
            });

            var updates = _mapper.Map<List<Provider>>(itemsToBeCompleted);

            await _providerRepository.UpdateManyWithSpecifedColumnsOnlyAsync(updates,
                x => x.ProviderFeedbackSentOn,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }
    }
}
