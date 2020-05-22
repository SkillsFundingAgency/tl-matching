using FluentAssertions;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.PostcodeLookup.Validation
{
    public class When_PostcodeLookupStaging_Row_Has_No_Postcode : IClassFixture<PostcodeLookupStagingFileImportFixture>
    {
        private readonly PostcodeLookupStagingFileImportFixture _fixture;
        public When_PostcodeLookupStaging_Row_Has_No_Postcode(PostcodeLookupStagingFileImportFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData(" ")]
        [InlineData("")]
        [InlineData(null)]
        public void Then_Validation_Result_Is_Not_Valid(string postcode)
        {
            _fixture.Dto.Postcode = postcode;
            
            var validationResult = _fixture.Validator.Validate(_fixture.Dto);
            
            validationResult.IsValid.Should().BeFalse();
            
            validationResult.Errors.Count.Should().BeGreaterOrEqualTo(1);
            validationResult.Errors[0].ErrorCode.Should().Be(ValidationErrorCode.MissingMandatoryData.ToString());
            validationResult.Errors[0].ErrorMessage.Should().Be($"'{nameof(PostcodeLookupStagingFileImportDto.Postcode)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }
    }
}