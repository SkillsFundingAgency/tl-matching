using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class When_Provider_Row_Has_No_Name : IClassFixture<ProviderFileImportFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_Provider_Row_Has_No_Name(ProviderFileImportFixture fixture)
        {
            fixture.ProviderFileImportDto.ProviderName = null;

            _validationResult = fixture.ProviderDataValidator.Validate(fixture.ProviderFileImportDto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Code_Is_MissingMandatoryData() =>
            _validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.MissingMandatoryData.ToString());

        [Fact]
        public void Then_Error_Message_Is_MissingMandatoryData() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(ProviderFileImportDto.ProviderName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
    }
}