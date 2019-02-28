using System;
using System.Linq.Expressions;
using FluentAssertions;
using Humanizer;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderQualification.Validation
{
    public class When_ProviderQualification_Row_Postcode_Has_Invalid_Format : IClassFixture<ProviderQualificationFileImportValidationTestFixture>
    {
        private readonly ProviderQualificationFileImportValidationTestFixture _fixture;

        public When_ProviderQualification_Row_Postcode_Has_Invalid_Format(ProviderQualificationFileImportValidationTestFixture fixture)
        {
            _fixture = fixture;
            fixture.ProviderVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new Domain.Models.ProviderVenue
                {
                    Id = 1,
                    Source = "Test"
                });
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
                .Be($"'{nameof(ProviderQualificationFileImportDto.PostCode)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");
        }
    }
}