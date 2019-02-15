using FluentAssertions;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Validation
{
    public class When_ProviderVenue_Row_Has_No_Source : IClassFixture<ProviderVenueFileImportFixture>
    {
        private readonly ProviderVenueFileImportFixture _fixture;

        public When_ProviderVenue_Row_Has_No_Source(ProviderVenueFileImportFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Then_Validation_Result_Is_Not_Valid(string source)
        {
            _fixture.Dto.Source = source;
            var validationResult = _fixture.Validator.Validate(_fixture.Dto);
            Assert.False(validationResult.IsValid);
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.MissingMandatoryData.ToString());
            validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(ProviderVenueFileImportDto.Source)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }
    }
}