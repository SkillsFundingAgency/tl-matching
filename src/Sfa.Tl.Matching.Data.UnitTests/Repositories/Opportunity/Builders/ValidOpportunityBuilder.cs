using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Opportunity.Builders
{
    public class ValidOpportunityBuilder
    {
        public Domain.Models.Opportunity Build() => new Domain.Models.Opportunity
        {
            Id = 1,
            EmployerId = 5,
            EmployerContact = "Employer Contact",
            EmployerContactPhone = "020 123 4567",
            EmployerContactEmail = "employer.contact@employer.co.uk",
            CreatedBy = EntityCreationConstants.CreatedByUser,
            CreatedOn = EntityCreationConstants.CreatedOn,
            ModifiedBy = EntityCreationConstants.ModifiedByUser,
            ModifiedOn = EntityCreationConstants.ModifiedOn
        };
    }
}
