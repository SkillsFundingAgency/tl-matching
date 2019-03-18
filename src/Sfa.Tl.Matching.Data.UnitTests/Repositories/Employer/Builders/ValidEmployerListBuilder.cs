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
                CrmId = new Guid("1F7B99CB-0FAD-4FFC-AF6A-D5537293E04B"),
                CompanyName = "Company Name",
                AlsoKnownAs = "Also Known As",
                Aupa = "Aware",
                CompanyType = "CompanyType",
                PrimaryContact = "PrimaryContact",
                Phone = "01777757777",
                Email = "primary@contact.co.uk",
                Postcode = "AA1 1AA",
                Owner = "Owner",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            },
            new Domain.Models.Employer
            {
                Id = 2,
                CrmId = new Guid("2F7B99CB-0FAD-4FFC-AF6A-D5537293E04B"),
                CompanyName = "Company Name2",
                AlsoKnownAs = "Also Known As2",
                Aupa = "Aware2",
                CompanyType = "CompanyType2",
                PrimaryContact = "PrimaryContact2",
                Phone = "017777577772",
                Email = "primary2@contact.co.uk",
                Postcode = "AA2 2AA",
                Owner = "Owner2",
                CreatedBy = EntityCreationConstants.CreatedByUser,
                CreatedOn = EntityCreationConstants.CreatedOn,
                ModifiedBy = EntityCreationConstants.ModifiedByUser,
                ModifiedOn = EntityCreationConstants.ModifiedOn
            }
        };
    }
}