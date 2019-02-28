using System;
using System.Linq.Expressions;
using FluentAssertions;
using Humanizer;
using NSubstitute;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Validation
{
    public class When_ProviderVenue_Row_UkPrn_Has_Invalid_Format : IClassFixture<ProviderVenueFileImportValidationTestFixture>
    {
        private readonly ProviderVenueFileImportValidationTestFixture _fixture;

        public When_ProviderVenue_Row_UkPrn_Has_Invalid_Format(ProviderVenueFileImportValidationTestFixture fixture)
        {
            _fixture = fixture;
            fixture.ProviderRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Provider, bool>>>())
                .Returns(new Domain.Models.Provider
                {
                    Id = 1,
                    UkPrn = long.Parse(ValidProviderVenueFileImportDtoBuilder.UkPrn),
                    Source = "Test"
                });
        }

        [Theory]
        [InlineData("159856587455885")]
        public void Then_Validation_Result_Is_Not_Valid(string ukPrn)
        {
            _fixture.Dto.UkPrn = ukPrn;
            var validationResult = _fixture.Validator.Validate(_fixture.Dto);
            validationResult.IsValid.Should().BeFalse();
            validationResult.Errors.Count.Should().Be(1);
            validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.InvalidFormat.ToString());
            validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(ProviderVenueFileImportDto.UkPrn)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");
        }
    }
}