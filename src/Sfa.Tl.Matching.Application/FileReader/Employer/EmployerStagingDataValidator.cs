using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.Employer
{
    public class EmployerStagingDataValidator : AbstractValidator<EmployerStagingFileImportDto>
    {
        public EmployerStagingDataValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(dto => dto.CrmId)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(EmployerStagingFileImportDto.CrmId)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsGuid())
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(EmployerStagingFileImportDto.CrmId)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.CompanyName)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(EmployerStagingFileImportDto.CompanyName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.Aupa)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(EmployerStagingFileImportDto.Aupa)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsAupaStatus())
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(EmployerStagingFileImportDto.Aupa)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            //this will return true if its null or a valid Company Type because this is a optional field
            RuleFor(dto => dto.CompanyType)
                .Must(dto => dto.IsCompanyType())
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(EmployerStagingFileImportDto.CompanyType)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.Postcode)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(EmployerStagingFileImportDto.Postcode)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.Owner)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(EmployerStagingFileImportDto.Owner)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }
    }
}