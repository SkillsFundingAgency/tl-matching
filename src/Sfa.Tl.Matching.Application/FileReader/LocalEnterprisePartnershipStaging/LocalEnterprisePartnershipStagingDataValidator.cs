using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.LocalEnterprisePartnershipStaging
{
    public class LocalEnterprisePartnershipStagingDataValidator : AbstractValidator<LocalEnterprisePartnershipStagingFileImportDto>
    {
        public LocalEnterprisePartnershipStagingDataValidator()
        {
            RuleFor(dto => dto.Code)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(LocalEnterprisePartnershipStagingFileImportDto.Code)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Length(9)
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(LocalEnterprisePartnershipStagingFileImportDto.Code)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.Name)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(LocalEnterprisePartnershipStagingFileImportDto.Name)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .MaximumLength(100)
                    .WithErrorCode(ValidationErrorCode.InvalidLength.ToString())
                    .WithMessage($"'{nameof(LocalEnterprisePartnershipStagingFileImportDto.Name)}' - {ValidationErrorCode.InvalidLength.Humanize()}");
        }
    }
}