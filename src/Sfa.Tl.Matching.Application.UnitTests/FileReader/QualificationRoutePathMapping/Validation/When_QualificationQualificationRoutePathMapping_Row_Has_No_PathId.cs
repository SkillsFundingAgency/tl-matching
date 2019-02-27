using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_QualificationQualificationRoutePathMapping_Row_Has_No_PathId : IClassFixture<QualificationRoutePathMappingFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;
        
        public When_QualificationQualificationRoutePathMapping_Row_Has_No_PathId(QualificationRoutePathMappingFileImportValidationTestFixture fixture)
        {
            fixture.Dto.Accountancy = null;
            fixture.Dto.AgricultureLandManagementandProduction = null;

            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_InValid() =>
            _validationResult.IsValid.Should().BeFalse();
    }
}