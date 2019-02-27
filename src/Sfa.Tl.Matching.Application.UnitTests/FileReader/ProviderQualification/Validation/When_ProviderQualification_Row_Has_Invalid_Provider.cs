using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderQualification.Validation
{
    public class When_ProviderQualification_Row_Has_Invalid_ProviderVenue : IClassFixture<ProviderQualificationFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_ProviderQualification_Row_Has_Invalid_ProviderVenue(ProviderQualificationFileImportValidationTestFixture fixture)
        {
            fixture.ProviderVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns((Domain.Models.ProviderVenue)null);

            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Code_Is_ProviderVenueDoesntExist() =>
            _validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.ProviderVenueDoesntExist.ToString());

        [Fact]
        public void Then_Error_Message_Is_ProviderVenueDoesntExist() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(ProviderQualificationFileImportDto.UkPrn)}' - {ValidationErrorCode.ProviderVenueDoesntExist.Humanize()}");
    }
}