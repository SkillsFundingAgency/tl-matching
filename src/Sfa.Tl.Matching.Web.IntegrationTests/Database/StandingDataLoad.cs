using System.Linq;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Web.IntegrationTests.Database.StandingData;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Database
{
    internal class StandingDataLoad
    {
        internal static void Load(MatchingDbContext context)
        {
            context.AddRange(EmailTemplateData.Create().ToList());
            context.AddRange(EmployerData.Create().ToList());
            context.AddRange(RouteAndPathData.Create().ToList());
            context.AddRange(ProviderVenueData.Create());
            context.AddRange(OpportunityData.Create());
            context.AddRange(OpportunityData.CreateReferralSingle());
            context.AddRange(OpportunityData.CreateProvisionGapSingle());
            context.AddRange(OpportunityData.CreateReferralMultiple());
            context.AddRange(OpportunityData.CreateReferralMultipleAndProvisionGap());
            context.AddRange(OpportunityData.CreateReferralSingleAndProvisionGap());

            context.SaveChanges();
        }
    }
}