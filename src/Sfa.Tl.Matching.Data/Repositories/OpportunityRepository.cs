﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.Extensions;
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

        public async Task<IList<OpportunityReferralDto>> GetProviderOpportunities(int opportunityId, IEnumerable<int> itemIds)
        {
            var data = await (from op in _dbContext.Opportunity
                              join oi in _dbContext.OpportunityItem on op.Id equals oi.OpportunityId
                              join emp in _dbContext.Employer on op.EmployerId equals emp.Id
                              join re in _dbContext.Referral on oi.Id equals re.OpportunityItemId
                              join pv in _dbContext.ProviderVenue on re.ProviderVenueId equals pv.Id
                              join p in _dbContext.Provider on pv.ProviderId equals p.Id
                              join r in _dbContext.Route on oi.RouteId equals r.Id
                              orderby re.DistanceFromEmployer
                              where op.Id == opportunityId
                                    && itemIds.Contains(oi.Id)
                                    && oi.IsSelectedForReferral
                                    && oi.IsSaved
                                    && p.IsCdfProvider
                                    && p.IsEnabledForReferral
                                    && pv.IsEnabledForReferral
                                    && !pv.IsRemoved
                              select new OpportunityReferralDto
                              {
                                  OpportunityId = op.Id,
                                  OpportunityItemId = oi.Id,
                                  ReferralId = re.Id,
                                  ProviderName = p.Name,
                                  ProviderDisplayName = p.DisplayName,
                                  ProviderPrimaryContact = p.PrimaryContact,
                                  ProviderPrimaryContactEmail = p.PrimaryContactEmail,
                                  ProviderSecondaryContact = p.SecondaryContact,
                                  ProviderSecondaryContactEmail = p.SecondaryContactEmail,
                                  CompanyName = emp.CompanyName,
                                  EmployerContact = op.EmployerContact,
                                  EmployerContactPhone = op.EmployerContactPhone,
                                  EmployerContactEmail = op.EmployerContactEmail,
                                  SearchRadius = oi.SearchRadius,
                                  DistanceFromEmployer = re.DistanceFromEmployer.ToString("0.0"),
                                  Postcode = oi.Postcode,
                                  Town = oi.Town,
                                  JobRole = oi.JobRole,
                                  ProviderVenueName = pv.Name,
                                  ProviderVenuePostcode = pv.Postcode,
                                  ProviderVenueTown = pv.Town,
                                  PlacementsKnown = oi.PlacementsKnown,
                                  Placements = oi.Placements,
                                  RouteName = r.Name,
                                  CreatedBy = op.CreatedBy
                              }).ToListAsync();

            return data;
        }

        public async Task<EmployerReferralDto> GetEmployerReferrals(int opportunityId, IEnumerable<int> itemIds)
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
                                  WorkplaceDetails = (
                                      from oi in _dbContext.OpportunityItem
                                      where oi.OpportunityId == opportunityId
                                            && itemIds.Contains(oi.Id)
                                            && oi.IsSelectedForReferral
                                            && oi.IsSaved
                                      select new WorkplaceDto
                                      {
                                          PlacementsKnown = oi.PlacementsKnown,
                                          Placements = oi.Placements.Value,
                                          JobRole = oi.JobRole,
                                          WorkplacePostcode = oi.Postcode,
                                          WorkplaceTown = oi.Town,
                                          ProviderAndVenueDetails = (
                                            from r in _dbContext.Referral
                                            join pv in _dbContext.ProviderVenue on r.ProviderVenueId equals pv.Id
                                            join p in _dbContext.Provider on pv.ProviderId equals p.Id
                                            where r.OpportunityItemId == oi.Id
                                                      && !pv.IsRemoved
                                                      && pv.IsEnabledForReferral
                                                      && p.IsCdfProvider
                                                      && p.IsEnabledForReferral
                                            select new ProviderReferralDto
                                            {
                                                ProviderName = p.Name,
                                                ProviderDisplayName = p.DisplayName,
                                                ProviderVenueName = pv.Name,
                                                ProviderVenueTown = pv.Town,
                                                ProviderVenuePostCode = pv.Postcode
                                            })
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
                                                       ProviderNames = (from r in oi.Referral
                                                                        join pv in _dbContext.ProviderVenue on r.ProviderVenueId equals pv.Id
                                                                        join p in _dbContext.Provider on pv.ProviderId equals p.Id
                                                                        select new ProviderNameViewModel
                                                                        {
                                                                            DisplayName = p.DisplayName,
                                                                            VenueName = pv.Name,
                                                                            Postcode = pv.Postcode
                                                                        }).ToList(),
                                                       OpportunityType = oi.OpportunityType
                                                   }).ToList()
                                           }).SingleOrDefaultAsync();

            return opportunityBasket;
        }

        public async Task<OpportunityReportDto> GetPipelineOpportunitiesAsync(int opportunityId)
        {
            var dto = await (from o in _dbContext.Opportunity.Include(o => o.OpportunityItem).ThenInclude(oi => oi.ProvisionGap)
                             join e in _dbContext.Employer
                                 on o.EmployerId equals e.Id
                             where o.Id == opportunityId
                             select new OpportunityReportDto
                             {
                                 CompanyName = e.CompanyName,
                                 ProvisionGapItems = o.OpportunityItem
                                     .Where(oi => IsValidBasketState(oi, OpportunityType.ProvisionGap))
                                     .Select(oi => new ProvisionGapItemDto
                                     {
                                         JobRole = oi.JobRole,
                                         Placements = oi.Placements,
                                         PlacementsKnown = oi.PlacementsKnown,
                                         Workplace = $"{oi.Town} {oi.Postcode}",
                                         Reason = GetReasons(oi.ProvisionGap.First()),
                                     }).ToList(),
                                 ReferralItems =
                                     (from oi in o.OpportunityItem
                                      join re in _dbContext.Referral on oi.Id equals re.OpportunityItemId
                                      join pv in _dbContext.ProviderVenue on re.ProviderVenueId equals pv.Id
                                      join p in _dbContext.Provider on pv.ProviderId equals p.Id
                                      where IsValidBasketState(oi, OpportunityType.Referral)
                                      select new ReferralItemDto
                                      {
                                          JobRole = oi.JobRole,
                                          Workplace = $"{oi.Town} {oi.Postcode}",
                                          DistanceFromEmployer = re.DistanceFromEmployer,
                                          PlacementsKnown = oi.PlacementsKnown,
                                          Placements = oi.Placements,
                                          ProviderName = ProviderDisplayExtensions.GetDisplayText(pv.Name, pv.Postcode, p.DisplayName),
                                          ProviderVenueTownAndPostcode = $"{pv.Town} {pv.Postcode}",
                                          PrimaryContact = p.PrimaryContact,
                                          PrimaryContactEmail = p.PrimaryContactEmail,
                                          PrimaryContactPhone = p.PrimaryContactPhone,
                                          SecondaryContact = p.SecondaryContact,
                                          SecondaryContactEmail = p.SecondaryContactEmail,
                                          SecondaryContactPhone = p.SecondaryContactPhone
                                      }).ToList()
                             }).SingleOrDefaultAsync();

            return dto;
        }

        public int GetEmployerOpportunityCount(int opportunityId)
        {
            return _dbContext.OpportunityItem.Count(item =>
                item.OpportunityId == opportunityId && item.IsSaved && !item.IsCompleted);
        }

        public async Task<IList<EmployerFeedbackDto>> GetReferralsForEmployerFeedbackAsync(DateTime referralDate)
        {
            // Employers sent a single referral and have not been sent the feedback email
            //TODO: Create a new type for the dto here?
            /*
             OpportunityId, 
             OpportunityItemId, 
             EmployerContact
             EmployerContactEmail
            where
            oi.ModifiedDate >= referralDate
            !EmployerFeedbackSent
             */

            return new List<EmployerFeedbackDto>();
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