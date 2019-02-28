using System.Threading.Tasks;
using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.Provider
{
    public class ProviderDataValidator : AbstractValidator<ProviderFileImportDto>
    {
        public ProviderDataValidator(IRepository<Domain.Models.Provider> repository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(dto => dto.UkPrn)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.UkPrn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Matches(ValidationConstants.UkprnRegex)
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.UkPrn)}' - {ValidationErrorCode.InvalidFormat.Humanize()}")
                .MustAsync((x, cancellation) => HaveUniqueProvider(repository, x))
                    .WithErrorCode(ValidationErrorCode.ProviderAlreadyExists.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.UkPrn)}' - {ValidationErrorCode.ProviderAlreadyExists.Humanize()}");

            RuleFor(dto => dto.ProviderName)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.ProviderName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.OfstedRating)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.OfstedRating)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsOfstedRating())
                    .WithErrorCode(ValidationErrorCode.WrongDataType.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.OfstedRating)}' - {ValidationErrorCode.WrongDataType.Humanize()}");

            RuleFor(dto => dto.Status)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.Status)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsYesNo())
                    .WithErrorCode(ValidationErrorCode.WrongDataType.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.Status)}' - {ValidationErrorCode.WrongDataType.Humanize()}");

            RuleFor(dto => dto.PrimaryContactName)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.PrimaryContactName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.PrimaryContactTelephone)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.PrimaryContactTelephone)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.PrimaryContactEmail)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.PrimaryContactEmail)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .EmailAddress();

            RuleFor(dto => dto.SecondaryContactName)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.SecondaryContactName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.SecondaryContactTelephone)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.SecondaryContactTelephone)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.SecondaryContactEmail)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.SecondaryContactEmail)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .EmailAddress();

            RuleFor(dto => dto.Source)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.Source)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }

        private async Task<bool> HaveUniqueProvider(IRepository<Domain.Models.Provider> repository, string ukPrn)
        {
            var result = int.TryParse(ukPrn, out var ukprnNumber);

            if (!result) return false;

            var provider = await repository.GetSingleOrDefault(p => p.UkPrn == ukprnNumber);

            return provider == null;
        }
    }
}