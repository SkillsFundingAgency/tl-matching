using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.Employer
{
    public class EmployerDataValidator : AbstractValidator<EmployerFileImportDto>
    {
        public EmployerDataValidator()
        {
            When(dto => dto != null, () =>
            {
                RuleFor(dto => dto.CrmId)
                    .NotNull()
                        .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                        .WithMessage($"'{nameof(EmployerFileImportDto.CrmId)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                    .NotEmpty()
                        .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                        .WithMessage($"'{nameof(EmployerFileImportDto.CrmId)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                    .Must(dto => dto.IsGuid())
                        .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                        .WithMessage($"'{nameof(EmployerFileImportDto.CrmId)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

                RuleFor(dto => dto.CompanyName)
                    .NotNull()
                        .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                        .WithMessage($"'{nameof(EmployerFileImportDto.CompanyName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                    .NotEmpty()
                        .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                        .WithMessage($"'{nameof(EmployerFileImportDto.CompanyName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

                RuleFor(dto => dto.Aupa)
                    .NotNull()
                        .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                        .WithMessage($"'{nameof(EmployerFileImportDto.Aupa)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                    .NotEmpty()
                        .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                        .WithMessage($"'{nameof(EmployerFileImportDto.Aupa)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

                RuleFor(dto => dto.PostCode)
                    .NotNull()
                        .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                        .WithMessage($"'{nameof(EmployerFileImportDto.PostCode)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                    .NotEmpty()
                        .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                        .WithMessage($"'{nameof(EmployerFileImportDto.PostCode)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

                RuleFor(dto => dto.Owner)
                    .NotNull()
                        .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                        .WithMessage($"'{nameof(EmployerFileImportDto.Owner)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                    .NotEmpty()
                        .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                        .WithMessage($"'{nameof(EmployerFileImportDto.Owner)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
            });
        }
    }
}
