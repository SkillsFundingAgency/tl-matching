using System.Collections.Generic;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Functions.UnitTests.MatchingServiceReport.Builders
{
    public class MatchingServiceProviderOpportunityReportBuilder
    {
        public IList<MatchingServiceProviderOpportunityReport> BuildList() => new List<MatchingServiceProviderOpportunityReport>
        {
            new()
            {
                OpportunityItemCount = 1,
                RouteName = "Digital",
                ProviderName = "Provider Name",
                ProviderVenuePostCode = "CV1 2WT",
                LepCode = "E1234567",
                LepName = "Test LEP"
            },
            new()
            {
                OpportunityItemCount = 2,
                RouteName = "Digital",
                ProviderName = "Provider Name",
                ProviderVenuePostCode = "CV1 2WT",
                LepCode = "E1234567",
                LepName = "Test LEP"
            }
        };
    }
}
