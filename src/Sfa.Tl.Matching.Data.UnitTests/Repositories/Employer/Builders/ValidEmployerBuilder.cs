using System;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Employer.Builders
{
    public class ValidEmployerBuilder
    {
        public Domain.Models.Employer Build() => new Domain.Models.Employer
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
        };
    }
}