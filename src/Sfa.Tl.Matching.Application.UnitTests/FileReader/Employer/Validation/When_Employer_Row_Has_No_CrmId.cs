using FluentAssertions;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Validation
{
    public class When_Employer_Row_Has_No_CrmId : IClassFixture<EmployerFileImportFixture>
    {
        private readonly EmployerFileImportFixture _fixture;

        public When_Employer_Row_Has_No_CrmId(EmployerFileImportFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Then_Validation_Result_Is_Not_Valid(string crmId)
        {
            _fixture.Dto.CrmId = crmId;
            var validationResult = _fixture.Validator.Validate(_fixture.Dto);
            Assert.False(validationResult.IsValid);
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.MissingMandatoryData.ToString());
            validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(EmployerFileImportDto.CrmId)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }
    }
}