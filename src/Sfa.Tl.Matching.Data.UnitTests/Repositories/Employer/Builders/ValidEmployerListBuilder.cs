using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Employer.Builders
{
    public class ValidEmployerListBuilder
    {
        public IList<Domain.Models.Employer> Build() => new List<Domain.Models.Employer>
        {
            new Domain.Models.Employer
            {
                Id = 1,
                CrmId = new Guid("55555555-5555-5555-5555-555555555555"),
                CompanyName = "Company",
                AlsoKnownAs = "Also Known As",
                CompanyNameSearch = "CompanyAlsoKnownAs",
                Aupa = "Aware",
                PrimaryContact = "Employer Contact",
                Phone = "020 123 4567",
                Email = "employer.contact@employer.co.uk",
                Owner = "Owner",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.Employer
            {
                Id = 2,
                CrmId = new Guid("66666666-6666-6666-6666-666666666666"),
                CompanyName = "Company Two",
                AlsoKnownAs = "Also Known As Two",
                CompanyNameSearch = "CompanyTwoAlsoKnownAsTwo",
                Aupa = "Active",
                PrimaryContact = "Employer Two Contact",
                Phone = "020 123 2222",
                Email = "employer.two.contact@employer.co.uk",
                Owner = "Owner",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}
