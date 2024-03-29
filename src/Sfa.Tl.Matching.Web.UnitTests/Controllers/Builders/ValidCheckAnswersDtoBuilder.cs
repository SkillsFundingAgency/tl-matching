﻿using System.Collections.Generic;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidCheckAnswersDtoBuilder
    {
        public CheckAnswersViewModel Build() => new()
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
                new() { Name = "Provider1", DistanceFromEmployer = 1.3m, Postcode = "AA1 1AA" },
                new() { Name = "Provider2", DistanceFromEmployer = 31.6m, Postcode = "BB1 1BB" }
            }
        };
    }
}