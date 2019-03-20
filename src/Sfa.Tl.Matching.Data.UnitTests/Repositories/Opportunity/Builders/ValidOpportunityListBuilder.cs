using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders
{
    public class ValidOpportunityListBuilder
    {
        public IList<Domain.Models.Opportunity> Build() => new List<Domain.Models.Opportunity>
        { 
            new Domain.Models.Opportunity
            {
                Id = 1,
                RouteId = 1,
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
            },
            new Domain.Models.Opportunity
            {
                Id = 2,
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
            }
        };
    }
}
