using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidCheckAnswersDtoBuilder
    {
        public CheckAnswersDto Build() => new CheckAnswersDto
        {
            OpportunityId = 1,
            SearchRadius = 3,
            JobRole = "JobRole",
            PlacementsKnown = true,
            Placements = 2,
            Postcode = "AA1 1AA",
            EmployerName = "EmployerName",
            RouteName = "RouteName",
            ModifiedBy = "ModifiedBy"
        };
    }
}