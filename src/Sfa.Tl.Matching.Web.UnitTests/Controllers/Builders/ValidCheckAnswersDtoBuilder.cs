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
            PlacementsKnown = true,
            Placements = 2,
            Postcode = "AA1 1AA",
            EmployerName = "EmployerName",
            RouteName = "RouteName",
            Providers = new List<ReferralsViewModel>
            {                 
                new ReferralsViewModel { Name = "Provider1", DistanceFromEmployer = 1.3m, Postcode = "AA1 1AA" },
                new ReferralsViewModel { Name = "Provider2", DistanceFromEmployer = 31.6m, Postcode = "BB1 1BB" }
            }
        };
    }
}