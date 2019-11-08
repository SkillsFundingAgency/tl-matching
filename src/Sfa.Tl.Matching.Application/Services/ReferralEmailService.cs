using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Humanizer;
using Sfa.Tl.Matching.Application.Extensions;
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
        private readonly IMapper _mapper;
        private readonly MatchingConfiguration _configuration;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;

        public ReferralEmailService(
                    IMapper mapper,
                    MatchingConfiguration configuration,
                    IDateTimeProvider dateTimeProvider,
                    IEmailService emailService,
                    IOpportunityRepository opportunityRepository,
                    IRepository<OpportunityItem> opportunityItemRepository,
                    IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository)
        {
            _mapper = mapper;
            _configuration = configuration;
            _dateTimeProvider = dateTimeProvider;
            _emailService = emailService;
            _opportunityRepository = opportunityRepository;
            _opportunityItemRepository = opportunityItemRepository;
            _backgroundProcessHistoryRepository = backgroundProcessHistoryRepository;
        }

        public async Task SendEmployerReferralEmailAsync(int opportunityId, IEnumerable<int> itemIds, int backgroundProcessHistoryId, string username)
        {
            if (await GetBackgroundProcessHistoryData(backgroundProcessHistoryId) == null) return;

            try
            {
                var employerReferral = await GetEmployerReferralsAsync(opportunityId, itemIds);
                var sb = new StringBuilder();

                if (employerReferral == null) return;

                var tokens = new Dictionary<string, string>
                {
                    { "employer_contact_name", employerReferral.EmployerContact.ToTitleCase() },
                    { "employer_business_name", employerReferral.CompanyName.ToTitleCase() },
                    { "employer_contact_number", employerReferral.EmployerContactPhone },
                    { "employer_contact_email", employerReferral.EmployerContactEmail }
                };

                foreach (var data in employerReferral.WorkplaceDetails.OrderBy(dto => dto.WorkplaceTown))
                {
                    var placements = GetNumberOfPlacements(data.PlacementsKnown, data.Placements);

                    sb.AppendLine($"# {data.WorkplaceTown} {data.WorkplacePostcode}");
                    sb.AppendLine($"* Job role: {data.JobRole}");
                    sb.AppendLine($"* Students wanted: {placements}");

                    var count = 1;
                    foreach (var providerAndVenue in data.ProviderAndVenueDetails)
                    {
                        sb.AppendLine($"* {count.ToOrdinalWords().ToTitleCase()} provider selected: {providerAndVenue.CustomisedProviderDisplayName}");
                        sb.Append("Primary contact: ");
                        sb.AppendLine(FormatContactDetails(providerAndVenue.ProviderPrimaryContact, providerAndVenue.ProviderPrimaryContactPhone, providerAndVenue.ProviderPrimaryContactEmail));

                        if (!string.IsNullOrWhiteSpace(providerAndVenue.ProviderSecondaryContact))
                        {
                            sb.AppendLine($"Secondary contact: {FormatContactDetails(providerAndVenue.ProviderSecondaryContact, providerAndVenue.ProviderSecondaryContactPhone, providerAndVenue.ProviderSecondaryContactEmail)}");
                        }

                        count++;
                    }
                    sb.AppendLine("");
                }

                tokens.Add("placements_list", sb.ToString());

                await SendEmailAsync(EmailTemplateName.EmployerReferralV4, opportunityId, employerReferral.EmployerContactEmail, tokens, employerReferral.CreatedBy);

                await UpdateBackgroundProcessHistoryAsync(GetBackgroundProcessHistoryData, backgroundProcessHistoryId, 1,
                    BackgroundProcessHistoryStatus.Complete, username);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending employer referral emails. {ex.Message} " +
                                   $"Opportunity id {opportunityId}";

                await UpdateBackgroundProcessHistoryAsync(GetBackgroundProcessHistoryData,
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

            var referrals = await GetOpportunityReferralsAsync(opportunityId, itemIds);

            try
            {
                foreach (var referral in referrals)
                {
                    var placements = GetNumberOfPlacements(referral.PlacementsKnown, referral.Placements);

                    var tokens = new Dictionary<string, string>
                    {
                        { "contact_name", referral.ProviderPrimaryContact },
                        { "provider_name", referral.ProviderDisplayName },
                        { "route", referral.RouteName.ToLowerInvariant() },
                        { "venue_text", referral.VenueText },
                        { "search_radius", referral.DistanceFromEmployer },
                        { "job_role_list", string.IsNullOrEmpty(referral.JobRole) || referral.JobRole == "None given"
                            ? $"* looking for students in courses related to {referral.RouteName.ToLowerInvariant()}"
                            : $"* looking for this job role: {referral.JobRole}" },
                        { "employer_business_name", referral.CompanyName.ToTitleCase() },
                        { "employer_contact_name", referral.EmployerContact.ToTitleCase() },
                        { "employer_contact_number", referral.EmployerContactPhone },
                        { "employer_contact_email", referral.EmployerContactEmail },
                        { "employer_town_postcode", $"{referral.Town} {referral.Postcode }" },
                        { "number_of_placements", placements }
                    };

                    const EmailTemplateName template = EmailTemplateName.ProviderReferralV4;
                    await SendEmailAsync(template, opportunityId, referral.ProviderPrimaryContactEmail, tokens, referral.CreatedBy);

                    if (!string.IsNullOrWhiteSpace(referral.ProviderSecondaryContactEmail) && !string.IsNullOrWhiteSpace(referral.ProviderSecondaryContact))
                    {
                        tokens["contact_name"] = referral.ProviderSecondaryContact;
                        await SendEmailAsync(template, opportunityId,
                            referral.ProviderSecondaryContactEmail, tokens, referral.CreatedBy);
                    }

                    await CompleteSelectedReferralsAsync(opportunityId, referral.OpportunityItemId, username);
                }

                await CompleteRemainingItemsAsync(opportunityId, username);

                await UpdateBackgroundProcessHistoryAsync(GetBackgroundProcessHistoryData, backgroundProcessHistoryId, referrals.Count,
                    BackgroundProcessHistoryStatus.Complete, username);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending provider referral emails. {ex.Message} " +
                                   $"Opportunity id {opportunityId}";

                await UpdateBackgroundProcessHistoryAsync(GetBackgroundProcessHistoryData, backgroundProcessHistoryId, referrals.Count,
                    BackgroundProcessHistoryStatus.Error, username, errorMessage);
            }
        }

        private string FormatContactDetails(string name, string phone, string email)
        {
            var sb = new StringBuilder($"{name}");

            var hasPhone = !string.IsNullOrWhiteSpace(phone);
            var hasEmail = !string.IsNullOrWhiteSpace(email);

            if (hasPhone || hasEmail)
            {
                sb.Append(" (");
                if (hasPhone)
                {
                    sb.Append($"Telephone: {phone}");
                    if (hasEmail)
                    {
                        sb.Append("; ");
                    }
                }

                if (hasEmail)
                {
                    sb.Append($"Email: {email}");
                }
                sb.Append(")");
            }

            return sb.ToString();
        }

        private async Task CompleteSelectedReferralsAsync(int opportunityId, int itemId, string username)
        {
            var selectedOpportunityItemIds = _opportunityItemRepository.GetManyAsync(oi => oi.Opportunity.Id == opportunityId
                                                                                      && oi.Id == itemId
                                                                                      && oi.IsSaved
                                                                                      && oi.IsSelectedForReferral
                                                                                      && !oi.IsCompleted)
                .Select(oi => oi.Id).ToList();

            if (selectedOpportunityItemIds.Count > 0)
            {
                await SetOpportunityItemsAsCompletedAsync(selectedOpportunityItemIds, username);
            }
        }

        private async Task CompleteRemainingItemsAsync(int opportunityId, string username)
        {
            var remainingOpportunities = _opportunityItemRepository.GetManyAsync(oi => oi.Opportunity.Id == opportunityId
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
                    await SetOpportunityItemsAsCompletedAsync(provisionIds, username);
            }
        }

        private async Task SetOpportunityItemsAsCompletedAsync(IEnumerable<int> opportunityItemIds, string username)
        {
            var itemsToBeCompleted = opportunityItemIds.Select(id => new OpportunityItemIsSelectedWithUsernameForCompleteDto
            {
                Id = id,
                Username = username
            });

            var updates = _mapper.Map<List<OpportunityItem>>(itemsToBeCompleted);

            await _opportunityItemRepository.UpdateManyWithSpecifedColumnsOnlyAsync(updates,
                x => x.IsCompleted,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }

        private async Task<EmployerReferralDto> GetEmployerReferralsAsync(int opportunityId, IEnumerable<int> itemIds)
        {
            return await _opportunityRepository.GetEmployerReferralsAsync(opportunityId, itemIds);
        }

        private async Task<IList<OpportunityReferralDto>> GetOpportunityReferralsAsync(int opportunityId, IEnumerable<int> itemIds)
        {
            return await _opportunityRepository.GetProviderOpportunitiesAsync(opportunityId, itemIds);
        }

        private static string GetNumberOfPlacements(bool? placementsKnown, int? placements)
        {
            return placementsKnown.GetValueOrDefault()
                ? placements.ToString()
                : "at least 1";
        }

        private async Task SendEmailAsync(EmailTemplateName template, int? opportunityId,
            string toAddress, IDictionary<string, string> tokens, string createdBy)
        {
            if (!_configuration.SendEmailEnabled)
            {
                return;
            }

            await _emailService.SendEmailAsync(opportunityId, template.ToString(),
                toAddress,
                tokens, createdBy);
        }

        private async Task UpdateBackgroundProcessHistoryAsync(
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

            await _backgroundProcessHistoryRepository.UpdateWithSpecifedColumnsOnlyAsync(backgroundProcessHistory,
                history => history.RecordCount,
                history => history.Status,
                history => history.StatusMessage,
                history => history.ModifiedBy,
                history => history.ModifiedOn);
        }

        private Func<int, Task<BackgroundProcessHistory>> GetBackgroundProcessHistoryData => BackgroundProcessHistoryDataAsync;

        private async Task<BackgroundProcessHistory> BackgroundProcessHistoryDataAsync(int backgroundProcessHistoryId)
        {
            var backgroundProcessHistory =
                await _backgroundProcessHistoryRepository
                    .GetSingleOrDefaultAsync(p => p.Id == backgroundProcessHistoryId);

            if (backgroundProcessHistory == null ||
                backgroundProcessHistory.Status != BackgroundProcessHistoryStatus.Pending.ToString())
            {
                return null;
            }

            return backgroundProcessHistory;
        }
    }
}