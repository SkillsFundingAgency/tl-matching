﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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

        public OpportunityService(
            IMapper mapper,
            IOpportunityRepository opportunityRepository,
            IRepository<OpportunityItem> opportunityItemRepository,
            IRepository<ProvisionGap> provisionGapRepository,
            IRepository<Referral> referralRepository)
        {
            _mapper = mapper;
            _opportunityRepository = opportunityRepository;
            _opportunityItemRepository = opportunityItemRepository;
            _provisionGapRepository = provisionGapRepository;
            _referralRepository = referralRepository;
        }

        public async Task<int> CreateOpportunityAsync(OpportunityDto dto)
        {
            dto.Id = 0;
            var opportunity = _mapper.Map<Opportunity>(dto);

            var opportunityId = await _opportunityRepository.Create(opportunity);

            return opportunityId;
        }

        public async Task<int> CreateOpportunityItemAsync(OpportunityItemDto dto)
        {
            dto.OpportunityItemId = 0;
            var opportunityItem = _mapper.Map<OpportunityItem>(dto);

            var opportunityItemId = await _opportunityItemRepository.Create(opportunityItem);

            return opportunityItemId;
        }

        public async Task<OpportunityDto> GetOpportunity(int opportunityId)
        {
            var opportunity = await _opportunityRepository.GetSingleOrDefault(o => o.Id == opportunityId);

            var dto = _mapper.Map<OpportunityDto>(opportunity);

            return dto;
        }

        public async Task<OpportunityItemDto> GetOpportunityItem(int opportunityItemId)
        {
            var opportunityItem = await _opportunityItemRepository.GetSingleOrDefault(o => o.Id == opportunityItemId);

            var dto = _mapper.Map<OpportunityItemDto>(opportunityItem);

            return dto;
        }

        public async Task<PlacementInformationSaveDto> GetPlacementInformationAsync(int opportunityItemId)
        {
            var placementInformation = await _opportunityItemRepository.GetSingleOrDefault(
                o => o.Id == opportunityItemId,
                oi => oi.ProvisionGap,
(Expression<Func<OpportunityItem, object>>)(oi => oi.Opportunity), oi => oi.Opportunity.Employer);

            var dto = _mapper.Map<OpportunityItem, PlacementInformationSaveDto>(placementInformation);

            return dto;
        }

        public async Task<CheckAnswersViewModel> GetCheckAnswers(int opportunityItemId)
        {
            var dto = await _opportunityItemRepository.GetSingleOrDefault(o => o.Id == opportunityItemId,
                o => new CheckAnswersViewModel
                {
                    OpportunityItemId = o.Id,
                    OpportunityId = o.OpportunityId,
                    Postcode = o.Postcode,
                    JobRole = o.JobRole,
                    Placements = o.Placements,
                    PlacementsKnown = o.PlacementsKnown,
                    RouteId = o.RouteId,
                    SearchRadius = o.SearchRadius,
                    RouteName = o.Route.Name,
                    EmployerName = o.Opportunity.Employer.CompanyName,
                    CompanyNameAka = o.Opportunity.Employer.AlsoKnownAs,
                    Providers = o.Referral.Select(r => new ReferralsViewModel
                    {
                        Postcode = r.ProviderVenue.Postcode,
                        DistanceFromEmployer = r.DistanceFromEmployer,
                        Name = r.ProviderVenue.Provider.Name
                    }).ToList()
                });

            return dto;
        }

        public List<ReferralDto> GetReferrals(int opportunityItemId)
        {
            var referrals = _referralRepository.GetMany(r => r.OpportunityItemId == opportunityItemId)
                .OrderBy(r => r.DistanceFromEmployer)
                .Select(r => new ReferralDto
                {
                    Name = r.ProviderVenue.Provider.Name,
                    Postcode = r.ProviderVenue.Postcode,
                    DistanceFromEmployer = r.DistanceFromEmployer,
                    ProviderVenueId = r.ProviderVenueId
                })
                .ToList();

            return referrals;
        }

        public async Task<OpportunityBasketViewModel> GetOpportunityBasket(int opportunityId)
        {
            var viewModel = await _opportunityRepository.GetOpportunityBasket(opportunityId);

            if (viewModel == null) return new OpportunityBasketViewModel();

            viewModel.Type = GetOpportunityBasketType(viewModel);

            return viewModel;
        }

        public async Task<ConfirmDeleteOpportunityItemViewModel> GetConfirmDeleteOpportunityItemAsync(int opportunityItemId)
        {
            return await _opportunityItemRepository.GetSingleOrDefault(
                 oi => oi.Id == opportunityItemId,
                 oi => new ConfirmDeleteOpportunityItemViewModel
                 {
                     OpportunityItemId = oi.Id,
                     OpportunityId = oi.OpportunityId,
                     CompanyName = oi.Opportunity.Employer.CompanyName,
                     JobRole = oi.JobRole,
                     Postcode = oi.Postcode,
                     Placements = oi.Placements,
                     BasketItemCount = oi.Opportunity.OpportunityItem.Count(item => item.IsSaved)
                 });
        }

        public async Task<bool> IsReferralOpportunityItemAsync(int opportunityItemId)
        {
            return await _referralRepository.GetMany(o => o.OpportunityItemId == opportunityItemId).AnyAsync();
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
            return await _opportunityItemRepository.Count(o => o.OpportunityId == opportunityId && o.IsSaved);
        }

        public async Task<int> GetReferredOpportunityItemCountAsync(int opportunityId)
        {
            return await _opportunityItemRepository.Count(o => o.OpportunityId == opportunityId
                                                               && o.IsSaved
                                                               && o.IsSelectedForReferral
                                                               && !o.IsCompleted);
        }

        public async Task UpdateReferrals(OpportunityItemDto dto)
        {
            var existingReferrals = _referralRepository.GetMany(r => r.OpportunityItemId == dto.OpportunityItemId)
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
            await _referralRepository.DeleteMany(deleteReferrals);

            await _referralRepository.CreateMany(toBeAdded);

            var updateReferrals = same.Select(Find).ToList();
            await _referralRepository.UpdateMany(updateReferrals);
        }

        public async Task UpdateOpportunity<T>(T dto) where T : BaseOpportunityDto
        {
            var trackedEntity = await _opportunityRepository.GetSingleOrDefault(o => o.Id == dto.OpportunityId);

            trackedEntity = _mapper.Map(dto, trackedEntity);

            await _opportunityRepository.Update(trackedEntity);
        }

        public async Task UpdateOpportunityItemAsync<T>(T dto) where T : BaseOpportunityDto
        {
            var trackedEntity = await _opportunityItemRepository.GetSingleOrDefault(o => o.Id == dto.OpportunityItemId);
            trackedEntity = _mapper.Map(dto, trackedEntity);

            await _opportunityItemRepository.Update(trackedEntity);
        }

        public async Task UpdateProvisionGapAsync(PlacementInformationSaveDto dto)
        {
            var provisionGap = await _provisionGapRepository.GetSingleOrDefault(p => p.OpportunityItemId == dto.OpportunityItemId);

            if (provisionGap != null)
            {
                provisionGap = _mapper.Map(dto, provisionGap);
                await _provisionGapRepository.Update(provisionGap);
            }
        }

        public async Task DeleteOpportunityItemAsync(int opportunityId, int opportunityItemId)
        {
            var referralItems = _referralRepository.GetMany(referral => referral.OpportunityItemId == opportunityItemId);
            var provisionGaps = _provisionGapRepository.GetMany(gap => gap.OpportunityItemId == opportunityItemId);

            await _referralRepository.DeleteMany(referralItems.ToList());
            await _provisionGapRepository.DeleteMany(provisionGaps.ToList());
            await _opportunityItemRepository.Delete(opportunityItemId);

            var opportunityItems = _opportunityItemRepository.GetMany(item => item.OpportunityId == opportunityId);
            if (!opportunityItems.Any(item => item.IsSaved))
            {
                opportunityItems
                    .Where(item => item.IsSaved == false)
                    .ToList()
                    .ForEach(async opitem =>
                    {
                        await _referralRepository.DeleteMany(_referralRepository.GetMany(rf => rf.OpportunityItemId == opitem.Id).ToList());
                        await _provisionGapRepository.DeleteMany(_provisionGapRepository.GetMany(gap => gap.OpportunityItemId == opitem.Id).ToList());
                        await _opportunityItemRepository.Delete(opitem);
                    });

                await _opportunityRepository.Delete(opportunityId);
            }
        }

        public async Task ClearOpportunityItemsSelectedForReferralAsync(int opportunityId)
        {
            var opportunityItemsToBeReset = _opportunityItemRepository.GetMany(
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

            await _opportunityItemRepository.UpdateManyWithSpecifedColumnsOnly(opportunityItemsToBeUpdated,
                x => x.IsSelectedForReferral,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }

        public async Task ContinueWithOpportunities(ContinueOpportunityViewModel viewModel)
        {
            var allProvisionGaps =
                viewModel.SelectedOpportunity.All(oi => oi.OpportunityType == OpportunityType.ProvisionGap.ToString());

            if (allProvisionGaps)
            {
                var ids = viewModel.SelectedOpportunity.Select(oi => oi.Id);

                await SetOpportunityItemsAsCompleted(ids);
                return;
            }

            var referralIds = viewModel.SelectedOpportunity.Where(oi => oi.IsSelected && oi.OpportunityType == OpportunityType.Referral.ToString())
                .Select(oi => oi.Id).ToList();

            if (referralIds.Any())
                await SetOpportunityItemsAsReferral(referralIds);
        }

        public async Task ConfirmOpportunities(int opportunityId)
        {
            var basketItems = _opportunityItemRepository.GetMany(oi => oi.Opportunity.Id == opportunityId
                                                                          && oi.IsSaved
                                                                          && !oi.IsCompleted);

            var referralItems = basketItems.Where(oi => oi.OpportunityType == OpportunityType.Referral.ToString()).ToList();
            var provisionItems = basketItems.Where(oi => oi.OpportunityType == OpportunityType.ProvisionGap.ToString()).ToList();
            var remainingItems = basketItems.Where(oi => !oi.IsSelectedForReferral).ToList();

            IEnumerable<int> opportunityItemIdsToComplete;

            var isMultipleBasket = referralItems.Count > 0 && provisionItems.Count > 0;
            if (isMultipleBasket && remainingItems.Count == 0)
            {
                opportunityItemIdsToComplete = basketItems.Select(oi => oi.Id);
                await SetOpportunityItemsAsCompleted(opportunityItemIdsToComplete);
                return;
            }

            var selectedReferralItems = referralItems.Where(oi => oi.IsSelectedForReferral).ToList();

            var allReferralsSelected = referralItems.Count == selectedReferralItems.Count;
            opportunityItemIdsToComplete = allReferralsSelected ?
                referralItems.Select(oi => oi.Id) :
                selectedReferralItems.Select(oi => oi.Id);

            var opportunityItemIdsToCompleteList = opportunityItemIdsToComplete.ToList();
            if (opportunityItemIdsToCompleteList.Any())
                await SetOpportunityItemsAsCompleted(opportunityItemIdsToCompleteList);
        }

        private async Task SetOpportunityItemsAsCompleted(IEnumerable<int> opportunityItemIds)
        {
            var itemsToBeCompleted = opportunityItemIds.Select(id => new OpportunityItemIsSelectedForCompleteDto
            {
                Id = id,
                IsCompleted = true
            });

            var updates = _mapper.Map<List<OpportunityItem>>(itemsToBeCompleted);

            await _opportunityItemRepository.UpdateManyWithSpecifedColumnsOnly(updates,
                x => x.IsCompleted,
                x => x.ModifiedOn,
                x => x.ModifiedBy);
        }

        private async Task SetOpportunityItemsAsReferral(IEnumerable<int> opportunityItemIds)
        {
            var itemsForReferral = opportunityItemIds.Select(id => new OpportunityItemIsSelectedForReferralDto
            {
                Id = id,
                IsSelectedForReferral = true
            });

            var updates = _mapper.Map<List<OpportunityItem>>(itemsForReferral);

            await _opportunityItemRepository.UpdateManyWithSpecifedColumnsOnly(updates,
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