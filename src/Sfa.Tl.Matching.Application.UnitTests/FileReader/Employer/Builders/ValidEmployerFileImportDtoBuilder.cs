using Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Constants;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Builders
{
    internal class ValidEmployerFileImportDtoBuilder
    {
        private readonly EmployerFileImportDto _employerFileImportDto;

        public ValidEmployerFileImportDtoBuilder()
        {
            _employerFileImportDto = new EmployerFileImportDto
            {
                CrmId = EmployerConstants.CrmId,
                CompanyName = EmployerConstants.CompanyName,
                AlsoKnownAs = EmployerConstants.AlsoKnownAs,
                Aupa = EmployerConstants.Aupa,
                CompanyType = EmployerConstants.CompanyType,
                PrimaryContact = EmployerConstants.PrimaryContact,
                Phone = EmployerConstants.Phone,
                Email = EmployerConstants.Email,
                PostCode = EmployerConstants.PostCode,
                Owner = EmployerConstants.Owner,
                CreatedBy = EmployerConstants.CreatedBy
            };
        }

        public EmployerFileImportDto Build() =>
            _employerFileImportDto;
    }
}