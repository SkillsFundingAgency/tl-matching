using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.Extensions;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ReferralEmailService : IReferralEmailService
    {
        private readonly IMapper _mapper;
        private readonly MatchingConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IEmailHistoryService _emailHistoryService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;

        public ReferralEmailService(
                    IMapper mapper,
                    MatchingConfiguration configuration,
                    IDateTimeProvider dateTimeProvider,
                    IEmailService emailService,
                    IEmailHistoryService emailHistoryService,
                    IOpportunityRepository opportunityRepository,
                    IRepository<OpportunityItem> opportunityItemRepository,
                    IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository)
        {
            _mapper = mapper;
            _configuration = configuration;
            _dateTimeProvider = dateTimeProvider;
            _emailService = emailService;
            _emailHistoryService = emailHistoryService;
            _opportunityRepository = opportunityRepository;
            _opportunityItemRepository = opportunityItemRepository;
            _backgroundProcessHistoryRepository = backgroundProcessHistoryRepository;
        }

        public async Task SendEmployerReferralEmailAsync(int opportunityId, IEnumerable<int> itemIds, int backgroundProcessHistoryId, string username)
        {
            if (await GetBackgroundProcessHistoryData(backgroundProcessHistoryId) == null) return;

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
                    var providers = string.Join(", ", data.ProviderAndVenueDetails.Select(dto => dto.CustomisedProviderDisplayName));

                    sb.AppendLine($"# {data.WorkplaceTown} {data.WorkplacePostcode}");
                    sb.AppendLine($"*Job role: {data.JobRole}");
                    sb.AppendLine($"*Students wanted: {placements}");
                    sb.AppendLine($"*Providers selected: {providers}");
                    sb.AppendLine("");
                }

                tokens.Add("placements_list", sb.ToString());

                await SendEmail(EmailTemplateName.EmployerReferralComplex, opportunityId, employerReferral.EmployerContactEmail,
                    "Your industry placement referral – ESFA", tokens, employerReferral.CreatedBy);

                await UpdateBackgroundProcessHistory(GetBackgroundProcessHistoryData, backgroundProcessHistoryId, 1,
                    BackgroundProcessHistoryStatus.Complete, username);

            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending employer referral emails. {ex.Message} " +
                                   $"Opportunity id {opportunityId}";

                await UpdateBackgroundProcessHistory(GetBackgroundProcessHistoryData,
                    backgroundProcessHistoryId,
                    1,
                    BackgroundProcessHistoryStatus.Error,
                    username,
                    errorMessage);
            }
        }

        public async Task SendProviderReferralEmailAsync(int opportunityId, IEnumerable<int> itemIds, int backgroundProcessHistoryId, string username)
        {
            if (await GetBackgroundProcessHistoryData(backgroundProcessHistoryId) == null) return;

            var referrals = await GetOpportunityReferrals(opportunityId, itemIds);

            const string emailSubject = "Industry Placement Matching Referral";

            try
            {
                foreach (var referral in referrals)
                {
                    var placements = GetNumberOfPlacements(referral.PlacementsKnown, referral.Placements);

                    var tokens = new Dictionary<string, string>
                    {
                        { "contact_name", referral.ProviderPrimaryContact },
                        { "provider_name", referral.ProviderName },
                        { "route", referral.RouteName.ToLowerInvariant() },
                        { "venue_text", GetVenueText(referral.ProviderVenueName, 
                            referral.ProviderVenueTown,
                            referral.ProviderVenuePostcode,
                            referral.ProviderDisplayName) },
                        { "search_radius", referral.DistanceFromEmployer },
                        { "job_role_list", string.IsNullOrEmpty(referral.JobRole) || referral.JobRole == "None given"
                            ? $"* who is looking for students in courses related to {referral.RouteName.ToLowerInvariant()}"
                            : $"* who is looking for this job role: {referral.JobRole}" },
                        { "employer_business_name", referral.CompanyName },
                        { "employer_contact_name", referral.EmployerContact},
                        { "employer_contact_number", referral.EmployerContactPhone },
                        { "employer_contact_email", referral.EmployerContactEmail },
                        { "employer_town_postcode", $"{referral.Town} {referral.Postcode }" },
                        { "number_of_placements", placements }
                    };

                    await SendEmail(EmailTemplateName.ProviderReferral_V3, opportunityId, referral.ProviderPrimaryContactEmail,
                        emailSubject, tokens, referral.CreatedBy);

                    if (!string.IsNullOrEmpty(referral.ProviderSecondaryContactEmail))
                    {
                        tokens["contact_name"] = referral.ProviderSecondaryContact;
                        await SendEmail(EmailTemplateName.ProviderReferral_V3, opportunityId,
                            referral.ProviderSecondaryContactEmail,
                            emailSubject, tokens, referral.CreatedBy);
                    }

                    await CompleteSelectedReferrals(opportunityId, referral.OpportunityItemId, username);
                }

                await CompleteRemainingItems(opportunityId, username);

                await UpdateBackgroundProcessHistory(GetBackgroundProcessHistoryData, backgroundProcessHistoryId, referrals.Count,
                    BackgroundProcessHistoryStatus.Complete, username);

            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending provider referral emails. {ex.Message} " +
                                   $"Opportunity id {opportunityId}";

                await UpdateBackgroundProcessHistory(GetBackgroundProcessHistoryData, backgroundProcessHistoryId, referrals.Count,
                    BackgroundProcessHistoryStatus.Error, username, errorMessage);
            }
        }

        private async Task CompleteSelectedReferrals(int opportunityId, int itemId, string username)
        {
            var selectedOpportunityItemIds = _opportunityItemRepository.GetMany(oi => oi.Opportunity.Id == opportunityId
                                                                                      && oi.Id == itemId
                                                                                      && oi.IsSaved
                                                                                      && oi.IsSelectedForReferral
                                                                                      && !oi.IsCompleted)
                .Select(oi => oi.Id).ToList();

            if (selectedOpportunityItemIds.Count > 0)
            {
                await SetOpportunityItemsAsCompleted(selectedOpportunityItemIds, username);
            }
        }

        private async Task CompleteRemainingItems(int opportunityId, string username)
        {
            var remainingOpportunities = _opportunityItemRepository.GetMany(oi => oi.Opportunity.Id == opportunityId
                                                                                  && oi.IsSaved
                                                                                  && !oi.IsSelectedForReferral
                                                                                  && !oi.IsCompleted);

            var referralItems = remainingOpportunities.Where(oi => oi.OpportunityType == OpportunityType.Referral.ToString())
                .ToList();

            var provisionItems = remainingOpportunities
                .Where(oi => oi.OpportunityType == OpportunityType.ProvisionGap.ToString()).ToList();

            if (provisionItems.Count > 0 && referralItems.Count == 0)
            {
                var provisionIds = provisionItems.Select(oi => oi.Id).ToList();

                if (provisionIds.Count > 0)
                    await SetOpportunityItemsAsCompleted(provisionIds, username);
            }
        }

        private async Task SetOpportunityItemsAsCompleted(IEnumerable<int> opportunityItemIds, string username)
        {
            var itemsToBeCompleted = opportunityItemIds.Select(id => new OpportunityItemIsSelectedWithUsernameForCompleteDto
            {
                Id = id,
                Username = username
            });

            var updates = _mapper.Map<List<OpportunityItem>>(itemsToBeCompleted);

            await _opportunityItemRepository.UpdateManyWithSpecifedColumnsOnly(updates,
                x => x.IsCompleted,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
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
                : "At least 1";
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

        private async Task UpdateBackgroundProcessHistory(
            Func<int, Task<BackgroundProcessHistory>> data,
            int backgroundProcessHistoryId,
            int providerCount, BackgroundProcessHistoryStatus historyStatus,
            string userName, string errorMessage = null)
        {
            var backgroundProcessHistory = await data(backgroundProcessHistoryId);

            backgroundProcessHistory.RecordCount = providerCount;
            backgroundProcessHistory.Status = historyStatus.ToString();
            backgroundProcessHistory.StatusMessage = errorMessage;
            backgroundProcessHistory.ModifiedBy = userName;
            backgroundProcessHistory.ModifiedOn = _dateTimeProvider.UtcNow();

            await _backgroundProcessHistoryRepository.UpdateWithSpecifedColumnsOnly(backgroundProcessHistory,
                history => history.RecordCount,
                history => history.Status,
                history => history.StatusMessage,
                history => history.ModifiedBy,
                history => history.ModifiedOn);
        }

        private Func<int, Task<BackgroundProcessHistory>> GetBackgroundProcessHistoryData => BackgroundProcessHistoryData;

        private async Task<BackgroundProcessHistory> BackgroundProcessHistoryData(int backgroundProcessHistoryId)
        {
            var backgroundProcessHistory =
                await _backgroundProcessHistoryRepository
                    .GetSingleOrDefault(p => p.Id == backgroundProcessHistoryId);

            if (backgroundProcessHistory == null ||
                backgroundProcessHistory.Status != BackgroundProcessHistoryStatus.Pending.ToString())
            {
                return null;
            }

            return backgroundProcessHistory;
        }

        private static string GetVenueText(string venueName, string venueTown, string venuePostcode, string providerDisplayName)
        {
            var venueText = string.Empty;
            if (venueName != venuePostcode)
                venueText = $"at {ProviderDisplayExtensions.GetDisplayText(venueName, venuePostcode, providerDisplayName)} ";

            venueText += $"in {venueTown} {venuePostcode}";

            return venueText;
        }
    }
}