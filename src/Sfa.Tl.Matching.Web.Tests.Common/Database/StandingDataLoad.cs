using System.Linq;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Web.Tests.Common.Database.StandingData;

namespace Sfa.Tl.Matching.Web.Tests.Common.Database
{
    internal class StandingDataLoad
    {
        internal static void Load(MatchingDbContext context)
        {
            var providerVenues = ProviderVenueData.Create();
            context.AddRange(EmailTemplateData.Create().ToList());
            context.AddRange(EmployerData.Create().ToList());
            context.AddRange(RouteAndPathData.Create().ToList());
            context.AddRange(providerVenues);            
            context.AddRange(OpportunityData.Create(providerVenues));
            context.AddRange(ServiceStatusHistoryData.Create().ToList());
            context.AddRange(LearningAimReferenceData.Create().ToList());
            context.AddRange(OpportunityData.CreateReferralSingle(providerVenues.ElementAt(0)));
            context.AddRange(OpportunityData.CreateProvisionGapSingle());
            context.AddRange(OpportunityData.CreateReferralMultiple(providerVenues));
            context.AddRange(OpportunityData.CreateReferralMultipleAndProvisionGap(providerVenues));
            context.AddRange(OpportunityData.CreateReferralSingleAndProvisionGap(providerVenues.ElementAt(0)));
            context.AddRange(OpportunityData.CreateProvidersMultiple(providerVenues));
            context.AddRange(OpportunityData.CreateNReferrals(5000, providerVenues.ElementAt(0)));

            context.SaveChanges();
        }
    }
}