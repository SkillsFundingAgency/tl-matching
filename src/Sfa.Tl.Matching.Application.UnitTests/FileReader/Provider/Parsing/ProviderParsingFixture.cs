using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Parsing
{
    public class ProviderParsingFixture
    {
        public ProviderDataParser Parser;
        public ProviderFileImportDto Dto;

        public ProviderParsingFixture()
        {
            Dto = new ValidProviderFileImportDtoBuilder().Build();
            Parser = new ProviderDataParser();
        }
    }
}