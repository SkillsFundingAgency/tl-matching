using FluentAssertions;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Validation
{
    public class When_ProviderVenue_Row_Has_No_UkPrn : IClassFixture<ProviderVenueFileImportValidationTestFixture>
    {
        private readonly ProviderVenueFileImportValidationTestFixture _fixture;

        public When_ProviderVenue_Row_Has_No_UkPrn(ProviderVenueFileImportValidationTestFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Then_Validation_Result_Is_Not_Valid(string ukPrn)
        {
            _fixture.Dto.UkPrn = ukPrn;
            var validationResult = _fixture.Validator.Validate(_fixture.Dto);
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors[0].ErrorCode.Should().Be(ValidationErrorCode.MissingMandatoryData.ToString());
            validationResult.Errors[0].ErrorMessage.Should().Be($"'{nameof(ProviderVenueFileImportDto.UkPrn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }
    }
}