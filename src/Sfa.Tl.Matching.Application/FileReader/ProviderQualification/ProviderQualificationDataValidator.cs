using System.Threading.Tasks;
using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.ProviderQualification
{
    public class ProviderQualificationDataValidator : AbstractValidator<ProviderQualificationFileImportDto>
    {
        public ProviderQualificationDataValidator(IRepository<Domain.Models.ProviderVenue> providerVenueRepository, IRepository<Domain.Models.ProviderQualification> providerQualificationRepository, IRepository<Qualification> qualificationRepository)
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;

            RuleFor(dto => dto.UkPrn)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderQualificationFileImportDto.UkPrn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                .Matches(ValidationConstants.UkprnRegex)
                    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                    .WithMessage($"'{nameof(ProviderQualificationFileImportDto.UkPrn)}' - {ValidationErrorCode.InvalidFormat.Humanize()}")
                .DependentRules(() =>
                {
                    RuleFor(dto => dto.PostCode)
                        .NotEmpty()
                            .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                            .WithMessage($"'{nameof(ProviderQualificationFileImportDto.PostCode)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
                        .Matches(dto => ValidationConstants.UkPostCodeRegex)
                            .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
                            .WithMessage($"'{nameof(ProviderQualificationFileImportDto.PostCode)}' - {ValidationErrorCode.InvalidFormat.Humanize()}")
                        .DependentRules(() =>
                        {
                            RuleFor(dto => dto)
                                .MustAsync((dto, cancellation) => HaveExistingVenueForQualification(providerVenueRepository, dto))
                                    .WithErrorCode(ValidationErrorCode.ProviderVenueDoesntExist.ToString())
                                    .WithMessage($"'{nameof(ProviderQualificationFileImportDto.UkPrn)}' - {ValidationErrorCode.ProviderVenueDoesntExist.Humanize()}")
                                .MustAsync((dto, cancellation) => HaveExistingQualification(qualificationRepository, dto))
                                    .WithErrorCode(ValidationErrorCode.QualificationDoesntExist.ToString())
                                    .WithMessage($"'{nameof(ProviderQualificationFileImportDto.LarsId)}' - {ValidationErrorCode.QualificationDoesntExist.Humanize()}")
                                .DependentRules(() =>
                                {
                                    RuleFor(dto => dto)
                                        .MustAsync((dto, cancellation) => HaveUniqueProviderQualification(providerQualificationRepository, dto))
                                            .WithErrorCode(ValidationErrorCode.ProviderQualificationAlreadyExists.ToString())
                                            .WithMessage($"'{nameof(ProviderQualificationFileImportDto.LarsId)}' - {ValidationErrorCode.ProviderQualificationAlreadyExists.Humanize()}");
                                });
                        });
                });

            RuleFor(dto => dto.Source)
                .NotEmpty()
                    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                    .WithMessage($"'{nameof(ProviderQualificationFileImportDto.Source)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }

        private async Task<bool> HaveExistingVenueForQualification(IRepository<Domain.Models.ProviderVenue> providerRepository, ProviderQualificationFileImportDto dto)
        {
            var ukprn = long.Parse(dto.UkPrn);

            var providerVenue = await providerRepository.GetSingleOrDefault(pv => pv.Provider.UkPrn == ukprn && pv.Postcode == dto.PostCode);

            if (providerVenue == null) return false;

            //NOTE: This hack is because of the Generic implementation of data validator and data parser
            dto.ProviderVenueId = providerVenue.Id;

            return true;
        }

        private async Task<bool> HaveExistingQualification(IRepository<Qualification> qualificationRepository, ProviderQualificationFileImportDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.LarsId)) return false;

            var qualification = await qualificationRepository.GetSingleOrDefault(p => p.LarsId == dto.LarsId);

            if (qualification == null) return false;

            //NOTE: This hack is because of the Generic implementation of data validator and data parser
            dto.QualificationId = qualification.Id;

            return true;
        }

        private async Task<bool> HaveUniqueProviderQualification(IRepository<Domain.Models.ProviderQualification> providerQualificationRepository, ProviderQualificationFileImportDto dto)
        {
            var providerQualification = await providerQualificationRepository.GetSingleOrDefault(pq =>
                pq.ProviderVenueId == dto.ProviderVenueId && pq.QualificationId == dto.QualificationId);

            return providerQualification == null;
        }
    }
}