using System.Threading.Tasks;
using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.ProviderVenue
{
    public class ProviderVenueDataValidator : AbstractValidator<ProviderVenueFileImportDto>
    {
        public ProviderVenueDataValidator(IRepository<Domain.Models.Provider> repository)
        {
            RuleFor(dto => dto)
                .MustAsync((dto, cancellation) => ProviderMustExistsForVenue(repository, dto))
                    .WithErrorCode(ValidationErrorCode.RecordAlreadyExists.ToString())
                    .WithMessage($"'{nameof(ProviderVenueFileImportDto.UkPrn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");

            RuleFor(dto => dto.PostCode)
                .Matches(dto => ValidationConstants.UkPostCodeRegex)
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(ProviderVenueFileImportDto.PostCode)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto.Source)
                .NotNull().NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderVenueFileImportDto.Source)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }

        private async Task<bool> ProviderMustExistsForVenue(IRepository<Domain.Models.Provider> repository, ProviderVenueFileImportDto dto)
        {
            var result = int.TryParse(dto.UkPrn, out var ukPrn);

            if (!result) return false;

            var provider = await repository.GetSingleOrDefault(p => p.UkPrn == ukPrn);

            if (provider == null) return false;

            //NOTE: This hack is because of the Generic implementation of data validator and data parser
            dto.ProviderId = provider.Id;

            return true;
        }
    }
}