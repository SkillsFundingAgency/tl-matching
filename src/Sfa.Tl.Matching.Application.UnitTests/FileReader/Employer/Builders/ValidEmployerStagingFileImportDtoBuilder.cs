using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Builders
{
    public class ValidEmployerStagingFileImportDtoBuilder
    {
        public const string CrmId = "7FBD4621-CEAF-4DFA-B8D6-E98C0567CD27";
        public const string Companyname = "Companyname";
        public const string Alsoknownas = "Alsoknownas";
        public const string Aupa = "Active";
        public const string CompanyType = "Employer";
        public const string PrimaryContact = "PrimaryContact";
        public const string Email = "email@address.com";
        public const string Phone = "01474777777";
        public const string Postcode = "AB1 1AA";
        public const string Owner = "Owner";
        public const string CreatedBy = "CreatedBy";

        public EmployerStagingFileImportDto Build() => new EmployerStagingFileImportDto
        {
            CrmId = CrmId,
            CompanyName = Companyname,
            AlsoKnownAs = Alsoknownas,
            Aupa = Aupa,
            CompanyType = CompanyType,
            PrimaryContact = PrimaryContact,
            Phone = Phone,
            Email = Email,
            Postcode = Postcode,
            Owner = Owner,
            CreatedBy = CreatedBy
        };
    }
}