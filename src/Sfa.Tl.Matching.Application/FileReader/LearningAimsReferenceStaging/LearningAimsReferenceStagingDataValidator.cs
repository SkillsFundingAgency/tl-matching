using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.LearningAimsReferenceStaging
{
    public class LearningAimsReferenceStagingDataValidator : AbstractValidator<LearningAimsReferenceStagingFileImportDto>
    {
        public LearningAimsReferenceStagingDataValidator()
        {
            RuleFor(dto => dto.LarId)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                .WithMessage($"'{nameof(LearningAimsReferenceStagingFileImportDto.LarId)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .MaximumLength(8)
                .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                .WithMessage($"'{nameof(LearningAimsReferenceStagingFileImportDto.LarId)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.Title)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                .WithMessage($"'{nameof(LearningAimsReferenceStagingFileImportDto.Title)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .MaximumLength(400)
                .WithErrorCode(ValidationErrorCode.InvalidLength.ToString())
                .WithMessage($"'{nameof(LearningAimsReferenceStagingFileImportDto.Title)}' - {ValidationErrorCode.InvalidLength.Humanize()}");

            RuleFor(dto => dto.SourceCreatedOn)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                .WithMessage($"'{nameof(LearningAimsReferenceStagingFileImportDto.SourceCreatedOn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsDateTime())
                .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                .WithMessage($"'{nameof(LearningAimsReferenceStagingFileImportDto.SourceCreatedOn)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.SourceModifiedOn)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                .WithMessage($"'{nameof(LearningAimsReferenceStagingFileImportDto.SourceModifiedOn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsDateTime())
                .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                .WithMessage($"'{nameof(LearningAimsReferenceStagingFileImportDto.SourceModifiedOn)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");
        }
    }
}