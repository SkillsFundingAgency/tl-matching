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
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ReferralEmailService : IReferralEmailService
    {
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEmailService _emailService;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository; private readonly IRepository<FunctionLog> _functionLogRepository;

        public ReferralEmailService(
                    IMapper mapper,
                    IDateTimeProvider dateTimeProvider,
                    IEmailService emailService,
                    IOpportunityRepository opportunityRepository,
                    IRepository<OpportunityItem> opportunityItemRepository,
                    IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository,
                    IRepository<FunctionLog> functionLogRepository)
        {
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _emailService = emailService;
            _opportunityRepository = opportunityRepository;
            _opportunityItemRepository = opportunityItemRepository;
            _backgroundProcessHistoryRepository = backgroundProcessHistoryRepository;
            _functionLogRepository = functionLogRepository;
        }

        public async Task SendEmployerReferralEmailAsync(int opportunityId, IEnumerable<int> itemIds, int backgroundProcessHistoryId, string username)
        {
            if (await GetBackgroundProcessHistoryDataAsync(backgroundProcessHistoryId) == null)
            {
                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = $"Background Processing History not found or not pending for id {backgroundProcessHistoryId} for employer referral emails",
                    FunctionName = nameof(ReferralEmailService),
                    RowNumber = -1
                });
                return;
            }

            try
            {
                var itemIdList = itemIds.ToList();
                var employerReferral = await _opportunityRepository.GetEmployerReferralsAsync(opportunityId, itemIdList);

                if (employerReferral == null ||
                    !employerReferral.WorkplaceDetails.Any() ||
                    !itemIdList.Any())
                {
                    await _functionLogRepository.CreateAsync(new FunctionLog
                    {
                        ErrorMessage = $"No referral found or no opportunity item ids when retrieving employer referral for opportunity id {opportunityId} with {itemIdList.Count} items",
                        FunctionName = nameof(ReferralEmailService),
                        RowNumber = -1
                    });
                }
                else
                {
                    var sb = new StringBuilder();
                    var tokens = new Dictionary<string, string>
                    {
                        {"employer_contact_name", employerReferral.PrimaryContact.ToTitleCase()},
                        {"employer_business_name", employerReferral.CompanyName.ToTitleCase()},
                        {"employer_contact_number", employerReferral.Phone},
                        {"employer_contact_email", employerReferral.Email}
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
                            sb.AppendLine(
                                $"* {count.ToOrdinalWords().ToTitleCase()} provider selected: {providerAndVenue.CustomisedProviderDisplayName}");
                            sb.Append("Primary contact: ");
                            sb.AppendLine(FormatContactDetails(providerAndVenue.PrimaryContact,
                                providerAndVenue.PrimaryContactPhone, providerAndVenue.PrimaryContactEmail));

                            if (!string.IsNullOrWhiteSpace(providerAndVenue.SecondaryContact))
                            {
                                sb.AppendLine(
                                    $"Secondary contact: {FormatContactDetails(providerAndVenue.SecondaryContact, providerAndVenue.SecondaryContactPhone, providerAndVenue.SecondaryContactEmail)}");
                            }

                            count++;
                        }

                        sb.AppendLine("");
                    }

                    tokens.Add("placements_list", sb.ToString());

                    await SendEmailAsync(EmailTemplateName.EmployerReferralV6, opportunityId, employerReferral.Email,
                        tokens, employerReferral.CreatedBy);
                }

                await UpdateBackgroundProcessHistoryAsync(backgroundProcessHistoryId,
                    employerReferral != null && itemIdList.Any() ? 1 : 0,
                    BackgroundProcessHistoryStatus.Complete, username);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending employer referral emails. {ex.Message} " +
                                   $"Opportunity id {opportunityId}";

                await UpdateBackgroundProcessHistoryAsync(backgroundProcessHistoryId,
                    1,
                    BackgroundProcessHistoryStatus.Error, username, errorMessage);
            }
        }

        public async Task SendProviderReferralEmailAsync(int opportunityId, IEnumerable<int> itemIds, int backgroundProcessHistoryId, string username)
        {
            if (await GetBackgroundProcessHistoryDataAsync(backgroundProcessHistoryId) == null)
            {
                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = $"Background Processing History not found or not pending for id {backgroundProcessHistoryId} for provider referral emails",
                    FunctionName = nameof(ReferralEmailService),
                    RowNumber = -1
                });
                return;
            }

            var itemIdList = itemIds.ToList();

            var referrals = await _opportunityRepository.GetProviderReferralsAsync(opportunityId, itemIdList);

            try
            {
                if (referrals == null || referrals.Count == 0)
                {
                    await _functionLogRepository.CreateAsync(new FunctionLog
                    {
                        ErrorMessage = $"No provider referrals found for opportunity id {opportunityId} with {itemIdList.Count} items",
                        FunctionName = nameof(ReferralEmailService),
                        RowNumber = -1
                    });
                }
                else
                {
                    //Group by opportunity item, then loop over that with referrals onside
                    var opportunityItemGroups = (
                        from p in referrals
                        orderby p.OpportunityItemId
                        group p by p.OpportunityItemId
                        into g
                        select new
                        {
                            OpportunityItemId = g.Key,
                            Referrals = g.Select(c => c).ToList()
                        }
                    ).ToList();

                    foreach (var opportunityItem in opportunityItemGroups)
                    {
                        foreach (var referral in opportunityItem.Referrals)
                        {
                            var placements = GetNumberOfPlacements(referral.PlacementsKnown, referral.Placements);

                            var tokens = new Dictionary<string, string>
                            {
                                {"contact_name", referral.ProviderPrimaryContact},
                                {"provider_name", referral.ProviderDisplayName},
                                {"route", referral.RouteName.ToLowerInvariant()},
                                {"venue_text", referral.VenueText},
                                {"search_radius", referral.DistanceFromEmployer},
                                {
                                    "job_role_list",
                                    string.IsNullOrEmpty(referral.JobRole) || referral.JobRole == "None given"
                                        ? $"* looking for students in courses related to {referral.RouteName.ToLowerInvariant()}"
                                        : $"* looking for this job role: {referral.JobRole}"
                                },
                                {"employer_business_name", referral.CompanyName.ToTitleCase()},
                                {"employer_contact_name", referral.EmployerContact.ToTitleCase()},
                                {"employer_contact_number", referral.EmployerContactPhone},
                                {"employer_contact_email", referral.EmployerContactEmail},
                                {"employer_town_postcode", $"{referral.Town} {referral.Postcode}"},
                                {"number_of_placements", placements}
                            };

                            const EmailTemplateName template = EmailTemplateName.ProviderReferralV6;
                            await SendEmailAsync(template, opportunityId, referral.ProviderPrimaryContactEmail, tokens,
                                referral.CreatedBy, referral.OpportunityItemId);

                            if (!string.IsNullOrWhiteSpace(referral.ProviderSecondaryContactEmail) &&
                                !string.IsNullOrWhiteSpace(referral.ProviderSecondaryContact))
                            {
                                tokens["contact_name"] = referral.ProviderSecondaryContact;
                                await SendEmailAsync(template, opportunityId, referral.ProviderSecondaryContactEmail,
                                    tokens,
                                    referral.CreatedBy, referral.OpportunityItemId);
                            }
                        }
                    }
                }

                await SetOpportunityItemsAsCompletedAsync(itemIdList, username);

                await CompleteRemainingProvisionGapsAsync(opportunityId, username);

                await UpdateBackgroundProcessHistoryAsync(backgroundProcessHistoryId,
                    referrals?.Count ?? 0,
                    BackgroundProcessHistoryStatus.Complete, username);
            }
            catch (Exception ex)
            {
                var errorMessage = $"Error sending provider referral emails. {ex.Message} " +
                                   $"Opportunity id {opportunityId}" +
                                   $"\r\nInner exception: {ex.InnerException?.Message}\r\n" +
                                   $"Stack trace: {ex.StackTrace}";

                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = errorMessage,
                    FunctionName = nameof(ReferralEmailService),
                    RowNumber = -1
                });

                await UpdateBackgroundProcessHistoryAsync(backgroundProcessHistoryId,
                    referrals?.Count ?? 0,
                    BackgroundProcessHistoryStatus.Error, username, errorMessage);
            }
        }

        private static string FormatContactDetails(string name, string phone, string email)
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
                sb.Append(')');
            }

            return sb.ToString();
        }

        private async Task CompleteRemainingProvisionGapsAsync(int opportunityId, string username)
        {
            var remainingOpportunities = _opportunityItemRepository.GetMany(oi => oi.Opportunity.Id == opportunityId
                                                                                  && oi.IsSaved
                                                                                  && !oi.IsCompleted);

            if (!remainingOpportunities.Any(oi => oi.OpportunityType == OpportunityType.Referral.ToString()))
            {
                var provisionGapItemIds = remainingOpportunities
                    .Where(oi => oi.OpportunityType == OpportunityType.ProvisionGap.ToString())
                    .Select(oi => oi.Id)
                    .ToList();
                if (provisionGapItemIds.Count > 0)
                {
                    await SetOpportunityItemsAsCompletedAsync(provisionGapItemIds, username);
                }
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

            await _opportunityItemRepository.UpdateManyWithSpecifiedColumnsOnlyAsync(updates,
                x => x.IsCompleted,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }

        private static string GetNumberOfPlacements(bool? placementsKnown, int? placements)
        {
            return placementsKnown.GetValueOrDefault()
                ? placements.ToString()
                : "at least 1";
        }

        private async Task SendEmailAsync(EmailTemplateName template, int? opportunityId, string toAddress, IDictionary<string, string> tokens, string createdBy, int? opportunityItemId = null)
        {
            await _emailService.SendEmailAsync(template.ToString(), toAddress, opportunityId, opportunityItemId, tokens, createdBy);
        }

        private async Task UpdateBackgroundProcessHistoryAsync(
            int backgroundProcessHistoryId,
            int providerCount,
            BackgroundProcessHistoryStatus historyStatus,
            string userName,
            string errorMessage = null)
        {
            var backgroundProcessHistory = await GetBackgroundProcessHistoryDataAsync(backgroundProcessHistoryId, false);
            if (backgroundProcessHistory == null)
            {
                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = $"UpdateBackgroundProcessHistoryAsync::Background Processing History not found for id {backgroundProcessHistoryId}",
                    FunctionName = nameof(ReferralEmailService),
                    RowNumber = -1
                });

                throw new InvalidOperationException($"Cannot update non-existent background processing history for id={backgroundProcessHistoryId}");
            }

            backgroundProcessHistory.RecordCount = providerCount;
            backgroundProcessHistory.Status = historyStatus.ToString();
            backgroundProcessHistory.StatusMessage = errorMessage;
            backgroundProcessHistory.ModifiedBy = userName;
            backgroundProcessHistory.ModifiedOn = _dateTimeProvider.UtcNow();

            await _backgroundProcessHistoryRepository.UpdateWithSpecifiedColumnsOnlyAsync(backgroundProcessHistory,
                history => history.RecordCount,
                history => history.Status,
                history => history.StatusMessage,
                history => history.ModifiedBy,
                history => history.ModifiedOn);
        }

        private async Task<BackgroundProcessHistory> GetBackgroundProcessHistoryDataAsync(int backgroundProcessHistoryId, bool getPendingOnly = true)
        {
            var backgroundProcessHistory = await _backgroundProcessHistoryRepository.GetSingleOrDefaultAsync(p => p.Id == backgroundProcessHistoryId);

            if (backgroundProcessHistory == null
                || (getPendingOnly && backgroundProcessHistory.Status != BackgroundProcessHistoryStatus.Pending.ToString()))
            {
                return null;
            }

            return backgroundProcessHistory;
        }
    }
}