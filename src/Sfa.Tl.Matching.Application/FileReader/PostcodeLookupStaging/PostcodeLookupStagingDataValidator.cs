using FluentValidation;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.PostcodeLookupStaging
{
    public class PostcodeLookupStagingDataValidator : AbstractValidator<PostcodeLookupStagingFileImportDto>
    {
        public PostcodeLookupStagingDataValidator()
        {
            RuleFor(dto => dto.Postcode)
                .NotEmpty()
                .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
                .WithMessage($"'{nameof(PostcodeLookupStagingFileImportDto.Postcode)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
            //.Matches(dto => ValidationConstants.UkPostcodeRegex)
            //.WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
            //.WithMessage($"'{nameof(PostcodeLookupStagingFileImportDto.Postcode)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            //No rule for SecondaryLepCode - it isn't consistently present in file
            //RuleFor(dto => dto.PrimaryLepCode)
            //    .NotEmpty()
            //    .WithErrorCode(ValidationErrorCode.MissingMandatoryData.ToString())
            //    .WithMessage($"'{nameof(PostcodeLookupStagingFileImportDto.PrimaryLepCode)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}")
            //    .Length(9)
            //    .WithErrorCode(ValidationErrorCode.InvalidFormat.ToString())
            //    .WithMessage($"'{nameof(PostcodeLookupStagingFileImportDto.PrimaryLepCode)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");

            //Also no rule for SecondaryLepCode
        }
    }
}
