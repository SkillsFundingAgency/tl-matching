using Sfa.Tl.Matching.Application.FileReader.ProviderQualification;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderQualification.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderQualification.Parsing
{
    public class ProviderQualificationParsingFixture
    {
        public ProviderQualificationFileImportDto Dto;
        public ProviderQualificationDataParser Parser;

        public ProviderQualificationParsingFixture()
        {
            Dto = new ValidProviderQualificationFileImportDtoBuilder().Build();
            Parser = new ProviderQualificationDataParser();
        }
    }
}