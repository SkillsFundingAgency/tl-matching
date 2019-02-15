using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Validation
{
    public class EmployerFileImportFixture
    {
        public EmployerDataValidator Validator;
        public EmployerFileImportDto Dto;

        public EmployerFileImportFixture()
        {
            Dto = new ValidEmployerFileImportDtoBuilder().Build();
            Validator = new EmployerDataValidator();
        }
    }
}