using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimReference.Validation
{
    public class When_LearningAimReferenceStaging_Row_Is_Valid : IClassFixture<LearningAimReferenceStagingFileImportFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_LearningAimReferenceStaging_Row_Is_Valid(LearningAimReferenceStagingFileImportFixture fixture)
        {
            fixture.Dto.LarId = "12345678";
            fixture.Dto.Title = "Valid title";
            fixture.Dto.SourceCreatedOn = "07-Jun-18";
            fixture.Dto.SourceModifiedOn = "07-Jun-18";

            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Valid() =>
            _validationResult.IsValid.Should().BeTrue();

        [Fact]
        public void Then_Error_Count_Is_Zero() =>
            _validationResult.Errors.Should().BeEmpty();
    }
}