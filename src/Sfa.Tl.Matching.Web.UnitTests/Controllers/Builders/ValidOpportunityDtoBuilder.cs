using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidOpportunityDtoBuilder
    {
        public OpportunityDto Build() => new OpportunityDto
        {
            Id = 1,
            SearchRadius = 3,
            JobTitle = "JobTitle",
            PlacementsKnown = true,
            Placements = 2,
            Postcode = "AA1 1AA",
            EmployerName = "EmployerName",
            EmployerContact = "EmployerContact",
            EmployerContactEmail = "EmployerContactEmail",
            EmployerContactPhone = "EmployerContactPhone",
            RouteName = "RouteName",
            ModifiedBy = "ModifiedBy"
        };
    }
}