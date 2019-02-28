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
    public class When_ProviderQualification_Record_Already_Exists : IClassFixture<ProviderQualificationFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_ProviderQualification_Record_Already_Exists(ProviderQualificationFileImportValidationTestFixture fixture)
        {
            fixture.QualificationRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Qualification, bool>>>())
                .Returns(new Qualification
                {
                    Id = 1,
                    Title = "Test"
                });

            fixture.ProviderQualificationRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderQualification, bool>>>())
                .Returns(new Domain.Models.ProviderQualification
                {
                    Id = 1,
                    Source = "Test"
                });

            fixture.ProviderVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new Domain.Models.ProviderVenue
                {
                    Id = 1,
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
        public void Then_Error_Code_Is_ProviderQualificationAlreadyExists() =>
            _validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.ProviderQualificationAlreadyExists.ToString());

        [Fact]
        public void Then_Error_Message_Is_ProviderQualificationAlreadyExists() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(ProviderQualificationFileImportDto.LarsId)}' - {ValidationErrorCode.ProviderQualificationAlreadyExists.Humanize()}");
    }
}