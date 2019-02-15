using System;
using Sfa.Tl.Matching.Data.UnitTests.Repositories.Constants;

namespace Sfa.Tl.Matching.Data.UnitTests.Repositories.Employer.Builders
{
    internal class ValidEmployerBuilder
    {
        private readonly Domain.Models.Employer _employer;

        public ValidEmployerBuilder()
        {
            _employer = new Domain.Models.Employer
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
            };
        }

        public Domain.Models.Employer Build() =>
            _employer;
    }
}
