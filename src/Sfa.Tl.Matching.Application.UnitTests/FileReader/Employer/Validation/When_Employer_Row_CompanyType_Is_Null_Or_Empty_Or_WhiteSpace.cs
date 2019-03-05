using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Validation
{
    public class When_Employer_Row_CompanyType_Is_Null_Or_Empty_Or_WhiteSpace : IClassFixture<EmployerFileImportFixture>
    {
        private readonly EmployerFileImportFixture _fixture;

        public When_Employer_Row_CompanyType_Is_Null_Or_Empty_Or_WhiteSpace(EmployerFileImportFixture fixture)
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