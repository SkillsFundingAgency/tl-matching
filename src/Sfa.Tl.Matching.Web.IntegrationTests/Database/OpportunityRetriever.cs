using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Database
{
    public class OpportunityRetriever
    {
        public Domain.Models.Opportunity GetLast()
        {
            var testConfig = new TestConfiguration();
            var matchingDbContext = testConfig.GetDbContext();

            var opportuinity =matchingDbContext.Opportunity
                    .Include(o => o.OpportunityItem)
                        .ThenInclude(oi => oi.Referral)
                        .ThenInclude(r => r.ProviderVenue)
                        .ThenInclude(pv => pv.Provider)
                    .Include(o => o.OpportunityItem)
                        .ThenInclude(oi => oi.Route)
                    .Last();

            return opportuinity;
        }
    }
}