using System;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders
{
    public class ValidEmployerBuilder
    {
        private readonly Domain.Models.Employer _employer;

        public ValidEmployerBuilder()
        {
            _employer = new Domain.Models.Employer
            {
                Id = 1,
                CrmId = new Guid("A3185517-2255-4299-ACCA-36116D512B46"),
                CompanyName = "My Company",
                AlsoKnownAs = "Another Also Known As",
                Aupa = "Active",
                CompanyType = "Employer",
                PrimaryContact = "Primary Contact",
                Phone = "01474777777",
                Email = "email@address.com",
                PostCode = "AB1 1AA",
                Owner = "Owner",
                CreatedBy = "CreatedBy",
                ModifiedBy = "ModifiedBy"
            };
        }

        public Domain.Models.Employer Build() =>
            _employer;
    }
}
