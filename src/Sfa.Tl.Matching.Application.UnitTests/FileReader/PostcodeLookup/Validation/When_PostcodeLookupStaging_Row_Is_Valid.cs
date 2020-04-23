using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.PostcodeLookup.Validation
{
    public class When_PostcodeLookupStaging_Row_Is_Valid : IClassFixture<PostcodeLookupStagingFileImportFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_PostcodeLookupStaging_Row_Is_Valid(PostcodeLookupStagingFileImportFixture fixture)
        {
            fixture.Dto.Postcode = "CV1 2WT";
            fixture.Dto.LepCode = "L00000001";

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