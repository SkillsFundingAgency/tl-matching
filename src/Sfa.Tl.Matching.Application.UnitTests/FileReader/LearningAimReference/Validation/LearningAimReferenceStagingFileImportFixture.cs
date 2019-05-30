using Sfa.Tl.Matching.Application.FileReader.LearningAimReferenceStaging;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimReference.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimReference.Validation
{
    public class LearningAimReferenceStagingFileImportFixture
    {
        public LearningAimReferenceStagingDataValidator Validator;
        public LearningAimReferenceStagingFileImportDto Dto;

        public LearningAimReferenceStagingFileImportFixture()
        {
            Dto = new ValidLearningAimReferenceStagingFileImportDtoBuilder().Build();
            Validator = new LearningAimReferenceStagingDataValidator();
        }
    }
}