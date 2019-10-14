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

        public async Task<IList<OpportunityReferralDto>> GetProviderOpportunitiesAsync(int opportunityId, IEnumerable<int> itemIds)
        {
            var data = await (from op in _dbContext.Opportunity
                              join oi in _dbContext.OpportunityItem on op.Id equals oi.OpportunityId
                              join emp in _dbContext.Employer on op.EmployerCrmId equals emp.CrmId
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
                                  DistanceFromEmployer = re.DistanceFromEmployer.ToString("0.0"),
                                  JourneyTimeByCar = re.JourneyTimeByCar,
                                  JourneyTimeByPublicTransport = re.JourneyTimeByPublicTransport,
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

        public async Task<EmployerReferralDto> GetEmployerReferralsAsync(int opportunityId, IEnumerable<int> itemIds)
        {
            var data = await (from op in _dbContext.Opportunity
                              join emp in _dbContext.Employer on op.EmployerCrmId equals emp.CrmId
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
                                                ProviderPrimaryContact = p.PrimaryContact,
                                                ProviderPrimaryContactEmail = p.PrimaryContactEmail,
                                                ProviderPrimaryContactPhone = p.PrimaryContactPhone,
                                                ProviderSecondaryContact = p.SecondaryContact,
                                                ProviderSecondaryContactEmail = p.SecondaryContactEmail,
                                                ProviderSecondaryContactPhone = p.SecondaryContactPhone,
                                                ProviderVenueTown = pv.Town,
                                                ProviderVenuePostcode = pv.Postcode
                                            })
                                      })
                              }
                      ).SingleOrDefaultAsync();

            return data;
        }

        public async Task<OpportunityBasketViewModel> GetOpportunityBasketAsync(int opportunityId)
        {
            var opportunityBasket = await (from o in _dbContext.Opportunity
                                           join e in _dbContext.Employer
                                               on o.EmployerCrmId equals e.CrmId
                                           where o.Id == opportunityId
                                           select new OpportunityBasketViewModel
                                           {
                                               OpportunityId = o.Id,
                                               CompanyName = e.CompanyName,
                                               CompanyNameAka = e.AlsoKnownAs,
                                               ProvisionGapItems = o.OpportunityItem
                                                   .Where(oi => oi.OpportunityType == OpportunityType.ProvisionGap.ToString() &&
                                                                oi.IsSaved && !oi.IsCompleted)
                                                   .Select(oi => new BasketProvisionGapItemViewModel
                                                   {
                                                       OpportunityItemId = oi.Id,
                                                       JobRole = oi.JobRole,
                                                       Placements = oi.Placements,
                                                       PlacementsKnown = oi.PlacementsKnown,
                                                       Workplace = $"{oi.Town} {oi.Postcode}",
                                                       Reason = GetReasons(oi.ProvisionGap),
                                                       OpportunityType = oi.OpportunityType
                                                   }).ToList(),
                                               ReferralItems = o.OpportunityItem
                                                   .Where(oi => oi.OpportunityType == OpportunityType.Referral.ToString() &&
                                                                oi.IsSaved && !oi.IsCompleted)
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
            var dto = await (from o in _dbContext.Opportunity
                             join e in _dbContext.Employer
                                 on o.EmployerCrmId equals e.CrmId
                             where o.Id == opportunityId
                             select new OpportunityReportDto
                             {
                                 CompanyName = e.CompanyName,
                                 ProvisionGapItems = o.OpportunityItem
                                     .Where(oi => oi.OpportunityType == OpportunityType.ProvisionGap.ToString() &&
                                                  oi.IsSaved && !oi.IsCompleted)
                                     .Select(oi => new ProvisionGapItemDto
                                     {
                                         JobRole = oi.JobRole,
                                         Placements = oi.Placements,
                                         PlacementsKnown = oi.PlacementsKnown,
                                         Workplace = $"{oi.Town} {oi.Postcode}",
                                         Reason = GetReasons(oi.ProvisionGap)
                                     }).ToList(),
                                 ReferralItems =
                                     (from oi in o.OpportunityItem
                                      join re in _dbContext.Referral on oi.Id equals re.OpportunityItemId
                                      join pv in _dbContext.ProviderVenue on re.ProviderVenueId equals pv.Id
                                      join p in _dbContext.Provider on pv.ProviderId equals p.Id
                                      where (oi.OpportunityType == OpportunityType.Referral.ToString() &&
                                                       oi.IsSaved && !oi.IsCompleted)
                                      select new ReferralItemDto
                                      {
                                          JobRole = oi.JobRole,
                                          Workplace = $"{oi.Town} {oi.Postcode}",
                                          DistanceFromEmployer = re.DistanceFromEmployer,
                                          JourneyTimeByCar = re.JourneyTimeByCar,
                                          JourneyTimeByPublicTransport = re.JourneyTimeByPublicTransport,
                                          PlacementsKnown = oi.PlacementsKnown,
                                          Placements = oi.Placements,
                                          Postcode = pv.Postcode,
                                          ProviderVenueName = pv.Name,
                                          ProviderDisplayName = p.DisplayName,
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
            return _dbContext.OpportunityItem.Count(item => item.OpportunityId == opportunityId && item.IsSaved && !item.IsCompleted);
        }

        public async Task<List<MatchingServiceOpportunityReport>> GetMatchingServiceOpportunityReportAsync()
        {
            return await _dbContext.MatchingServiceOpportunityReport
                .OrderBy(o => o.OpportunityItemId)
                .ThenByDescending(o => o.OpportunityType)
                .ThenByDescending(o => o.IsCompleted)
                .ThenByDescending(o => o.IsSaved)
                .ToListAsync();
        }

        public async Task<List<MatchingServiceProviderOpportunityReport>> GetMatchingServiceProviderOpportunityReportAsync()
        {
            return await _dbContext.MatchingServiceProviderOpportunityReport.ToListAsync();
        }

        public async Task<IList<EmployerFeedbackDto>> GetReferralsForEmployerFeedbackAsync(DateTime referralDate)
        {
            var dto = await (from o in _dbContext.Opportunity
                             join oi in _dbContext.OpportunityItem
                                 on o.Id equals oi.OpportunityId
                             where oi.IsCompleted
                                   && o.EmployerFeedbackSentOn == null
                                   && oi.ModifiedOn.HasValue
                                   && oi.ModifiedOn.Value <= referralDate
                                   && o.OpportunityItem.Count(x => x.IsCompleted) == 1
                                   && o.OpportunityItem.Count(x => x.IsSaved) == 1
                                   && oi.OpportunityType == OpportunityType.Referral.ToString()
                             select new EmployerFeedbackDto
                             {
                                 OpportunityId = o.Id,
                                 OpportunityItemId = oi.Id,
                                 EmployerContact = o.EmployerContact,
                                 EmployerContactEmail = o.EmployerContactEmail
                             }).ToListAsync();

            return dto;
        }

        public async Task<IList<ProviderFeedbackDto>> GetAllReferralsForProviderFeedbackAsync(DateTime referralDate)
        {
            var dto = await (from o in _dbContext.Opportunity
                             join oi in _dbContext.OpportunityItem on o.Id equals oi.OpportunityId
                             join e in _dbContext.Employer on o.EmployerCrmId equals e.CrmId
                             join re in _dbContext.Referral on oi.Id equals re.OpportunityItemId
                             join pv in _dbContext.ProviderVenue on re.ProviderVenueId equals pv.Id
                             join p in _dbContext.Provider on pv.ProviderId equals p.Id
                             where oi.IsCompleted
                                   && oi.IsSaved
                                   && oi.ModifiedOn.HasValue
                                   && oi.ModifiedOn.Value <= referralDate
                                   && oi.OpportunityType == OpportunityType.Referral.ToString()
                                   && p.ProviderFeedbackSentOn == null
                             select new ProviderFeedbackDto
                             {
                                 OpportunityId = o.Id,
                                 OpportunityItemId = oi.Id,
                                 ProviderId = p.Id,
                                 Companyname = e.CompanyName,
                                 ProviderPrimaryContactName = p.PrimaryContact,
                                 ProviderPrimaryContactEmail = p.PrimaryContactEmail,
                                 ProviderSecondaryContactName = p.SecondaryContact,
                                 ProviderSecondaryContactEmail = p.SecondaryContactEmail,
                                 ProviderFeedbackEmailSentOn = p.ProviderFeedbackSentOn
                             })
                            .GroupBy(feedbackDto => feedbackDto.ProviderId)
                            .Select(data => data.FirstOrDefault())
                            .Select(feedbackDto => feedbackDto)
                            .ToListAsync();

            return dto;
        }

        public async Task<FailedEmailBodyDto> GetFailedOpportunityEmailAsync(int opportunityId, string sentTo)
        {
            var dto = await (from o in _dbContext.Opportunity
                             join oi in _dbContext.OpportunityItem on o.Id equals oi.OpportunityId
                             join e in _dbContext.Employer on o.EmployerCrmId equals e.CrmId
                             join re in _dbContext.Referral on oi.Id equals re.OpportunityItemId
                             join pv in _dbContext.ProviderVenue on re.ProviderVenueId equals pv.Id
                             join p in _dbContext.Provider on pv.ProviderId equals p.Id
                             where o.Id == opportunityId &&
                                   (p.PrimaryContactEmail == sentTo ||
                                    p.SecondaryContactEmail == sentTo ||
                                    e.Email == sentTo)
                             select new FailedEmailBodyDto
                             {
                                 PrimaryContactEmail = p.PrimaryContactEmail,
                                 SecondaryContactEmail = p.SecondaryContactEmail,
                                 EmployerEmail = e.Email,
                                 ProviderDisplayName = p.DisplayName,
                                 ProviderVenuePostcode = pv.Postcode,
                                 ProviderVenueName = pv.Name,
                             })
                          .FirstOrDefaultAsync();

            return dto;
        }

        private static string GetReasons(IEnumerable<ProvisionGap> provisionGaps)
        {
            var reasons = new List<string>();

            var provisionGap = provisionGaps.First();
            if (provisionGap.HadBadExperience.HasValue && provisionGap.HadBadExperience.Value)
                reasons.Add("Employer had a bad experience with them");

            if (provisionGap.NoSuitableStudent.HasValue && provisionGap.NoSuitableStudent.Value)
                reasons.Add("Providers do not have students doing the right course");

            if (provisionGap.ProvidersTooFarAway.HasValue && provisionGap.ProvidersTooFarAway.Value)
                reasons.Add("Providers are too far away");

            if (reasons.Count == 0)
                reasons.Add("None available");

            return string.Join(", ", reasons);
        }
    }
}