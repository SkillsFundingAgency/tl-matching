using System.Collections.Generic;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidCheckAnswersDtoBuilder
    {
        public CheckAnswersViewModel Build() => new CheckAnswersViewModel
        {
            OpportunityId = 1,
            OpportunityItemId = 2,
            SearchRadius = 3,
            JobRole = "JobRole",
            Placements = 2,
            Postcode = "AA1 1AA",
            CompanyName = "CompanyName",
            CompanyNameAka = "AlsoKnownAs",
            RouteName = "RouteName",
            Providers = new List<ReferralsViewModel>
            {                 
                new ReferralsViewModel { ProviderName = "Provider1", DistanceFromEmployer = 1.3m, ProviderVenuePostcode = "AA1 1AA" },
                new ReferralsViewModel { ProviderName = "Provider2", DistanceFromEmployer = 31.6m, ProviderVenuePostcode = "BB1 1BB" }
            }
        };
    }
}