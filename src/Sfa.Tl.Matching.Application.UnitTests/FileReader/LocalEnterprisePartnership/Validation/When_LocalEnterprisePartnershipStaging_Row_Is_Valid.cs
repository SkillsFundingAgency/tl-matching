using FluentAssertions;
using FluentValidation.Results;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LocalEnterprisePartnership.Validation
{
    public class When_LocalEnterprisePartnershipStaging_Row_Is_Valid : IClassFixture<LocalEnterprisePartnershipStagingFileImportFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_LocalEnterprisePartnershipStaging_Row_Is_Valid(LocalEnterprisePartnershipStagingFileImportFixture fixture)
        {
            fixture.Dto.Code = "L00000001";
            fixture.Dto.Name = "Valid name";

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