using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Validation
{
    public class When_ProviderVenue_Row_Has_Invalid_Provider : IClassFixture<ProviderVenueFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_ProviderVenue_Row_Has_Invalid_Provider(ProviderVenueFileImportValidationTestFixture fixture)
        {
            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Code_Is_ProviderDoesntExist() =>
            _validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.ProviderDoesntExist.ToString());

        [Fact]
        public void Then_Error_Message_Is_ProviderDoesntExist() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(ProviderVenueFileImportDto.UkPrn)}' - {ValidationErrorCode.ProviderDoesntExist.Humanize()}");
    }
}