using FluentAssertions;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderQualification.Validation
{
    public class When_ProviderQualification_Row_UkPrn_Missing : IClassFixture<ProviderQualificationFileImportValidationTestFixture>
    {
        private readonly ProviderQualificationFileImportValidationTestFixture _fixture;

        public When_ProviderQualification_Row_UkPrn_Missing(ProviderQualificationFileImportValidationTestFixture fixture)
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
            validationResult.Errors[0].ErrorMessage.Should().Be($"'{nameof(ProviderQualificationFileImportDto.UkPrn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }
    }
}