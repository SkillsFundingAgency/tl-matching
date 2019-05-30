using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.LearningAimReferenceStaging
{
    public class LearningAimReferenceStagingDataValidator : AbstractValidator<LearningAimReferenceStagingFileImportDto>
    {
        public LearningAimReferenceStagingDataValidator()
        {
            RuleFor(dto => dto.LarId)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(LearningAimReferenceStagingFileImportDto.LarId)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Length(8)
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(LearningAimReferenceStagingFileImportDto.LarId)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.Title)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(LearningAimReferenceStagingFileImportDto.Title)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .MaximumLength(400)
                    .WithErrorCode(ValidationErrorCode.InvalidLength.ToString())
                    .WithMessage($"'{nameof(LearningAimReferenceStagingFileImportDto.Title)}' - {ValidationErrorCode.InvalidLength.Humanize()}");

            RuleFor(dto => dto.SourceCreatedOn)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(LearningAimReferenceStagingFileImportDto.SourceCreatedOn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsDateTime())
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(LearningAimReferenceStagingFileImportDto.SourceCreatedOn)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.SourceModifiedOn)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(LearningAimReferenceStagingFileImportDto.SourceModifiedOn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsDateTime())
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(LearningAimReferenceStagingFileImportDto.SourceModifiedOn)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");
        }
    }
}