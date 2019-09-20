using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.Event;

namespace Sfa.Tl.Matching.Application.FileReader.Employer
{
    public class CrmEmployerEventDataValidator : AbstractValidator<CrmEmployerEventBase>
    {
        public CrmEmployerEventDataValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(dto => dto.accountid)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                .WithMessage($"'{nameof(EmployerStagingFileImportDto.CrmId)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsGuid())
                .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                .WithMessage($"'{nameof(EmployerStagingFileImportDto.CrmId)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.Name)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                .WithMessage($"'{nameof(EmployerStagingFileImportDto.CompanyName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.sfa_aupa)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                .WithMessage($"'{nameof(EmployerStagingFileImportDto.Aupa)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsAupaStatus())
                .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                .WithMessage($"'{nameof(EmployerStagingFileImportDto.Aupa)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.customertypecode)
                .Must(dto => dto.IsCompanyType())
                .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                .WithMessage($"'{nameof(EmployerStagingFileImportDto.CompanyType)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.owneridname)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                .WithMessage($"'{nameof(EmployerStagingFileImportDto.Owner)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }
    }
}