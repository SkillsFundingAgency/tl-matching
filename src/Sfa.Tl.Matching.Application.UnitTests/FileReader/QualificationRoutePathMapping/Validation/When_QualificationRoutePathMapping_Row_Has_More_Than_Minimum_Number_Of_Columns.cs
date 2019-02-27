using FluentAssertions;
using FluentValidation.Results;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_QualificationRoutePathMapping_Row_Has_More_Than_Minimum_Number_Of_Columns : IClassFixture<QualificationRoutePathMappingFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;
        
        public When_QualificationRoutePathMapping_Row_Has_More_Than_Minimum_Number_Of_Columns(QualificationRoutePathMappingFileImportValidationTestFixture fixture)
        {
            fixture.Dto.AgricultureLandManagementandProduction = ValidQualificationRoutePathMappingFileImportDtoBuilder.AgricultureLandManagementandProduction;
            fixture.Dto.AnimalCareandManagement = ValidQualificationRoutePathMappingFileImportDtoBuilder.AnimalCareandManagement;
            fixture.Dto.Hospitality = ValidQualificationRoutePathMappingFileImportDtoBuilder.Hospitality;

            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Valid() =>
            _validationResult.IsValid.Should().BeTrue();
    }
}