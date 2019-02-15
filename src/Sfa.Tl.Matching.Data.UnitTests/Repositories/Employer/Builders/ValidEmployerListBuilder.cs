using System;
using System.Collections.Generic;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Application.UnitTests.Data.Employer.Builders
{
    internal class ValidEmployerListBuilder
    {
        private readonly IList<Domain.Models.Employer> _employers;

        public ValidEmployerListBuilder()
        {
            _employers =
                new List<Domain.Models.Employer>
            {
                new Domain.Models.Employer
                {
                    Id = 1,
                    CrmId = new Guid("8F7B99CB-0FAD-4FFC-AF6A-D5537293E04B"),
                    CompanyName = "Company Name",
                    AlsoKnownAs = "Also Known As",
                    Aupa = "Aware",
                    CompanyType = "CompanyType",
                    PrimaryContact = "PrimaryContact",
                    Phone = "01777757777",
                    Email = "primary@contact.co.uk",
                    PostCode = "AA1 1AA",
                    Owner = "Owner",
                    CreatedBy = EntityCreationConstants.CreatedByUser,
                    CreatedOn = EntityCreationConstants.CreatedOn,
                    ModifiedBy = EntityCreationConstants.ModifiedByUser,
                    ModifiedOn = EntityCreationConstants.ModifiedOn
                },
                new Domain.Models.Employer
                {
                    Id = 2,
                    CrmId = new Guid("948C8B73-DF58-4EFB-96E1-8C2A3AD060F0"),
                    CompanyName = "Company Name 2",
                    AlsoKnownAs = "Also Known As 2",
                    Aupa = "Unware",
                    CompanyType = "CompanyType",
                    PrimaryContact = "PrimaryContact2",
                    Phone = "01777751111",
                    Email = "anotherprimary@contact.co.uk",
                    PostCode = "AA2 0BB",
                    Owner = "Owner2",
                    CreatedBy = EntityCreationConstants.CreatedByUser,
                    CreatedOn = EntityCreationConstants.CreatedOn,
                    ModifiedBy = EntityCreationConstants.ModifiedByUser,
                    ModifiedOn = EntityCreationConstants.ModifiedOn
                }
            };
        }

        public IEnumerable<Domain.Models.Employer> Build() =>
            _employers;
    }
}
