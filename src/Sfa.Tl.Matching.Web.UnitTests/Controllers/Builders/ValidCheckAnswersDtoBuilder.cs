using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidCheckAnswersDtoBuilder
    {
        public CheckAnswersDto Build() => new CheckAnswersDto
        {
            OpportunityId = 1,
            SearchRadius = 3,
            JobTitle = "JobTitle",
            PlacementsKnown = true,
            Placements = 2,
            Postcode = "AA1 1AA",
            EmployerName = "EmployerName",
            EmployerContact = "EmployerContact",
            RouteName = "RouteName",
            ModifiedBy = "ModifiedBy"
        };
    }
}