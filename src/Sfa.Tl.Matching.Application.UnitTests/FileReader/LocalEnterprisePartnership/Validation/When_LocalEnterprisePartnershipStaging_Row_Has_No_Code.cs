using FluentAssertions;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LocalEnterprisePartnership.Validation
{
    public class When_LocalEnterprisePartnershipStaging_Row_Has_No_Code : IClassFixture<LocalEnterprisePartnershipStagingFileImportFixture>
    {
        private readonly LocalEnterprisePartnershipStagingFileImportFixture _fixture;

        public When_LocalEnterprisePartnershipStaging_Row_Has_No_Code(LocalEnterprisePartnershipStagingFileImportFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Then_Validation_Result_Is_Not_Valid(string code)
        {
            _fixture.Dto.Code = code;
            var validationResult = _fixture.Validator.Validate(_fixture.Dto);
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(2);
            validationResult.Errors[0].ErrorCode.Should().Be(ValidationErrorCode.MissingMandatoryData.ToString());
            validationResult.Errors[0].ErrorMessage.Should().Be($"'{nameof(LocalEnterprisePartnershipStagingFileImportDto.Code)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }
    }
}