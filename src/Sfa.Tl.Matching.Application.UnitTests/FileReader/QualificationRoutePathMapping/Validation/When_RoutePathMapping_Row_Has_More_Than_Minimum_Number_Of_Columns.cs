using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Has_More_Than_Minimum_Number_Of_Columns : IClassFixture<QualificationRoutePathMappingFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;
        
        public When_RoutePathMapping_Row_Has_More_Than_Minimum_Number_Of_Columns(QualificationRoutePathMappingFileImportValidationTestFixture fixture)
        {
            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Valid() =>
            _validationResult.IsValid.Should().BeTrue();
    }
}