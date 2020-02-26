using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.Event;

namespace Sfa.Tl.Matching.Application.FileReader.Employer
{
    public class CrmEmployerEventDataValidator : AbstractValidator<CrmEmployerEventBase>
    {
        public CrmEmployerEventDataValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(dto => dto.AccountId)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                .WithMessage($"'{nameof(Domain.Models.Employer.CrmId)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsGuid())
                .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                .WithMessage($"'{nameof(Domain.Models.Employer.CrmId)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.Name)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                .WithMessage($"'{nameof(Domain.Models.Employer.CompanyName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.sfa_aupa)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                .WithMessage($"'{nameof(Domain.Models.Employer.Aupa)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsAupaStatus())
                .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                .WithMessage($"'{nameof(Domain.Models.Employer.Aupa)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.CustomerTypeCode)
                .Must(dto => dto.IsCompanyType())
                .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                .WithMessage($"'CustomerTypeCode' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.OwnerIdName)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                .WithMessage($"'{nameof(Domain.Models.Employer.Owner)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }
    }
}