using System;
using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Validation
{
    public class When_ProviderVenue_Row_Has_Venue_Not_Unique : IClassFixture<ProviderVenueFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_ProviderVenue_Row_Has_Venue_Not_Unique(ProviderVenueFileImportValidationTestFixture fixture)
        {
            fixture.ProviderVenueRepository.GetSingleOrDefault(Arg.Any<Func<Domain.Models.ProviderVenue, bool>>())
                .Returns(new Domain.Models.ProviderVenue
                {
                    Id = 1,
                    ProviderId = 1,
                    Postcode = "CV1 2WT",
                    Source = "Test"
                });

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
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Code_Is_VenueAlreadyExists() =>
            _validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.VenueAlreadyExists.ToString());

        [Fact]
        public void Then_Error_Message_Is_VenueAlreadyExists() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(ProviderVenueFileImportDto.PostCode)}' - {ValidationErrorCode.VenueAlreadyExists.Humanize()}");
    }
}