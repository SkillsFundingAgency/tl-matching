using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public class OpportunityRepository : GenericRepository<Opportunity>, IOpportunityRepository
    {
        private readonly MatchingDbContext _dbContext;

        public OpportunityRepository(ILogger<OpportunityRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<IList<OpportunityReferralDto>> GetProviderOpportunities(int opportunityId)
        {
            // TODO FIX this
            throw new Exception("TODO: fix query below");
            /*
            return await (from op in _dbContext.Opportunity
                          join re in _dbContext.Referral on op.Id equals re.OpportunityId
                          join pv in _dbContext.ProviderVenue on re.ProviderVenueId equals pv.Id
                          join p in _dbContext.Provider on pv.ProviderId equals p.Id
                          join r in _dbContext.Route on op.RouteId equals r.Id
                          orderby re.DistanceFromEmployer
                          where op.Id == opportunityId
                                && p.IsCdfProvider
                                && p.IsEnabledForReferral
                                && pv.IsEnabledForReferral
                                && !pv.IsRemoved
                          select new OpportunityReferralDto
                          {
                              OpportunityId = op.Id,
                              ReferralId = re.Id,
                              ProviderName = p.Name,
                              ProviderPrimaryContact = p.PrimaryContact,
                              ProviderPrimaryContactEmail = p.PrimaryContactEmail,
                              ProviderSecondaryContactEmail = p.SecondaryContactEmail,
                              CompanyName = op.CompanyName,
                              EmployerContact = op.EmployerContact,
                              EmployerContactPhone = op.EmployerContactPhone,
                              EmployerContactEmail = op.EmployerContactEmail,
                              SearchRadius = op.SearchRadius,
                              Postcode = op.Postcode,
                              JobRole = op.JobRole,
                              ProviderVenuePostcode = pv.Postcode,
                              PlacementsKnown = op.PlacementsKnown,
                              Placements = op.Placements,
                              RouteName = r.Name,
                              CreatedBy = op.CreatedBy
                          }).ToListAsync();
            */
        }

        public async Task<EmployerReferralDto> GetEmployerReferrals(int opportunityId)
        {
            var data = await (from op in _dbContext.Opportunity
                              join emp in _dbContext.Employer on op.EmployerId equals emp.Id
                              where op.Id == opportunityId
                              select new EmployerReferralDto
                              {
                                  OpportunityId = op.Id,
                                  CompanyName = emp.CompanyName,
                                  EmployerContact = op.EmployerContact,
                                  EmployerContactEmail = op.EmployerContactEmail,
                                  EmployerContactPhone = op.EmployerContactPhone,
                                  Postcode = emp.Postcode,
                                  CreatedBy = op.CreatedBy,
                                  ProviderReferrals = (
                                      from oi in _dbContext.OpportunityItem
                                      join r in _dbContext.Referral on oi.Id equals r.OpportunityItemId
                                      join pv in _dbContext.ProviderVenue on r.ProviderVenueId equals pv.Id
                                      join p in _dbContext.Provider on pv.ProviderId equals p.Id
                                      where oi.OpportunityId == opportunityId && oi.IsSelectedForReferral && oi.IsSaved
                                      select new ProviderReferralDto
                                      {
                                          Placements = oi.Placements.Value,
                                          JobRole = oi.JobRole,
                                          ProviderName = p.Name,
                                          ProviderVenuePostCode = pv.Postcode,
                                          ProviderVenueTown = pv.Town
                                      })
                              }
                      ).SingleOrDefaultAsync();

            return data;
        }

        public async Task<OpportunityBasketViewModel> GetOpportunityBasket(int opportunityId)
        {
            var opportunityBasket = await (from o in _dbContext.Opportunity.Include(o => o.OpportunityItem).ThenInclude(oi => oi.ProvisionGap)
                                           join e in _dbContext.Employer
                                               on o.EmployerId equals e.Id
                                           where o.Id == opportunityId
                                           select new OpportunityBasketViewModel
                                           {
                                               OpportunityId = o.Id,
                                               CompanyName = e.CompanyName,
                                               CompanyNameAka = e.AlsoKnownAs,
                                               ProvisionGapItems = o.OpportunityItem
                                                   .Where(oi => IsValidBasketState(oi, OpportunityType.ProvisionGap))
                                                   .Select(oi => new BasketProvisionGapItemViewModel
                                                   {
                                                       OpportunityItemId = oi.Id,
                                                       JobRole = oi.JobRole,
                                                       Placements = oi.Placements,
                                                       PlacementsKnown = oi.PlacementsKnown,
                                                       Workplace = $"{oi.Town} {oi.Postcode}",
                                                       Reason = GetReasons(oi.ProvisionGap.First()),
                                                       OpportunityType = oi.OpportunityType
                                                   }).ToList(),
                                               ReferralItems = o.OpportunityItem
                                                   .Where(oi => IsValidBasketState(oi, OpportunityType.Referral))
                                                   .Select(oi => new BasketReferralItemViewModel
                                                   {
                                                       OpportunityItemId = oi.Id,
                                                       JobRole = oi.JobRole,
                                                       Workplace = $"{oi.Town} {oi.Postcode}",
                                                       PlacementsKnown = oi.PlacementsKnown,
                                                       Placements = oi.Placements,
                                                       Providers = oi.Referral.Count,
                                                       OpportunityType = oi.OpportunityType
                                                   }).ToList()
                                           }).SingleOrDefaultAsync();

            return opportunityBasket;
        }

        public int GetEmployerOpportunityCount(int opportunityId)
        {
            return _dbContext.OpportunityItem.Count(item =>
                item.OpportunityId == opportunityId && item.IsSaved && !item.IsCompleted);
        }

        private static bool IsValidBasketState(OpportunityItem oi, OpportunityType type)
        {
            return oi.OpportunityType == type.ToString()
                   && oi.IsSaved && !oi.IsCompleted;
        }

        private static string GetReasons(ProvisionGap provisionGap)
        {
            var reasons = new List<string>();
            if (provisionGap.HadBadExperience.HasValue && provisionGap.HadBadExperience.Value)
                reasons.Add("Employer had a bad experience with them");

            if (provisionGap.NoSuitableStudent.HasValue && provisionGap.NoSuitableStudent.Value)
                reasons.Add("Providers do not have students doing the right course");

            if (provisionGap.ProvidersTooFarAway.HasValue && provisionGap.ProvidersTooFarAway.Value)
                reasons.Add("Providers were too far away");

            if (reasons.Count == 0)
                reasons.Add("None available");

            return string.Join(", ", reasons);
        }
    }
}