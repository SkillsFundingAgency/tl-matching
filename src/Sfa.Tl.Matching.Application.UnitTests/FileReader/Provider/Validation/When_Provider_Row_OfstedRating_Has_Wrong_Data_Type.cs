using System.Linq;
using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class When_Provider_Row_OfstedRating_Has_Wrong_Data_Type : IClassFixture<ProviderFileImportFixture>
    {
        private readonly ValidationResult _validationResult;
        public When_Provider_Row_OfstedRating_Has_Wrong_Data_Type(ProviderFileImportFixture fixture)
        {
            fixture.ProviderFileImportDto.OfstedRating = "A";

            _validationResult = fixture.ProviderDataValidator.Validate(fixture.ProviderFileImportDto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Code_Is_WrongDataType() =>
            _validationResult.Errors.First(e => e.ErrorCode == ValidationErrorCode.WrongDataType.ToString()).ErrorCode.Should()
                .Be(ValidationErrorCode.WrongDataType.ToString());

        [Fact]
        public void Then_Error_Message_Is_WrongDataType() =>
            _validationResult.Errors.First(e => e.ErrorCode == ValidationErrorCode.WrongDataType.ToString()).ErrorMessage.Should()
                .Be($"'{nameof(ProviderFileImportDto.OfstedRating)}' - {ValidationErrorCode.WrongDataType.Humanize()}");
    }
}