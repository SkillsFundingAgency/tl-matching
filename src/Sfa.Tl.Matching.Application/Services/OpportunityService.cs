using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Sfa.Tl.Matching.Api.Clients.GoogleMaps;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.EqualityComparer;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Services
{
    public class OpportunityService : IOpportunityService
    {
        private readonly IMapper _mapper;
        private readonly IOpportunityRepository _opportunityRepository;
        private readonly IRepository<OpportunityItem> _opportunityItemRepository;
        private readonly IRepository<ProvisionGap> _provisionGapRepository;
        private readonly IRepository<Referral> _referralRepository;
        private readonly IGoogleMapApiClient _googleMapApiClient;
        private readonly IFileWriter<OpportunityReportDto> _opportunityPipelineReportWriter;
        private readonly IDateTimeProvider _dateTimeProvider;

        public OpportunityService(
            IMapper mapper,
            IOpportunityRepository opportunityRepository,
            IRepository<OpportunityItem> opportunityItemRepository,
            IRepository<ProvisionGap> provisionGapRepository,
            IRepository<Referral> referralRepository,
            IGoogleMapApiClient googleMapApiClient,
            IFileWriter<OpportunityReportDto> opportunityPipelineReportWriter,
            IDateTimeProvider dateTimeProvider)
        {
            _mapper = mapper;
            _opportunityRepository = opportunityRepository;
            _opportunityItemRepository = opportunityItemRepository;
            _provisionGapRepository = provisionGapRepository;
            _referralRepository = referralRepository;
            _googleMapApiClient = googleMapApiClient;
            _opportunityPipelineReportWriter = opportunityPipelineReportWriter;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> CreateOpportunityAsync(OpportunityDto dto)
        {
            dto.Id = 0;
            var opportunity = _mapper.Map<Opportunity>(dto);

            var opportunityId = await _opportunityRepository.CreateAsync(opportunity);

            return opportunityId;
        }

        public async Task<int> CreateOpportunityItemAsync(OpportunityItemDto dto)
        {
            dto.OpportunityItemId = 0;
            var opportunityItem = _mapper.Map<OpportunityItem>(dto);

            opportunityItem.Town = await _googleMapApiClient.GetAddressDetailsAsync(opportunityItem.Postcode);

            var opportunityItemId = await _opportunityItemRepository.CreateAsync(opportunityItem);

            return opportunityItemId;
        }

        public async Task<OpportunityDto> GetOpportunityAsync(int opportunityId)
        {
            var opportunity = await _opportunityRepository.GetSingleOrDefaultAsync(o => o.Id == opportunityId,
                navigationPropertyPath: o => o.Employer);

            var dto = _mapper.Map<OpportunityDto>(opportunity);

            return dto;
        }

        public async Task<OpportunityItemDto> GetOpportunityItemAsync(int opportunityItemId)
        {
            var opportunityItem = await _opportunityItemRepository.GetSingleOrDefaultAsync(o => o.Id == opportunityItemId);

            var dto = _mapper.Map<OpportunityItemDto>(opportunityItem);

            return dto;
        }

        public async Task<PlacementInformationSaveDto> GetPlacementInformationAsync(int opportunityItemId)
        {
            var placementInformation = await _opportunityItemRepository.GetSingleOrDefaultAsync(
                o => o.Id == opportunityItemId,
                oi => oi.ProvisionGap,
                (Expression<Func<OpportunityItem, object>>)(oi => oi.Opportunity), oi => oi.Opportunity.Employer);

            var dto = _mapper.Map<OpportunityItem, PlacementInformationSaveDto>(placementInformation);

            return dto;
        }

        public async Task<CheckAnswersViewModel> GetCheckAnswersAsync(int opportunityItemId)
        {
            var dto = await _opportunityItemRepository.GetSingleOrDefaultAsync(oi => oi.Id == opportunityItemId,
                oi => new CheckAnswersViewModel
                {
                    OpportunityItemId = oi.Id,
                    OpportunityId = oi.OpportunityId,
                    Postcode = oi.Postcode,
                    JobRole = oi.JobRole,
                    Placements = oi.Placements,
                    RouteId = oi.RouteId,
                    RouteName = oi.Route.Name,
                    CompanyName = oi.Opportunity.Employer.CompanyName,
                    CompanyNameAka = oi.Opportunity.Employer.AlsoKnownAs,
                    PlacementsKnown = oi.PlacementsKnown,
                    Providers = oi.Referral.Select(r => new ReferralsViewModel
                    {
                        ReferralId = r.Id,
                        Postcode = r.ProviderVenue.Postcode,
                        DistanceFromEmployer = r.DistanceFromEmployer,
                        Name = r.ProviderVenue.Provider.Name,
                        ProviderDisplayName = r.ProviderVenue.Provider.DisplayName,
                        ProviderVenueName = r.ProviderVenue.Name
                    }).ToList()
                });

            return dto;
        }

        public List<ReferralDto> GetReferrals(int opportunityItemId)
        {
            var referrals = _referralRepository.GetManyAsync(r => r.OpportunityItemId == opportunityItemId)
                .OrderBy(r => r.DistanceFromEmployer)
                .Select(r => new ReferralDto
                {
                    Name = r.ProviderVenue.Provider.Name,
                    Postcode = r.ProviderVenue.Postcode,
                    DistanceFromEmployer = r.DistanceFromEmployer,
                    ProviderVenueId = r.ProviderVenueId,
                    ProviderDisplayName = r.ProviderVenue.Provider.DisplayName,
                    ProviderVenueName = r.ProviderVenue.Name
                })
                .ToList();

            return referrals;
        }

        public async Task<OpportunityBasketViewModel> GetOpportunityBasketAsync(int opportunityId)
        {
            var viewModel = await _opportunityRepository.GetOpportunityBasketAsync(opportunityId);

            if (viewModel == null) return new OpportunityBasketViewModel();

            viewModel.Type = GetOpportunityBasketType(viewModel);

            return viewModel;
        }

        public async Task<ConfirmDeleteOpportunityItemViewModel> GetConfirmDeleteOpportunityItemAsync(int opportunityItemId)
        {
            return await _opportunityItemRepository.GetSingleOrDefaultAsync(
                 oi => oi.Id == opportunityItemId,
                 oi => new ConfirmDeleteOpportunityItemViewModel
                 {
                     OpportunityItemId = oi.Id,
                     OpportunityId = oi.OpportunityId,
                     CompanyName = oi.Opportunity.Employer.CompanyName,
                     CompanyNameAka = oi.Opportunity.Employer.AlsoKnownAs,
                     JobRole = oi.JobRole,
                     Postcode = oi.Postcode,
                     Placements = oi.Placements,
                     PlacementsKnown = oi.PlacementsKnown,
                     BasketItemCount = oi.Opportunity.OpportunityItem.Count(item => item.IsSaved && !item.IsCompleted)
                 });
        }

        public async Task<bool> IsReferralOpportunityItemAsync(int opportunityItemId)
        {
            return await _referralRepository.GetManyAsync(o => o.OpportunityItemId == opportunityItemId).AnyAsync();
        }

        public async Task<bool> IsNewReferralAsync(int opportunityItemId)
        {
            if (opportunityItemId == 0)
                return true;

            var isReferral = await IsReferralOpportunityItemAsync(opportunityItemId);

            return !isReferral;
        }

        public async Task<bool> IsNewProvisionGapAsync(int opportunityItemId)
        {
            if (opportunityItemId == 0)
                return true;

            var isReferral = await IsReferralOpportunityItemAsync(opportunityItemId);
            return isReferral;
        }

        public async Task<int> GetSavedOpportunityItemCountAsync(int opportunityId)
        {
            return await _opportunityItemRepository.CountAsync(o => o.OpportunityId == opportunityId && o.IsSaved);
        }

        public async Task<int> GetReferredOpportunityItemCountAsync(int opportunityId)
        {
            return await _opportunityItemRepository.CountAsync(o => o.OpportunityId == opportunityId
                                                               && o.IsSaved
                                                               && o.IsSelectedForReferral
                                                               && !o.IsCompleted);
        }

        public async Task UpdateReferralsAsync(OpportunityItemDto dto)
        {
            var existingReferrals = _referralRepository.GetManyAsync(r => r.OpportunityItemId == dto.OpportunityItemId)
                .ToList();

            var newReferrals = _mapper.Map<List<Referral>>(dto.Referral);
            foreach (var nr in newReferrals)
                nr.OpportunityItemId = dto.OpportunityItemId;

            var comparer = new ReferralEqualityComparer();
            var toBeAdded = newReferrals.Except(existingReferrals, comparer).ToList();
            var same = existingReferrals.Intersect(newReferrals, comparer).ToList();
            var toBeDeleted = existingReferrals.Except(same).ToList();

            Referral Find(Referral referral) => existingReferrals.First(r => r.Id == referral.Id);

            var deleteReferrals = toBeDeleted.Select(Find).ToList();
            await _referralRepository.DeleteManyAsync(deleteReferrals);

            await _referralRepository.CreateManyAsync(toBeAdded);

            var updateReferrals = same.Select(Find).ToList();
            await _referralRepository.UpdateManyAsync(updateReferrals);
        }

        public async Task UpdateOpportunityAsync<T>(T dto) where T : BaseOpportunityDto
        {
            var trackedEntity = await _opportunityRepository.GetSingleOrDefaultAsync(o => o.Id == dto.OpportunityId);

            trackedEntity = _mapper.Map(dto, trackedEntity);

            await _opportunityRepository.UpdateAsync(trackedEntity);
        }

        public async Task UpdateOpportunityItemAsync<T>(T dto) where T : BaseOpportunityDto
        {
            var trackedEntity = await _opportunityItemRepository.GetSingleOrDefaultAsync(o => o.Id == dto.OpportunityItemId);
            trackedEntity = _mapper.Map(dto, trackedEntity);

            await _opportunityItemRepository.UpdateAsync(trackedEntity);
        }

        public async Task UpdateProvisionGapAsync(PlacementInformationSaveDto dto)
        {
            var provisionGap = await _provisionGapRepository.GetSingleOrDefaultAsync(p => p.OpportunityItemId == dto.OpportunityItemId);

            if (provisionGap != null)
            {
                provisionGap = _mapper.Map(dto, provisionGap);
                await _provisionGapRepository.UpdateAsync(provisionGap);
            }
        }

        public async Task DeleteOpportunityItemAsync(int opportunityId, int opportunityItemId)
        {
            var referralItems = _referralRepository.GetManyAsync(referral => referral.OpportunityItemId == opportunityItemId);
            var provisionGaps = _provisionGapRepository.GetManyAsync(gap => gap.OpportunityItemId == opportunityItemId);

            await _referralRepository.DeleteManyAsync(referralItems.ToList());
            await _provisionGapRepository.DeleteManyAsync(provisionGaps.ToList());
            await _opportunityItemRepository.DeleteAsync(opportunityItemId);

            var opportunityItems = _opportunityItemRepository.GetManyAsync(item => item.OpportunityId == opportunityId);
            if (!opportunityItems.Any(item => item.IsSaved))
            {
                var items = opportunityItems
                    .Where(item => item.IsSaved == false)
                    .ToList();

                foreach (var opitem in items)
                {
                    await _referralRepository.DeleteManyAsync(_referralRepository.GetManyAsync(rf => rf.OpportunityItemId == opitem.Id).ToList());
                    await _provisionGapRepository.DeleteManyAsync(_provisionGapRepository.GetManyAsync(gap => gap.OpportunityItemId == opitem.Id).ToList());
                    await _opportunityItemRepository.DeleteAsync(opitem);
                }

                await _opportunityRepository.DeleteAsync(opportunityId);
            }
        }

        public async Task DeleteEmployerOpportunityItemAsync(int opportunityId)
        {
            var opportunity = await _opportunityRepository.GetFirstOrDefaultAsync(opp => opp.Id == opportunityId);

            if (opportunity == null) return;

            await DeleteOpportunityItemsAsync(opportunityId, true);
            await DeleteOpportunityItemsAsync(opportunityId, false);

            var completedOpportunityItems = _opportunityItemRepository.GetManyAsync(item => item.OpportunityId == opportunityId && item.IsSaved && item.IsCompleted);

            if (!completedOpportunityItems.Any())
            {
                await _opportunityRepository.DeleteAsync(opportunity);
            }
        }

        private async Task DeleteOpportunityItemsAsync(int opportunityId, bool isSaved)
        {
            var opportunityItems = _opportunityItemRepository.GetManyAsync(item => item.OpportunityId == opportunityId && item.IsSaved == isSaved && !item.IsCompleted);

            foreach (var opportunityItem in opportunityItems)
            {
                await _referralRepository.DeleteManyAsync(_referralRepository
                    .GetManyAsync(rf => rf.OpportunityItemId == opportunityItem.Id).ToList());

                await _provisionGapRepository.DeleteManyAsync(_provisionGapRepository
                    .GetManyAsync(gap => gap.OpportunityItemId == opportunityItem.Id).ToList());

                await _opportunityItemRepository.DeleteAsync(opportunityItem);
            }
        }

        public async Task ClearOpportunityItemsSelectedForReferralAsync(int opportunityId)
        {
            var opportunityItemsToBeReset = _opportunityItemRepository.GetManyAsync(
                        op => op.OpportunityId == opportunityId
                              && op.IsSelectedForReferral
                              && !op.IsCompleted)
                    .Select(op => new OpportunityItemIsSelectedForReferralDto
                    {
                        Id = op.Id,
                        IsSelectedForReferral = false
                    })
                    .ToList();

            var opportunityItemsToBeUpdated = _mapper.Map<List<OpportunityItem>>(opportunityItemsToBeReset);

            await _opportunityItemRepository.UpdateManyWithSpecifedColumnsOnlyAsync(opportunityItemsToBeUpdated,
                x => x.IsSelectedForReferral,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }

        public async Task ContinueWithOpportunitiesAsync(ContinueOpportunityViewModel viewModel)
        {
            var allProvisionGaps =
                viewModel.SelectedOpportunity.All(oi => oi.OpportunityType == OpportunityType.ProvisionGap.ToString());

            if (allProvisionGaps)
            {
                var ids = viewModel.SelectedOpportunity.Select(oi => oi.Id);

                await SetOpportunityItemsAsCompletedAsync(ids);
                return;
            }

            var referralIds = viewModel.SelectedOpportunity.Where(oi => oi.IsSelected && oi.OpportunityType == OpportunityType.Referral.ToString())
                .Select(oi => oi.Id).ToList();

            if (referralIds.Any())
                await SetOpportunityItemsAsReferralAsync(referralIds);
        }

        public async Task<string> GetCompanyNameWithAkaAsync(int? opportunityId)
        {
            var opportunity = await _opportunityRepository.GetSingleOrDefaultAsync(
                o => o.Id == opportunityId,
                (Expression<Func<Opportunity, object>>)(o => o.Employer));

            var companyName = opportunity?.Employer?.CompanyName;
            var companyNameAka = opportunity?.Employer?.AlsoKnownAs;

            return !string.IsNullOrWhiteSpace(companyNameAka) ?
                $"{companyName} ({companyNameAka})" : companyName;
        }

        public async Task<FileDownloadDto> GetOpportunitySpreadsheetDataAsync(int opportunityId)
        {
            var dto = await _opportunityRepository.GetPipelineOpportunitiesAsync(opportunityId);

            var fileContent = _opportunityPipelineReportWriter.WriteReport(dto);

            var companyName = dto.CompanyName.ToLetterOrDigit();
            var formattedDate = _dateTimeProvider.UtcNow().ToGmtStandardTime().ToString("ddMMMyyyy");

            return new FileDownloadDto
            {
                FileName = $"{companyName}_opportunities_{formattedDate}.xlsx",
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                FileContent = fileContent
            };
        }

        public async Task DeleteReferralAsync(int referralId)
        {
            var referral = await _referralRepository.GetFirstOrDefaultAsync(r => r.Id == referralId);

            if (referral == null) return;

            await _referralRepository.DeleteAsync(referral);
        }

        private async Task SetOpportunityItemsAsCompletedAsync(IEnumerable<int> opportunityItemIds)
        {
            var itemsToBeCompleted = opportunityItemIds.Select(id => new OpportunityItemIsSelectedForCompleteDto
            {
                Id = id
            });

            var updates = _mapper.Map<List<OpportunityItem>>(itemsToBeCompleted);

            await _opportunityItemRepository.UpdateManyWithSpecifedColumnsOnlyAsync(updates,
                x => x.IsCompleted,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }

        private async Task SetOpportunityItemsAsReferralAsync(IEnumerable<int> opportunityItemIds)
        {
            var itemsForReferral = opportunityItemIds.Select(id => new OpportunityItemIsSelectedForReferralDto
            {
                Id = id,
                IsSelectedForReferral = true
            });

            var updates = _mapper.Map<List<OpportunityItem>>(itemsForReferral);

            await _opportunityItemRepository.UpdateManyWithSpecifedColumnsOnlyAsync(updates,
                x => x.IsSelectedForReferral,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }

        private static OpportunityBasketType GetOpportunityBasketType(OpportunityBasketViewModel viewModel)
        {
            if (viewModel.ReferralCount == 1 && viewModel.ProvisionGapCount == 0)
                return OpportunityBasketType.ReferralSingle;
            if (viewModel.ReferralCount == 0 && viewModel.ProvisionGapCount > 0)
                return OpportunityBasketType.ProvisionGapSingle;
            if (viewModel.ReferralCount > 0 && viewModel.ProvisionGapCount == 0)
                return OpportunityBasketType.ReferralMultiple;
            if (viewModel.ReferralCount == 1 && viewModel.ProvisionGapCount > 0)
                return OpportunityBasketType.SingleReferralAndProvisionGap;
            if (viewModel.ReferralCount > 1 && viewModel.ProvisionGapCount > 0)
                return OpportunityBasketType.MultipleReferralAndProvisionGap;

            return OpportunityBasketType.ReferralSingle;
        }
    }
}