using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_QualificationRoutePathMapping_Row_Is_Valid : IClassFixture<QualificationRoutePathMappingFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;
        
        public When_QualificationRoutePathMapping_Row_Is_Valid(QualificationRoutePathMappingFileImportValidationTestFixture fixture)
        {
            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Valid() =>
            _validationResult.IsValid.Should().BeTrue();

        [Fact]
        public void Then_Error_Count_Is_Zero() =>
            _validationResult.Errors.Should().BeEmpty();
    }
}