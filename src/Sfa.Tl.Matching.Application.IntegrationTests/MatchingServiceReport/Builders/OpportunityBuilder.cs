using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Core.Internal;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.IntegrationTests.MatchingServiceReport.Builders
{
    public class OpportunityBuilder
    {
        private readonly MatchingDbContext _context;

        public OpportunityBuilder(MatchingDbContext context)
        {
            _context = context;
        }

        public Domain.Models.Opportunity CreateOpportunity(Guid employerCrmId, List<OpportunityItem> opportunityItems)
        {
            var opportunity = new Domain.Models.Opportunity
            {
                EmployerCrmId = employerCrmId,
                EmployerContact = "test",
                EmployerContactEmail = "test@test.com",
                EmployerContactPhone = "01234567890",
                OpportunityItem = opportunityItems,
                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
            };

            _context.Add(opportunity);

            _context.SaveChanges();

            return opportunity;
        }

        public OpportunityItem CreateReferralOpportunityItem(bool isSaved, bool isCompleted, params int[] providerVenueId)
        {
            return new OpportunityItem
            {
                Town = "test town",
                JobRole = "test job role",
                Postcode = "POST PO1",

                Placements = 1,
                PlacementsKnown = true,

                IsSaved = isSaved,
                IsCompleted = isCompleted,
                IsSelectedForReferral = true,
                OpportunityType = OpportunityType.Referral.ToString(),

                RouteId = 1,
                SearchResultProviderCount = 1,
                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                Referral = providerVenueId.Select(pv => new Referral
                {
                    DistanceFromEmployer = 1.1m,
                    ProviderVenueId = pv,
                    CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests"
                }).ToList()
            };
        }

        public OpportunityItem CreateProvisionGapOpportunityItem(bool isSaved, bool isCompleted)
        {
            return new OpportunityItem
            {
                Town = "test town",
                JobRole = "test job role",
                Postcode = "POST PO1",

                Placements = 1,
                PlacementsKnown = true,

                IsSaved = isSaved,
                IsCompleted = isCompleted,
                IsSelectedForReferral = true,
                OpportunityType = OpportunityType.ProvisionGap.ToString(),

                RouteId = 1,
                SearchResultProviderCount = 1,
                CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                ProvisionGap = new List<ProvisionGap>
                {
                    new ProvisionGap
                    {
                        HadBadExperience = true,
                        NoSuitableStudent = true,
                        ProvidersTooFarAway = true,
                        CreatedBy = "Sfa.Tl.Matching.Application.IntegrationTests",
                    }
                }
            };
        }

        public void ClearData()
        {
            var referral = _context.Referral.Where(r => r.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!referral.IsNullOrEmpty()) _context.Referral.RemoveRange(referral);

            var provisionGap = _context.ProvisionGap.Where(r => r.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!provisionGap.IsNullOrEmpty()) _context.ProvisionGap.RemoveRange(provisionGap);

            var oppItems = _context.OpportunityItem.Where(o => o.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!oppItems.IsNullOrEmpty()) _context.OpportunityItem.RemoveRange(oppItems);

            var opp = _context.Opportunity.Where(o => o.CreatedBy == "Sfa.Tl.Matching.Application.IntegrationTests").ToList();
            if (!opp.IsNullOrEmpty()) _context.Opportunity.RemoveRange(opp);

            _context.SaveChanges();
        }
    }
}