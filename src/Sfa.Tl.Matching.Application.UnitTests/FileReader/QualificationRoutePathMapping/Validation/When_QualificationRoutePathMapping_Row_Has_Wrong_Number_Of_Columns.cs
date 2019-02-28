using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_QualificationRoutePathMapping_Row_Has_Wrong_Number_Of_Columns : IClassFixture<QualificationRoutePathMappingFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_QualificationRoutePathMapping_Row_Has_Wrong_Number_Of_Columns(QualificationRoutePathMappingFileImportValidationTestFixture fixture)
        {
            fixture.Dto.Accountancy = null;
            fixture.Dto.AgricultureLandManagementandProduction = null;
            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_Two() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Code_Is_WrongNumberOfColumns() =>
            _validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.WrongNumberOfColumns.ToString());

        [Fact]
        public void Then_Error_Message_Is_WrongNumberOfColumns() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be(ValidationErrorCode.WrongNumberOfColumns.Humanize());
    }
}