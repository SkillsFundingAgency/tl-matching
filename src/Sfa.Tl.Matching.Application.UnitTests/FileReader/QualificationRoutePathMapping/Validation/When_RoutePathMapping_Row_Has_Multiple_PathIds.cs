using FluentAssertions;
using FluentValidation.Results;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Constants;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Has_Multiple_PathIds : IClassFixture<QualificationRoutePathMappingFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;
        
        public When_RoutePathMapping_Row_Has_Multiple_PathIds(QualificationRoutePathMappingFileImportValidationTestFixture fixture)
        {
            fixture.Dto.AgricultureLandManagementandProduction = RoutePathMappingConstants.AgricultureLandManagementandProduction;
            fixture.Dto.AnimalCareandManagement = RoutePathMappingConstants.AnimalCareandManagement;
            fixture.Dto.Hospitality = RoutePathMappingConstants.Hospitality;

            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Valid() =>
            _validationResult.IsValid.Should().BeTrue();
    }
}