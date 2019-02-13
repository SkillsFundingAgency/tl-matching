using System;
using FluentAssertions;
using FluentValidation.Results;
using NSubstitute;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Builders;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Validation
{
    public class When_ProviderVenue_Row_Is_Valid : IClassFixture<ProviderVenueFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;
        
        public When_ProviderVenue_Row_Is_Valid(ProviderVenueFileImportValidationTestFixture fixture)
        {
            fixture.ProviderRepository.GetSingleOrDefault(Arg.Any<Func<Domain.Models.Provider, bool>>())
                .Returns(new Domain.Models.Provider
                {
                    Id = 1,
                    UkPrn = long.Parse(ValidProviderVenueFileImportDtoBuilder.UkPrn),
                    Source = "Test"
                });

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