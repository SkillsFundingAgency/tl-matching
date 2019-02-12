using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class When_Provider_Row_UkPrn_Has_Invalid_Format : IClassFixture<ProviderFileImportFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_Provider_Row_UkPrn_Has_Invalid_Format(ProviderFileImportFixture fixture)
        {
            fixture.ProviderFileImportDto.UkPrn = "159856587455885";
            _validationResult = fixture.ProviderDataValidator.Validate(fixture.ProviderFileImportDto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Code_Is_InvalidFormat() =>
            _validationResult.Errors[0].ErrorCode.Should().Be(ValidationErrorCode.InvalidFormat.ToString());

        [Fact]
        public void Then_Error_Message_Is_InvalidFormat() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(Domain.Models.Provider.UkPrn)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");
    }
}