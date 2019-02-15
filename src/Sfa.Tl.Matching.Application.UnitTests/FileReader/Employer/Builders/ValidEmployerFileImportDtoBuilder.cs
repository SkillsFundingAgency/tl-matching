using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Builders
{
    internal class ValidEmployerFileImportDtoBuilder
    {
        public static EmployerFileImportDto Build() => new EmployerFileImportDto
        {
            CrmId = "7FBD4621-CEAF-4DFA-B8D6-E98C0567CD27",
            CompanyName = "CompanyName",
            AlsoKnownAs = "AlsoKnownAs",
            Aupa = "Active",
            CompanyType = "Employer",
            PrimaryContact = "PrimaryContact",
            Phone = "01474777777",
            Email = "email@address.com",
            PostCode = "AB1 1AA",
            Owner = "Owner"
        };
    }
}