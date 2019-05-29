using Sfa.Tl.Matching.Application.FileReader.LearningAimsReferenceStaging;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimsReference.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimsReference.Parsing
{
    public class LearningAimsReferenceStagingParsingFixture
    {
        public LearningAimsReferenceStagingDataParser Parser;
        public LearningAimsReferenceStagingFileImportDto Dto;

        public LearningAimsReferenceStagingParsingFixture()
        {
            Dto = new ValidLearningAimsReferenceStagingFileImportDtoBuilder().Build();
            Parser = new LearningAimsReferenceStagingDataParser();
        }
    }
}