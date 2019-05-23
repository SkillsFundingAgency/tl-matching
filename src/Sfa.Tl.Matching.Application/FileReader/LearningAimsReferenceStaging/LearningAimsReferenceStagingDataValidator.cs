using FluentValidation;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.LearningAimsReferenceStaging
{
    public class LearningAimsReferenceStagingDataValidator : AbstractValidator<LearningAimsReferenceStagingFileImportDto>
    {
        public LearningAimsReferenceStagingDataValidator()
        {
        }
    }
}