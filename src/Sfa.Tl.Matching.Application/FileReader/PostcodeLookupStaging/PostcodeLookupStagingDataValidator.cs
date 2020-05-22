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

            //No rule for Primary/SecondaryLepCode - they aren't consistently present in file
        }
    }
}
