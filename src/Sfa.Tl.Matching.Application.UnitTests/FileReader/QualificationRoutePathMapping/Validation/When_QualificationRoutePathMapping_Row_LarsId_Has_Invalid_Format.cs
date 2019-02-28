using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_QualificationRoutePathMapping_Row_LarsId_Has_Invalid_Format : IClassFixture<QualificationRoutePathMappingFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_QualificationRoutePathMapping_Row_LarsId_Has_Invalid_Format(QualificationRoutePathMappingFileImportValidationTestFixture fixture)
        {
            fixture.Dto.LarsId = "12345";

            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Code_Is_InvalidFormat() =>
            _validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.InvalidFormat.ToString());

        [Fact]
        public void Then_Error_Message_Is_InvalidFormat() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(QualificationRoutePathMappingFileImportDto.LarsId)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");
    }
}