using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Validation
{
    public class EmployerStagingFileImportFixture
    {
        public EmployerStagingDataValidator Validator;
        public EmployerStagingFileImportDto Dto;

        public EmployerStagingFileImportFixture()
        {
            Dto = new ValidEmployerStagingFileImportDtoBuilder().Build();
            Validator = new EmployerStagingDataValidator();
        }
    }
}