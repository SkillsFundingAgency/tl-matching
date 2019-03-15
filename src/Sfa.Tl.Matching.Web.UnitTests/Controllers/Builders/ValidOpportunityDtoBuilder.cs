using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Web.UnitTests.Controllers.Builders
{
    internal class ValidOpportunityDtoBuilder
    {
        private readonly OpportunityDto _dto;

        public ValidOpportunityDtoBuilder()
        {
            _dto = new OpportunityDto
            {
                Id = 1,
                SearchRadius = 3,
                JobTitle = "JobTitle",
                PlacementsKnown = true,
                Placements = 2,
                Postcode = "AA1 1AA",
                EmployerName = "CompanyName",
                EmployerContact = "EmployerContact",
                EmployerContactEmail = "EmployerContactEmail",
                EmployerContactPhone = "EmployerContactPhone",
                RouteName = "RouteName",
                ModifiedBy = "ModifiedBy"
            };
        }

        public OpportunityDto Build() =>
            _dto;
    }
}