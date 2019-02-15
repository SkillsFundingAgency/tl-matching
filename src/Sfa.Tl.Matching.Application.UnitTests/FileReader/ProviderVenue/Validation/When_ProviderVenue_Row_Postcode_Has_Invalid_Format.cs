using FluentAssertions;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Validation
{
    public class When_ProviderVenue_Row_Postcode_Has_Invalid_Format : IClassFixture<ProviderVenueFileImportFixture>
    {
        private readonly ProviderVenueFileImportFixture _fixture;

        public When_ProviderVenue_Row_Postcode_Has_Invalid_Format(ProviderVenueFileImportFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("3A4FF")]
        [InlineData("A3FG43")]
        public void Then_Validation_Result_Is_Not_Valid(string postcode)
        {
            _fixture.Dto.PostCode = postcode;
            var validationResult = _fixture.Validator.Validate(_fixture.Dto);
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.InvalidFormat.ToString());
            validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(ProviderVenueFileImportDto.PostCode)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");
        }
    }
}