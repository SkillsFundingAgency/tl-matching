using Sfa.Tl.Matching.Application.FileReader.PostcodeLookupStaging;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.PostcodeLookup.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.PostcodeLookup.Parsing
{
    public class PostcodeLookupStagingParsingFixture
    {
        public PostcodeLookupStagingDataParser Parser;
        public PostcodeLookupStagingFileImportDto Dto;

        public PostcodeLookupStagingParsingFixture()
        {
            Dto = new ValidPostcodeLookupStagingFileImportDtoBuilder().Build();
            Parser = new PostcodeLookupStagingDataParser();
        }
    }
}