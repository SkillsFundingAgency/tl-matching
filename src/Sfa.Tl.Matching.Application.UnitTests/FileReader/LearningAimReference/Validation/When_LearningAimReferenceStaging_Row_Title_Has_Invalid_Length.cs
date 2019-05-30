using FluentAssertions;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimReference.Validation
{
    public class When_LearningAimReferenceStaging_Row_Title_Has_Invalid_Length : IClassFixture<LearningAimReferenceStagingFileImportFixture>
    {
        private readonly LearningAimReferenceStagingFileImportFixture _fixture;

        public When_LearningAimReferenceStaging_Row_Title_Has_Invalid_Length(LearningAimReferenceStagingFileImportFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid()
        {
            _fixture.Dto.Title = new string('*', 401);
            
            var validationResult = _fixture.Validator.Validate(_fixture.Dto);
            
            validationResult.IsValid.Should().BeFalse();
            
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors[0].ErrorCode.Should().Be(ValidationErrorCode.InvalidLength.ToString());
            validationResult.Errors[0].ErrorMessage.Should().Be($"'{nameof(LearningAimReferenceStagingFileImportDto.Title)}' - {ValidationErrorCode.InvalidLength.Humanize()}");
        }
    }
}