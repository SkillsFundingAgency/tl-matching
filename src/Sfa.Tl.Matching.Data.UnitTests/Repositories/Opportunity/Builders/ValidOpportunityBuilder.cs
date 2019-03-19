using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders
{
    public class ValidOpportunityBuilder
    {
        public Domain.Models.Opportunity Build() => new Domain.Models.Opportunity
        {
            Id = 1,
            RouteId = 2,
            Postcode = "AA1 1AA",
            SearchRadius = 10,
            JobTitle = "Testing Job Title",
            DropOffStage = 9,
            PlacementsKnown = true,
            Placements = 3,
            SearchResultProviderCount = 12,
            EmployerId = 5,
            EmployerName = "Employer",
            EmployerContact = "Employer Contact",
            EmployerContactPhone = "020 123 4567",
            EmployerContactEmail = "employer.contact@employer.co.uk",
            UserEmail = "employer.contact@employer.co.uk",
            ConfirmationSelected = true,
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
