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
    public class When_ProviderQualification_Row_UkPrn_Has_Invalid_Format : IClassFixture<ProviderQualificationFileImportValidationTestFixture>
    {
        private readonly ProviderQualificationFileImportValidationTestFixture _fixture;

        public When_ProviderQualification_Row_UkPrn_Has_Invalid_Format(ProviderQualificationFileImportValidationTestFixture fixture)
        {
            _fixture = fixture;
            fixture.QualificationRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.Qualification, bool>>>())
                .Returns(new Domain.Models.Qualification
                {
                    Id = 1,
                    Title = "Title",
                    ShortTitle = "ShortTitle"
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
                .Be($"'{nameof(ProviderQualificationFileImportDto.UkPrn)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");
        }
    }
}