using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Parsing
{
    public class EmployerParsingFixture
    {
        public EmployerDataParser Parser;
        public EmployerFileImportDto Dto;

        public EmployerParsingFixture()
        {
            Dto = new ValidEmployerFileImportDtoBuilder().Build();
            Parser = new EmployerDataParser();
        }
    }
}