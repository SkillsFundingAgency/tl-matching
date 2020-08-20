using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Functions.UnitTests.MatchingServiceReport.Builders
{
    public class MatchingServiceProviderEmployerReportBuilder
    {
        public IList<MatchingServiceProviderEmployerReport> BuildList() => new List<MatchingServiceProviderEmployerReport>
        {
            new MatchingServiceProviderEmployerReport
            {
                OpportunityItemId = 1,
                ProviderName = "Provider Name",
                ProviderVenueName = "Venue Name",
                EmployerName = "Company",
                EmployerPostCode = "CV1 2WT",
                RouteName = "Digital",
                JobRole = "Test job",
                CreatedBy = "Sfa.Tl.Matching.Functions.UnitTests",
                Placements = 1,
                CreatedOn = DateTime.UtcNow
            },
            new MatchingServiceProviderEmployerReport
            {
                OpportunityItemId = 2,
                ProviderName = "Provider Name",
                ProviderVenueName = "Another Venue Name",
                EmployerName = "Company",
                EmployerPostCode = "CV1 2WX",
                RouteName = "Digital",
                JobRole = "Another job",
                CreatedBy = "Sfa.Tl.Matching.Functions.UnitTests",
                Placements = 2,
                CreatedOn = DateTime.UtcNow
            }
        };
    }
}
