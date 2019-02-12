using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_ShortTitle_Has_Invalid_Length : IClassFixture<QualificationRoutePathMappingFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_RoutePathMapping_Row_ShortTitle_Has_Invalid_Length(QualificationRoutePathMappingFileImportValidationTestFixture fixture)
        {
            fixture.Dto.ShortTitle = new string('X', 101);
            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Code_Is_InvalidLength() =>
            _validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.InvalidLength.ToString());

        [Fact]
        public void Then_Error_Message_Is_InvalidLength() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(QualificationRoutePathMappingFileImportDto.ShortTitle)}' - {ValidationErrorCode.InvalidLength.Humanize()}");
    }
}