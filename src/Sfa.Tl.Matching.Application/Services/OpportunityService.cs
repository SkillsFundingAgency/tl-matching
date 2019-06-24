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
        private readonly IRepository<Opportunity> _opportunityRepository;
        private readonly IRepository<ProvisionGap> _provisionGapRepository;
        private readonly IRepository<Referral> _referralRepository;

        public OpportunityService(
            IMapper mapper,
            IRepository<Opportunity> opportunityRepository,
            IRepository<ProvisionGap> provisionGapRepository,
            IRepository<Referral> referralRepository)
        {
            _mapper = mapper;
            _opportunityRepository = opportunityRepository;
            _provisionGapRepository = provisionGapRepository;
            _referralRepository = referralRepository;
        }

        public async Task<int> CreateOpportunity(OpportunityDto dto)
        {
            dto.Id = 0;
            var opportunity = _mapper.Map<Opportunity>(dto);

            var opportunityId = await _opportunityRepository.Create(opportunity);

            //TODO: Refactor this - put in to make up for loss of call to CreateProvisionGap
            //      The ProvisionGapMapper might not want to take OpportunityDto
            //      Should be able to do all of the below as part of the mapping from Dto above
            //      Make sure this functionality is covered by tests
            if (dto.OpportunityType == OpportunityType.ProvisionGap)
            {
                var provisionGap = _mapper.Map<ProvisionGap>(dto);
                //TODO: This should be opportunityItemId
                provisionGap.OpportunityItemId = opportunityId;

                await _provisionGapRepository.Create(provisionGap);
            }

            return opportunityId;
        }

        public async Task UpdateReferrals(OpportunityDto dto)
        {
            var existingReferrals = _referralRepository.GetMany(r => r.OpportunityId == dto.Id)
                .ToList();

            var newReferrals = _mapper.Map<List<Referral>>(dto.Referral);
            foreach (var nr in newReferrals)
                nr.OpportunityId = dto.Id;

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

        public async Task<OpportunityDto> GetOpportunity(int id)
        {
            var opportunity = await _opportunityRepository.GetSingleOrDefault(o => o.Id == id);

            var dto = _mapper.Map<OpportunityDto>(opportunity);

            return dto;
        }

        public async Task<OpportunityBasketViewModel> GetOpportunityBasket(int id)
        {
            var opportunity = await _opportunityRepository.GetSingleOrDefault(o => o.Id == id);

            var viewModel = _mapper.Map<OpportunityBasketViewModel>(opportunity);

            // TODO Get proper data when DB is done
            viewModel.ReferralItems = new List<BasketReferralItemViewModel>
            {
                new BasketReferralItemViewModel
                {
                    OpportunityItemId = 1,
                    StudentsWanted = "StudentsWanted",
                    JobRole = "JobRole",
                    Providers = 1,
                    Workplace = "Workplace"
                },
                new BasketReferralItemViewModel
                {
                    OpportunityItemId = 2,
                    StudentsWanted = "StudentsWanted1",
                    JobRole = "JobRole1",
                    Providers = 2,
                    Workplace = "Workplace1"
                }
            };

            // TODO Add back in when DB changes are done if (opportunity.ProvisionGapCount > 0)
            {
                viewModel.ProvisionGapItems = new List<BasketProvisionGapItemViewModel>
                {
                    new BasketProvisionGapItemViewModel
                    {
                        OpportunityItemId = 3,
                        StudentsWanted = "StudentsWanted",
                        JobRole = "JobRole",
                        Workplace = "Workplace",
                        Reason = "Reason1"
                    },
                    new BasketProvisionGapItemViewModel
                    {
                        OpportunityItemId = 4,
                        StudentsWanted = "StudentsWanted2",
                        JobRole = "JobRole2",
                        Workplace = "Workplace2",
                        Reason = "Reason2"
                    }
                };
            }

            return viewModel;
        }

        public OpportunityDto GetLatestCompletedOpportunity(int employerId)
        {
            var latestOpportunity = _opportunityRepository.GetMany(o => o.EmployerId == employerId)
                .Where(FilterValidOpportunities())
                .OrderByDescending(o => o.CreatedOn)
                .Take(1).SingleOrDefault();

            if (latestOpportunity == null)
                return null;

            latestOpportunity.Referral?.Clear();
            latestOpportunity.ProvisionGap?.Clear();

            var dto = _mapper.Map<OpportunityDto>(latestOpportunity);

            return dto;
        }

        public async Task<bool> IsReferralOpportunity(int id)
        {
            return await _opportunityRepository.GetMany(o => o.Id == id && o.Referral.Any()).AnyAsync();
        }

        public async Task<PlacementInformationSaveDto> GetPlacementInformationSave(int id)
        {
            var placementInformation = await _opportunityRepository.GetSingleOrDefault(e => e.Id == id,
                opp => opp.Route,
                //TODO: Referral only included so we can map OpportunityType,
                //      probably won't need it when data is refactored
                opp => opp.Referral);

            var dto = _mapper.Map<Opportunity, PlacementInformationSaveDto>(placementInformation);

            return dto;
        }

        public async Task<CheckAnswersDto> GetCheckAnswers(int id)
        {
            var checkAnswers = await _opportunityRepository.GetSingleOrDefault(e => e.Id == id,
                opp => opp.Route);

            var dto = _mapper.Map<Opportunity, CheckAnswersDto>(checkAnswers);

            return dto;
        }

        public async Task UpdateOpportunity<T>(T dto) where T : BaseOpportunityUpdateDto
        {
            var trackedEntity = await _opportunityRepository.GetSingleOrDefault(o => o.Id == dto.OpportunityId);
            trackedEntity = _mapper.Map(dto, trackedEntity);

            await _opportunityRepository.Update(trackedEntity);
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

        public async Task<bool> IsNewReferral(int opportunityId)
        {
            if (opportunityId == 0)
                return true;

            var isReferral = await IsReferralOpportunity(opportunityId);
            return !isReferral;
        }

        public async Task<bool> IsNewProvisionGap(int opportunityId)
        {
            if (opportunityId == 0)
                return true;

            var isReferral = await IsReferralOpportunity(opportunityId);
            return isReferral;
        }

        public async Task<int> GetOpportunityItemCountAsync(int opportunityId)
        {
            var opportunity = await _opportunityRepository.GetSingleOrDefault(o => o.Id == opportunityId);
            //TODO: Return the actual count
            //return opportunity.Count;
            return 1;
        }

        private static Expression<Func<Opportunity, bool>> FilterValidOpportunities()
        {
            return o => (o.ProvisionGap != null && o.ProvisionGap.Count > 0) ||
                        (o.Referral != null && o.Referral.Count > 0 && o.ConfirmationSelected.HasValue && o.ConfirmationSelected.Value);
        }
    }
}