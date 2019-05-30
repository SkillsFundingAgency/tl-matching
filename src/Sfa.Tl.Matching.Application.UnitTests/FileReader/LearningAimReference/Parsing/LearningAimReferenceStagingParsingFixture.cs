using Sfa.Tl.Matching.Application.FileReader.LearningAimReferenceStaging;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimReference.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimReference.Parsing
{
    public class LearningAimReferenceStagingParsingFixture
    {
        public LearningAimReferenceStagingDataParser Parser;
        public LearningAimReferenceStagingFileImportDto Dto;

        public LearningAimReferenceStagingParsingFixture()
        {
            Dto = new ValidLearningAimReferenceStagingFileImportDtoBuilder().Build();
            Parser = new LearningAimReferenceStagingDataParser();
        }
    }
}