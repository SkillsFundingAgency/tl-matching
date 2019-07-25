using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ReferralEmailService : IReferralEmailService
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;

        public ReferralEmailService(
                    MatchingConfiguration configuration,
                    IDateTimeProvider dateTimeProvider,
                    IEmailService emailService,
                    IEmailHistoryService emailHistoryService,
                    IOpportunityRepository opportunityRepository,
                    IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository)
        {
            _configuration = configuration;
            _dateTimeProvider = dateTimeProvider;
            _emailService = emailService;
            _emailHistoryService = emailHistoryService;
            _opportunityRepository = opportunityRepository;
            _backgroundProcessHistoryRepository = backgroundProcessHistoryRepository;
        }

        
       public async Task SendEmployerReferralEmailAsync(int opportunityId, IEnumerable<int> itemIds, int backgroundProcessHistoryId, string username)
        {
            var backgroundProcessHistory =
                await _backgroundProcessHistoryRepository
                    .GetSingleOrDefault(p => p.Id == backgroundProcessHistoryId);

            if (backgroundProcessHistory == null ||
                backgroundProcessHistory.Status != BackgroundProcessHistoryStatus.Pending.ToString())
            {
                return;
            }

            try
            {
                var employerReferral = await GetEmployerReferrals(opportunityId, itemIds);
                var sb = new StringBuilder();

                if (employerReferral == null) return;

                var tokens = new Dictionary<string, string>
                {
                    { "employer_contact_name", employerReferral.EmployerContact },
                    { "employer_business_name", employerReferral.CompanyName },
                    { "employer_contact_number", employerReferral.EmployerContactPhone },
                    { "employer_contact_email", employerReferral.EmployerContactEmail },
                    { "employer_postcode", employerReferral.Postcode }
                };

                foreach (var data in employerReferral.WorkplaceDetails.OrderBy(dto => dto.WorkplaceTown))
                {
                    var placements = GetNumberOfPlacements(data.PlacementsKnown, data.Placements);
                    var providers = string.Join(", ", data.ProviderDetails.Select(dto => dto.ProviderName));

                    sb.AppendLine($"# {data.WorkplaceTown} {data.WorkplacePostcode}");
                    sb.AppendLine($"*Job role: {data.JobRole}");
                    sb.AppendLine($"*Students wanted: {placements}");
                    sb.AppendLine($"*Providers selected: {providers}");
                    sb.AppendLine("");
                }

                tokens.Add("placements_list", sb.ToString());

                await SendEmail(EmailTemplateName.EmployerReferralComplex, opportunityId, employerReferral.EmployerContactEmail,
                    "Your industry placement referral – ESFA National Apprenticeship Service", tokens, employerReferral.CreatedBy);

                await UpdatebackgroundProcessHistory(backgroundProcessHistory, 1,
                    BackgroundProcessHistoryStatus.Complete, username);

            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending employer referral emails. {ex.Message} " +
                                   $"Opportunity id {opportunityId}";

                await UpdatebackgroundProcessHistory(backgroundProcessHistory,
                    1,
                    BackgroundProcessHistoryStatus.Error,
                    username,
                    errorMessage);
            }

        }

        public async Task SendProviderReferralEmailAsync(int opportunityId, IEnumerable<int> itemIds, int backgroundProcessHistoryId, string username)
        {
            var backgroundProcessHistory =
                await _backgroundProcessHistoryRepository
                    .GetSingleOrDefault(p => p.Id == backgroundProcessHistoryId);

            if (backgroundProcessHistory == null ||
                backgroundProcessHistory.Status != BackgroundProcessHistoryStatus.Pending.ToString())
            {
                return;
            }

            var referrals = await GetOpportunityReferrals(opportunityId, itemIds);

            try
            {
                foreach (var referral in referrals)
                {
                    var toAddress = referral.ProviderPrimaryContactEmail;
                    var placements = GetNumberOfPlacements(referral.PlacementsKnown, referral.Placements);

                    var tokens = new Dictionary<string, string>
                    {
                        { "primary_contact_name", referral.ProviderPrimaryContact },
                        { "provider_name", referral.ProviderName },
                        { "route", referral.RouteName.ToLowerInvariant() },
                        { "venue_postcode", $"{referral.ProviderVenueTown} {referral.ProviderVenuePostcode}" },
                        { "search_radius", referral.SearchRadius.ToString() },
                        { "job_role", referral.JobRole },
                        { "employer_business_name", referral.CompanyName },
                        { "employer_contact_name", referral.EmployerContact},
                        { "employer_contact_number", referral.EmployerContactPhone },
                        { "employer_contact_email", referral.EmployerContactEmail },
                        { "employer_postcode", $"{referral.Town} {referral.Postcode }" },
                        { "number_of_placements", placements }
                    };

                    await SendEmail(EmailTemplateName.ProviderReferral, opportunityId, toAddress,
                        "Industry Placement Matching Referral", tokens, referral.CreatedBy);

                }

                await UpdatebackgroundProcessHistory(backgroundProcessHistory, referrals.Count,
                    BackgroundProcessHistoryStatus.Complete, username);

            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending provider referral emails. {ex.Message} " +
                                   $"Opportunity id {opportunityId}";

                await UpdatebackgroundProcessHistory(backgroundProcessHistory, referrals.Count,
                    BackgroundProcessHistoryStatus.Error, username, errorMessage);
            }

        }

        private async Task<EmployerReferralDto> GetEmployerReferrals(int opportunityId, IEnumerable<int> itemIds)
        {
            return await _opportunityRepository.GetEmployerReferrals(opportunityId, itemIds);
        }

        private async Task<IList<OpportunityReferralDto>> GetOpportunityReferrals(int opportunityId, IEnumerable<int> itemIds)
        {
            return await _opportunityRepository.GetProviderOpportunities(opportunityId, itemIds);
        }

        private static string GetNumberOfPlacements(bool? placementsKnown, int? placements)
        {
            return placementsKnown.GetValueOrDefault()
                ? placements.ToString()
                : "at least 1";
        }

        private async Task SendEmail(EmailTemplateName template, int? opportunityId,
            string toAddress, string subject,
            IDictionary<string, string> tokens, string createdBy)
        {
            if (!_configuration.SendEmailEnabled)
            {
                return;
            }

            await _emailService.SendEmail(template.ToString(),
                toAddress,
                subject,
                tokens,
                "");

            await _emailHistoryService.SaveEmailHistory(template.ToString(),
                tokens,
                opportunityId,
                toAddress,
                createdBy);
        }

        private async Task UpdatebackgroundProcessHistory(
            BackgroundProcessHistory backgroundProcessHistory,
            int providerCount, BackgroundProcessHistoryStatus historyStatus,
            string userName, string errorMessage = null)
        {
            backgroundProcessHistory.RecordCount = providerCount;
            backgroundProcessHistory.Status = historyStatus.ToString();
            backgroundProcessHistory.StatusMessage = errorMessage;
            backgroundProcessHistory.ModifiedBy = userName;
            backgroundProcessHistory.ModifiedOn = _dateTimeProvider.UtcNow();
            await _backgroundProcessHistoryRepository.Update(backgroundProcessHistory);
        }

    }
}
