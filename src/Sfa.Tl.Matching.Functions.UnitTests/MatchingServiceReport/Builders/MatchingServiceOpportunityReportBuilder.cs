using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Functions.UnitTests.MatchingServiceReport.Builders
{
    public class MatchingServiceOpportunityReportBuilder
    {
        public IList<MatchingServiceOpportunityReport> BuildList() => new List<MatchingServiceOpportunityReport>
        {
            new()
            {
                OpportunityItemId = 1,
                OpportunityType = "Referral",
                PipelineOpportunity = false,
                EmployerId = 101,
                CompanyName = "Company",
                Aupa = "Aware",
                Owner = "Owner",
                EmployerPostCodeEnteredInSearch = "CV1 2WT",
                Placements = 1,
                JobRole = "Test job",
                RouteName = "Digital",
                LepCode = "E1234567",
                LepName = "Test LEP",
                NoSuitableStudent = false,
                HadBadExperience = false,
                ProvidersTooFarAway = false,
                UserName = "Test User",
                CreatedOn = DateTime.UtcNow
            },
            new()
            {
                OpportunityItemId = 2,
                //OpportunityType = "ProvisionGap",
                PipelineOpportunity = false,
                EmployerId = 101,
                CompanyName = "Company",
                Aupa = "Active",
                Owner = "Owner",
                EmployerPostCodeEnteredInSearch = "CV1 2WT",
                Placements = 1,
                JobRole = "Test job",
                RouteName = "Digital",
                LepCode = "E1234567",
                LepName = "Test LEP",
                NoSuitableStudent = false,
                HadBadExperience = false,
                ProvidersTooFarAway = false,
                UserName = "Test User",
                CreatedOn = DateTime.UtcNow
            }
        };
    }
}
