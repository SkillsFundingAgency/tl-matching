using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Builders
{
    public class ValidEmployerFileImportDtoBuilder
    {
        public const string CrmId = "7FBD4621-CEAF-4DFA-B8D6-E98C0567CD27";
        public const string CompanyName = "CompanyName";
        public const string AlsoKnownAs = "AlsoKnownAs";
        public const string Aupa = "Active";
        public const string CompanyType = "Employer";
        public const string PrimaryContact = "PrimaryContact";
        public const string Email = "email@address.com";
        public const string Phone = "01474777777";
        public const string PostCode = "AB1 1AA";
        public const string Owner = "Owner";
        public const string CreatedBy = "CreatedBy";

        public EmployerFileImportDto Build() => new EmployerFileImportDto
        {
            CrmId = CrmId,
            CompanyName = CompanyName,
            AlsoKnownAs = AlsoKnownAs,
            Aupa = Aupa,
            CompanyType = CompanyType,
            PrimaryContact = PrimaryContact,
            Phone = Phone,
            Email = Email,
            PostCode = PostCode,
            Owner = Owner,
            CreatedBy = CreatedBy
        };
    }
}