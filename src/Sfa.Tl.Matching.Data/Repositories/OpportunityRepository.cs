using System;
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
        public OpportunityRepository(ILogger<OpportunityRepository> logger, MatchingDbContext dbContext) : base(logger, dbContext)
        {
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

        public async Task<EmployerReferralDto> GetEmployerReferralsAsync(int opportunityId, IEnumerable<int> itemIds)
        {
            var data = await (from op in _dbContext.Opportunity
                              join emp in _dbContext.Employer on op.EmployerCrmId equals emp.CrmId
                              where op.Id == opportunityId
                              select new EmployerReferralDto
                              {
                                  OpportunityId = op.Id,
                                  CompanyName = emp.CompanyName,
                                  PrimaryContact = op.EmployerContact,
                                  Email = op.EmployerContactEmail,
                                  Phone = op.EmployerContactPhone,
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
                                                DisplayName = p.DisplayName,
                                                ProviderVenueName = pv.Name,
                                                PrimaryContact = p.PrimaryContact,
                                                PrimaryContactEmail = p.PrimaryContactEmail,
                                                PrimaryContactPhone = p.PrimaryContactPhone,
                                                SecondaryContact = p.SecondaryContact,
                                                SecondaryContactEmail = p.SecondaryContactEmail,
                                                SecondaryContactPhone = p.SecondaryContactPhone,
                                                Town = pv.Town,
                                                Postcode = pv.Postcode
                                            })
                                      })
                              }
                      ).SingleOrDefaultAsync();

            return data;
        }

        public async Task<OpportunityBasketViewModel> GetOpportunityBasketAsync(int opportunityId)
        {
            var opportunityBasketItems = await GetOpportunityBasketDataAsync(opportunityId);

            var opportunity = opportunityBasketItems.First();
            var opportunityTypes = opportunityBasketItems.GroupBy(grp => grp.OpportunityType).ToList();
            var result = new OpportunityBasketViewModel
            {
                OpportunityId = opportunity.OpportunityId,
                CompanyName = opportunity.CompanyName,
                CompanyNameAka = opportunity.CompanyNameAka,
                ProvisionGapItems = opportunityTypes.FirstOrDefault(grp => grp.Key == "ProvisionGap")
                                 ?.Select(oi => new BasketProvisionGapItemViewModel
                                 {
                                     OpportunityItemId = oi.OpportunityItemId,
                                     JobRole = oi.JobRole,
                                     Placements = oi.Placements,
                                     PlacementsKnown = oi.PlacementsKnown,
                                     Workplace = oi.Workplace,
                                     Reason = GetReasons(oi.HadBadExperience, oi.NoSuitableStudent, oi.ProvidersTooFarAway),
                                     OpportunityType = oi.OpportunityType
                                 }).ToList(),
                ReferralItems = opportunityTypes.FirstOrDefault(grp => grp.Key == "Referral")
                                ?.GroupBy(oi => new
                                {
                                    oi.OpportunityItemId,
                                    oi.JobRole,
                                    oi.Workplace,
                                    oi.PlacementsKnown,
                                    oi.Placements,
                                    oi.OpportunityType
                                }).Select(grp => new BasketReferralItemViewModel
                                {
                                    Workplace = grp.Key.Workplace,
                                    OpportunityItemId = grp.Key.OpportunityItemId,
                                    OpportunityType = grp.Key.OpportunityType,
                                    JobRole = grp.Key.JobRole,
                                    Placements = grp.Key.Placements,
                                    PlacementsKnown = grp.Key.PlacementsKnown,
                                    ProviderNames = grp.Select(g => new ProviderNameViewModel
                                    {
                                        DisplayName = g.DisplayName,
                                        Postcode = g.Postcode,
                                        VenueName = g.VenueName
                                    }).ToList()
                                }).ToList()
            };

            return result;
        }

        private async Task<IList<OpportunityBasketItem>> GetOpportunityBasketDataAsync(int opportunityId)
        {
            var opportunityBasketItems = await
                (from o in _dbContext.Opportunity
                 join oi in _dbContext.OpportunityItem
                     on o.Id equals oi.OpportunityId
                 join e in _dbContext.Employer
                     on o.EmployerCrmId equals e.CrmId
                 join provisionGap in _dbContext.ProvisionGap
                     on oi.Id equals provisionGap.OpportunityItemId
                     into pgItems
                 from pg in pgItems.DefaultIfEmpty()
                 join referral in _dbContext.Referral
                     on oi.Id equals referral.OpportunityItemId
                     into rItems
                 from r in rItems.DefaultIfEmpty()
                 join venue in _dbContext.ProviderVenue
                     on r.ProviderVenueId equals venue.Id
                     into pvItems
                 from pv in pvItems.DefaultIfEmpty()
                 join provider in _dbContext.Provider
                     on pv.ProviderId equals provider.Id
                     into pItems
                 from p in pItems.DefaultIfEmpty()
                 where o.Id == opportunityId
                       && oi.IsSaved
                       && !oi.IsCompleted
                 select new OpportunityBasketItem
                 {
                     OpportunityId = o.Id,
                     CompanyName = e.CompanyName,
                     CompanyNameAka = e.AlsoKnownAs,
                     OpportunityItemId = oi.Id,
                     JobRole = oi.JobRole,
                     Placements = oi.Placements,
                     PlacementsKnown = oi.PlacementsKnown,
                     Workplace = oi.Town + ' ' + oi.Postcode,
                     OpportunityType = oi.OpportunityType,
                     HadBadExperience = pg.HadBadExperience,
                     NoSuitableStudent = pg.NoSuitableStudent,
                     ProvidersTooFarAway = pg.ProvidersTooFarAway,
                     DisplayName = p.DisplayName,
                     VenueName = pv.Name,
                     Postcode = pv.Postcode
                 }).ToListAsync();

            return opportunityBasketItems;
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

        public async Task<IList<MatchingServiceOpportunityReport>> GetMatchingServiceOpportunityReportAsync()
        {
            return await _dbContext.MatchingServiceOpportunityReport
                .OrderBy(o => o.OpportunityItemId)
                .ThenByDescending(o => o.OpportunityType)
                .ThenByDescending(o => o.PipelineOpportunity)
                .ToListAsync();
        }

        public async Task<IList<MatchingServiceProviderOpportunityReport>> GetMatchingServiceProviderOpportunityReportAsync()
        {
            return await _dbContext.MatchingServiceProviderOpportunityReport.ToListAsync();
        }

        public async Task<IList<MatchingServiceProviderEmployerReport>> GetMatchingServiceProviderEmployerReportAsync()
        {
            return await _dbContext.MatchingServiceProviderEmployerReport.ToListAsync();
        }

        public async Task<EmailBodyDto> GetEmailDeliveryStatusForEmployerAsync(int opportunityId, string sentTo)
        {
            var dto = await (from o in _dbContext.Opportunity
                             where o.Id == opportunityId
                                   && o.EmployerContactEmail == sentTo
                             select new EmailBodyDto
                             {
                                 EmployerEmail = o.EmployerContactEmail,
                             })
                .FirstOrDefaultAsync();

            return dto;
        }

        public async Task<EmailBodyDto> GetEmailDeliveryStatusForProviderAsync(int opportunityId, string sentTo)
        {
            var dto = await (from o in _dbContext.Opportunity
                             join oi in _dbContext.OpportunityItem on o.Id equals oi.OpportunityId
                             join re in _dbContext.Referral on oi.Id equals re.OpportunityItemId
                             join pv in _dbContext.ProviderVenue on re.ProviderVenueId equals pv.Id
                             join p in _dbContext.Provider on pv.ProviderId equals p.Id
                             where o.Id == opportunityId &&
                                   (p.PrimaryContactEmail == sentTo ||
                                    p.SecondaryContactEmail == sentTo)
                             select new EmailBodyDto
                             {
                                 PrimaryContactEmail = p.PrimaryContactEmail,
                                 SecondaryContactEmail = p.SecondaryContactEmail,
                                 ProviderDisplayName = p.DisplayName,
                                 ProviderVenuePostcode = pv.Postcode,
                                 ProviderVenueName = pv.Name,
                             })
                .FirstOrDefaultAsync();

            return dto;
        }

        public async Task<IList<EmployerFeedbackDto>> GetReferralsForEmployerFeedbackAsync(DateTime dateToSearch)
        {
            var firstDayOfMonth = new DateTime(dateToSearch.Year, dateToSearch.Month, 1, 0, 0, 0);
            var firstDayOfFollowingMonth = firstDayOfMonth.AddMonths(1);

            var dto = await (from o in _dbContext.Opportunity
                             join oi in _dbContext.OpportunityItem
                                 on o.Id equals oi.OpportunityId
                             join ro in _dbContext.Route
                                 on oi.RouteId equals ro.Id
                             where o.EmployerCrmId.HasValue
                                   && oi.IsCompleted
                                   && oi.ModifiedOn.HasValue
                                   && (oi.ModifiedOn.Value >= firstDayOfMonth && oi.ModifiedOn.Value < firstDayOfFollowingMonth)
                                   && oi.OpportunityType == OpportunityType.Referral.ToString()
                             select new EmployerFeedbackDto
                             {
                                 OpportunityItemId = oi.Id,
                                 EmployerCrmId = o.EmployerCrmId.Value,
                                 EmployerContact = o.EmployerContact,
                                 EmployerContactEmail = o.EmployerContactEmail,
                                 JobRole = oi.JobRole,
                                 Route = ro.Name,
                                 PlacementsKnown = oi.PlacementsKnown,
                                 Placements = oi.Placements,
                                 ModifiedOn = oi.ModifiedOn,
                                 Town = oi.Town,
                                 Postcode = oi.Postcode
                             }).ToListAsync();

            return dto;
        }

        public async Task<IList<ProviderFeedbackDto>> GetReferralsForProviderFeedbackAsync(DateTime dateToSearch)
        {
            var firstDayOfMonth = new DateTime(dateToSearch.Year, dateToSearch.Month, 1, 0, 0, 0);
            var firstDayOfFollowingMonth = firstDayOfMonth.AddMonths(1);

            var allResults = await (from o in _dbContext.Opportunity
                                    join oi in _dbContext.OpportunityItem
                                        on o.Id equals oi.OpportunityId
                                    join r in _dbContext.Referral
                                        on oi.Id equals r.OpportunityItemId
                                    join e in _dbContext.Employer
                                        on o.EmployerCrmId equals e.CrmId
                                    join pv in _dbContext.ProviderVenue
                                        on r.ProviderVenueId equals pv.Id
                                    join p in _dbContext.Provider
                                        on pv.ProviderId equals p.Id
                                    join rt in _dbContext.Route
                                        on oi.RouteId equals rt.Id
                                    where o.EmployerCrmId.HasValue
                                          && oi.IsCompleted
                                          && oi.ModifiedOn.HasValue
                                          && (oi.ModifiedOn.Value >= firstDayOfMonth && oi.ModifiedOn.Value < firstDayOfFollowingMonth)
                                          && oi.OpportunityType == OpportunityType.Referral.ToString()
                                    select new
                                    {
                                        ProviderId = p.Id,
                                        ProviderName = p.Name,
                                        ProviderDisplayName = p.DisplayName,
                                        p.PrimaryContact,
                                        p.PrimaryContactEmail,
                                        p.SecondaryContact,
                                        p.SecondaryContactEmail,
                                        EmployerCompanyName = e.CompanyName,
                                        pv.Town,
                                        pv.Postcode,
                                        RouteName = rt.Name
                                    }).Distinct().ToListAsync();

            var dto = allResults.GroupBy(grp => new
            {
                grp.ProviderId,
                grp.ProviderName,
                grp.ProviderDisplayName,
                grp.PrimaryContact,
                grp.PrimaryContactEmail,
                grp.SecondaryContact,
                grp.SecondaryContactEmail,
                grp.EmployerCompanyName,
                grp.Town,
                grp.Postcode
            })
                .Select(g => new ProviderFeedbackDto
                {
                    ProviderId = g.Key.ProviderId,
                    ProviderName = g.Key.ProviderName,
                    ProviderDisplayName = g.Key.ProviderDisplayName,
                    Town = g.Key.Town,
                    Postcode = g.Key.Postcode,
                    PrimaryContact = g.Key.PrimaryContact,
                    PrimaryContactEmail = g.Key.PrimaryContactEmail,
                    SecondaryContact = g.Key.SecondaryContact,
                    SecondaryContactEmail = g.Key.SecondaryContactEmail,
                    EmployerCompanyName = g.Key.EmployerCompanyName,
                    Routes = g.Select(r => r.RouteName)
                }).OrderBy(p => p.EmployerCompanyName).ToList();

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

        private static string GetReasons(bool? hadBadExperience, bool? noSuitableStudent, bool? providersTooFarAway)
        {
            var reasons = new List<string>();

            if (hadBadExperience.HasValue && hadBadExperience.Value)
                reasons.Add("Employer had a bad experience with them");

            if (noSuitableStudent.HasValue && noSuitableStudent.Value)
                reasons.Add("Providers do not have students doing the right course");

            if (providersTooFarAway.HasValue && providersTooFarAway.Value)
                reasons.Add("Providers are too far away");

            if (reasons.Count == 0)
                reasons.Add("None available");

            return string.Join(", ", reasons);
        }
    }
}
