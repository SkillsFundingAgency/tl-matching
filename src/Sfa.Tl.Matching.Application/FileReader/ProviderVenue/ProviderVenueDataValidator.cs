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
        public ProviderVenueDataValidator(IRepository<Domain.Models.Provider> repository, IRepository<Domain.Models.ProviderVenue> venueRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(dto => dto.UkPrn)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderVenueFileImportDto.UkPrn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Matches(ValidationConstants.UkprnRegex)
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(ProviderVenueFileImportDto.UkPrn)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto)
                .MustAsync((dto, cancellation) => ProviderMustExistsForVenue(repository, dto))
                     .When(pv => !string.IsNullOrEmpty(pv.UkPrn))
                     .WithErrorCode(ValidationErrorCode.ProviderDoesntExist.ToString())
                     .WithMessage($"'{nameof(ProviderVenueFileImportDto.UkPrn)}' - {ValidationErrorCode.ProviderDoesntExist.Humanize()}");

            RuleFor(dto => dto.PostCode)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderVenueFileImportDto.PostCode)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Matches(dto => ValidationConstants.UkPostCodeRegex)
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(ProviderVenueFileImportDto.PostCode)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            RuleFor(dto => dto)
                 .MustAsync((dto, cancellation) => ProviderVenueMustBeUnique(venueRepository, dto))
                     .WithErrorCode(ValidationErrorCode.ProviderVenueAlreadyExists.ToString())
                     .WithMessage($"'{nameof(ProviderVenueFileImportDto.PostCode)}' - {ValidationErrorCode.ProviderVenueAlreadyExists.Humanize()}");

            RuleFor(dto => dto.Source)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderVenueFileImportDto.Source)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }

        private async Task<bool> ProviderMustExistsForVenue(IRepository<Domain.Models.Provider> providerRepository, ProviderVenueFileImportDto dto)
        {
            var result = long.TryParse(dto.UkPrn, out var ukPrn);

            if (!result) return false;

            var provider = await providerRepository.GetSingleOrDefault(p => p.UkPrn == ukPrn);

            if (provider == null) return false;

            //NOTE: This hack is because of the Generic implementation of data validator and data parser
            dto.ProviderId = provider.Id;

            return true;
        }

        private async Task<bool> ProviderVenueMustBeUnique(IRepository<Domain.Models.ProviderVenue> providerVenueRepository, ProviderVenueFileImportDto dto)
        {
            if (dto.ProviderId == 0) return false;

            var venue = await providerVenueRepository.GetSingleOrDefault(v =>
                v.ProviderId == dto.ProviderId
             && v.Postcode == dto.PostCode);

            return venue == null;
        }
    }
}