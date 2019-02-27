using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderQualification.Validation
{
    public class When_ProviderQualification_Row_Has_Invalid_Qualification : IClassFixture<ProviderQualificationFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_ProviderQualification_Row_Has_Invalid_Qualification(ProviderQualificationFileImportValidationTestFixture fixture)
        {
            var fixture1 = fixture;
            
            fixture.QualificationRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Qualification, bool>>>())
                .Returns((Qualification)null);

            fixture.ProviderVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new Domain.Models.ProviderVenue
                {
                    Id = 1,
                    Source = "Test"
                });

            _validationResult = fixture1.Validator.Validate(fixture1.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid()
        {
            _validationResult.IsValid.Should().BeFalse();
        }

        [Fact]
        public void Then_Validation_Error_Count_is_One()
        {
            _validationResult.Errors.Count.Should().Be(1);
        }

        [Fact]
        public void Then_Validation_Error_Code_is_QualificationDoesntExist()
        {
            _validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.QualificationDoesntExist.ToString());
        }

        [Fact]
        public void Then_Validation_Error_Message_is_QualificationDoesntExist()
        {
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(ProviderQualificationFileImportDto.LarsId)}' - {ValidationErrorCode.QualificationDoesntExist.Humanize()}");
        }
    }
}