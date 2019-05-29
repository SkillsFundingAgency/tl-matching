using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Validation
{
    public class When_EmployerStaging_Row_CompanyType_Is_Null_Or_Empty_Or_WhiteSpace : IClassFixture<EmployerStagingFileImportFixture>
    {
        private readonly EmployerStagingFileImportFixture _fixture;

        public When_EmployerStaging_Row_CompanyType_Is_Null_Or_Empty_Or_WhiteSpace(EmployerStagingFileImportFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public void Then_Validation_Result_Is_Not_Valid(string companyType)
        {
            _fixture.Dto.CompanyType = companyType;
            
            var validationResult = _fixture.Validator.Validate(_fixture.Dto);
            
            validationResult.IsValid.Should().BeTrue();
            
            validationResult.Errors.Count.Should().Be(0);
        }
    }
}