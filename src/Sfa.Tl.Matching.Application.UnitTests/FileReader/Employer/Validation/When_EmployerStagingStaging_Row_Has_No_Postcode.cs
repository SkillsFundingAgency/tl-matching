using FluentAssertions;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Validation
{
    public class When_EmployerStagingStaging_Row_Has_No_Postcode : IClassFixture<EmployerStagingFileImportFixture>
    {
        private readonly EmployerStagingFileImportFixture _fixture;

        public When_EmployerStagingStaging_Row_Has_No_Postcode(EmployerStagingFileImportFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Then_Validation_Result_Is_Not_Valid(string postcode)
        {
            _fixture.Dto.Postcode = postcode;
            var validationResult = _fixture.Validator.Validate(_fixture.Dto);
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors[0].ErrorCode.Should().Be(ValidationErrorCode.MissingMandatoryData.ToString());
            validationResult.Errors[0].ErrorMessage.Should().Be($"'{nameof(EmployerStagingFileImportDto.Postcode)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }
    }
}