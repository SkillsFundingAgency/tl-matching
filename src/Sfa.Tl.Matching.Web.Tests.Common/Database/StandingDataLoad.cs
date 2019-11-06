using System.Linq;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Web.Tests.Common.Database.StandingData;

namespace Sfa.Tl.Matching.Web.Tests.Common.Database
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
            context.AddRange(ServiceStatusHistoryData.Create().ToList());
            context.AddRange(LearningAimReferenceData.Create().ToList());
            context.AddRange(OpportunityData.CreateReferralSingle());
            context.AddRange(OpportunityData.CreateProvisionGapSingle());
            context.AddRange(OpportunityData.CreateReferralMultiple());
            context.AddRange(OpportunityData.CreateReferralMultipleAndProvisionGap());
            context.AddRange(OpportunityData.CreateReferralSingleAndProvisionGap());
            context.AddRange(OpportunityData.CreateProvidersMultiple());
            context.AddRange(OpportunityData.CreateNReferrals(1400));

            context.SaveChanges();
        }
    }
}