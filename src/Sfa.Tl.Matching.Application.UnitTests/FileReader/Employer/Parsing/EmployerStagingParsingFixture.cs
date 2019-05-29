using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Parsing
{
    public class EmployerStagingParsingFixture
    {
        public EmployerStagingDataParser Parser;
        public EmployerStagingFileImportDto Dto;

        public EmployerStagingParsingFixture()
        {
            Dto = new ValidEmployerStagingFileImportDtoBuilder().Build();
            Parser = new EmployerStagingDataParser();
        }
    }
}