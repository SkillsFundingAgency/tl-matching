using FluentAssertions;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.PostcodeLookup.Validation
{
    public class When_PostcodeLookupStaging_Row_Has_No_LepCode : IClassFixture<PostcodeLookupStagingFileImportFixture>
    {
        private readonly PostcodeLookupStagingFileImportFixture _fixture;

        public When_PostcodeLookupStaging_Row_Has_No_LepCode(PostcodeLookupStagingFileImportFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Then_Validation_Result_Is_Valid(string primaryLepCode)
        {
            _fixture.Dto.PrimaryLepCode = primaryLepCode;
            var validationResult = _fixture.Validator.Validate(_fixture.Dto);
            validationResult.IsValid.Should().BeTrue();
            validationResult.Errors.Count.Should().Be(0);
        }
    }
}