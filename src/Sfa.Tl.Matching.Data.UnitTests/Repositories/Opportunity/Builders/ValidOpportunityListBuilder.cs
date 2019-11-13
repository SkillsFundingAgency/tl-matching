using System;
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
                EmployerCrmId = new Guid("55555555-5555-5555-5555-555555555555"),
                EmployerContact = "Employer Contact",
                EmployerContactPhone = "020 123 4567",
                EmployerContactEmail = "employer.contact@employer.co.uk",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.Opportunity
            {
                Id = 2,
                EmployerCrmId = new Guid("66666666-6666-6666-6666-666666666666"),
                EmployerContact = "Employer Contact",
                EmployerContactPhone = "020 123 4567",
                EmployerContactEmail = "employer.contact@employer.co.uk",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
