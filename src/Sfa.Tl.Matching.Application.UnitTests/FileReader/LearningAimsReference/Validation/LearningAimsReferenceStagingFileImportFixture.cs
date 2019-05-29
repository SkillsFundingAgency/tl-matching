using Sfa.Tl.Matching.Application.FileReader.LearningAimsReferenceStaging;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimsReference.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimsReference.Validation
{
    public class LearningAimsReferenceStagingFileImportFixture
    {
        public LearningAimsReferenceStagingDataValidator Validator;
        public LearningAimsReferenceStagingFileImportDto Dto;

        public LearningAimsReferenceStagingFileImportFixture()
        {
            Dto = new ValidLearningAimsReferenceStagingFileImportDtoBuilder().Build();
            Validator = new LearningAimsReferenceStagingDataValidator();
        }
    }
}