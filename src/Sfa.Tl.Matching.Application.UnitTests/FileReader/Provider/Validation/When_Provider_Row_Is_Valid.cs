using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class When_Provider_Row_Is_Valid : IClassFixture<ProviderFileImportFixture>
    {
        private readonly ValidationResult _validationResult;
        public When_Provider_Row_Is_Valid(ProviderFileImportFixture fixture)
        {
            _validationResult = fixture.ProviderDataValidator.Validate(fixture.ProviderFileImportDto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Valid() =>
            _validationResult.IsValid.Should().BeTrue();

        [Fact]
        public void Then_Error_Count_Is_Zero() =>
            _validationResult.Errors.Count.Should().Be(0);
    }
}