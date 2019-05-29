using FluentAssertions;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Employer.Validation
{
    public class When_EmployerStaging_Row_AupaStatus_Has_Invalid_Format : IClassFixture<EmployerStagingFileImportFixture>
    {
        private readonly EmployerStagingFileImportFixture _fixture;

        public When_EmployerStaging_Row_AupaStatus_Has_Invalid_Format(EmployerStagingFileImportFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid()
        {
            _fixture.Dto.Aupa = "ABC";
            
            var validationResult = _fixture.Validator.Validate(_fixture.Dto);
            
            validationResult.IsValid.Should().BeFalse();
            
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors[0].ErrorCode.Should().Be(ValidationErrorCode.InvalidFormat.ToString());
            validationResult.Errors[0].ErrorMessage.Should().Be($"'{nameof(EmployerStagingFileImportDto.Aupa)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");
        }
    }
}