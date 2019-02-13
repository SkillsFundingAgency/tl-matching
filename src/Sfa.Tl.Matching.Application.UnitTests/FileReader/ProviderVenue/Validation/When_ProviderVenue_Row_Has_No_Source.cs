using System;
using FluentAssertions;
using Humanizer;
using NSubstitute;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Validation
{
    public class When_ProviderVenue_Row_Has_No_Source : IClassFixture<ProviderVenueFileImportValidationTestFixture>
    {
        private readonly ProviderVenueFileImportValidationTestFixture _fixture;

        public When_ProviderVenue_Row_Has_No_Source(ProviderVenueFileImportValidationTestFixture fixture)
        {
            _fixture = fixture;
            fixture.ProviderRepository.GetSingleOrDefault(Arg.Any<Func<Domain.Models.Provider, bool>>())
                .Returns(new Domain.Models.Provider
                {
                    Id = 1,
                    UkPrn = long.Parse(ValidProviderVenueFileImportDtoBuilder.UkPrn),
                    Source = "Test"
                });
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