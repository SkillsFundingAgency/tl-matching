using System;
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
            dto.Id = 0;
            var opportunityItem = _mapper.Map<OpportunityItem>(dto);

            var opportunityItemId = await _opportunityItemRepository.Create(opportunityItem);

            //TODO: Refactor this - put in to make up for loss of call to CreateProvisionGap
            //      The ProvisionGapMapper might not want to take OpportunityDto
            //      Should be able to do all of the below as part of the mapping from Dto above
            //      Make sure this functionality is covered by tests

            if (dto.OpportunityType == OpportunityType.ProvisionGap)
            {
                var provisionGap = _mapper.Map<ProvisionGap>(dto);
                //TODO: This should be opportunityItemId
                provisionGap.OpportunityItemId = opportunityItemId;

                await _provisionGapRepository.Create(provisionGap);
            }

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
(Expression<Func<OpportunityItem, object>>)(oi => oi.Opportunity), oi => oi.Opportunity.Employer);

            var dto = _mapper.Map<OpportunityItem, PlacementInformationSaveDto>(placementInformation);

            return dto;
        }

        public async Task<FindEmployerViewModel> GetOpportunityEmployerAsync(int opportunityId, int opportunityItemId)
        {
            return await _opportunityItemRepository.GetSingleOrDefault(
                o => o.Id == opportunityItemId,
                oi => new FindEmployerViewModel
                {
                    OpportunityItemId = opportunityItemId,
                    OpportunityId = opportunityId,
                    CompanyName = oi.Opportunity.Employer.CompanyName,
                    SelectedEmployerId = oi.Opportunity.EmployerId ?? 0
                });
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

            viewModel.Type = GetOpportunityBasketType(viewModel);

            return viewModel;
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

        public async Task<int> GetOpportunityItemCountAsync(int opportunityId)
        {
            return await _opportunityItemRepository.GetMany(o => o.OpportunityId == opportunityId && o.IsSaved == true).CountAsync();
        }


        public async Task UpdateReferrals(OpportunityDto dto)
        {
            // TODO Id should be OpportunityItemId
            var existingReferrals = _referralRepository.GetMany(r => r.OpportunityItemId == dto.Id)
                .ToList();

            //TODO: Use OpportunityItemDto here
            //var newReferrals = _mapper.Map<List<Referral>>(dto.Referral);
            var newReferrals = new List<Referral>();
            foreach (var nr in newReferrals)
                nr.OpportunityItemId = dto.Id; // TODO Id should be OpportunityItemId

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

        public async Task RemoveOpportunityItemASync(int opportunityItemId)
        {
            var referralItems = _referralRepository.GetMany(referral => referral.OpportunityItemId == opportunityItemId);
            await _referralRepository.DeleteMany(referralItems.ToList());
            await _opportunityItemRepository.Delete(opportunityItemId);
        }

        private static OpportunityBasketType GetOpportunityBasketType(OpportunityBasketViewModel viewModel)
        {
            if (viewModel.ReferralCount == 1 && viewModel.ProvisionGapCount == 0)
                return OpportunityBasketType.ReferralSingle;
            if (viewModel.ReferralCount == 0 && viewModel.ProvisionGapCount > 0)
                return OpportunityBasketType.ProvisionGapSingle;
            if (viewModel.ReferralCount > 0 && viewModel.ProvisionGapCount == 0)
                return OpportunityBasketType.ReferralMultipl;
            if (viewModel.ReferralCount == 1 && viewModel.ProvisionGapCount > 0)
                return OpportunityBasketType.SingleReferralAndProvisionGap;
            if (viewModel.ReferralCount > 1 && viewModel.ProvisionGapCount > 0)
                return OpportunityBasketType.MultipleReferralAndProvisionGap;

            return OpportunityBasketType.ReferralSingle;
        }
    }
}