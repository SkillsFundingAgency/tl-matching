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
            RuleFor(dto => dto.UkPrn)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.UkPrn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.UkPrn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Matches(ValidationConstants.UkprnRegex)
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.UkPrn)}' - {ValidationErrorCode.InvalidFormat.Humanize()}")
                .MustAsync((x, cancellation) => CanUkprnBeAdded(repository, x))
                    .WithErrorCode(ValidationErrorCode.RecordAlreadyExists.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.UkPrn)}' - {ValidationErrorCode.RecordAlreadyExists.Humanize()}");

            RuleFor(dto => dto.ProviderName)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.ProviderName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.ProviderName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.OfstedRating)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.OfstedRating)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.OfstedRating)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsOfstedRating())
                    .WithErrorCode(ValidationErrorCode.WrongDataType.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.OfstedRating)}' - {ValidationErrorCode.WrongDataType.Humanize()}");

            RuleFor(dto => dto.Status)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.Status)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.Status)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Must(dto => dto.IsYesNo())
                    .WithErrorCode(ValidationErrorCode.WrongDataType.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.Status)}' - {ValidationErrorCode.WrongDataType.Humanize()}");

            RuleFor(dto => dto.PrimaryContactName)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.PrimaryContactName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.PrimaryContactName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.PrimaryContactTelephone)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.PrimaryContactTelephone)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.PrimaryContactTelephone)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.PrimaryContactEmail)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.PrimaryContactEmail)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.PrimaryContactEmail)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .EmailAddress();

            RuleFor(dto => dto.SecondaryContactName)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.SecondaryContactName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.SecondaryContactName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.SecondaryContactTelephone)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.SecondaryContactTelephone)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.SecondaryContactTelephone)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.SecondaryContactEmail)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.SecondaryContactEmail)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.SecondaryContactEmail)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .EmailAddress();

            RuleFor(dto => dto.Source)
                .NotNull()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.Source)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderFileImportDto.Source)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }

        private async Task<bool> CanUkprnBeAdded(IRepository<Domain.Models.Provider> repository, string ukPrn)
        {
            var provider = await repository.GetSingleOrDefault(p => p.UkPrn == int.Parse(ukPrn));

            return provider == null;
        }
    }
}