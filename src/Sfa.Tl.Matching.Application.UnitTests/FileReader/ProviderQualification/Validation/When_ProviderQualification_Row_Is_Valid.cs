using System;
using System.Linq.Expressions;
using FluentAssertions;
using FluentValidation.Results;
using NSubstitute;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderQualification.Validation
{
    public class When_ProviderQualification_Row_Is_Valid : IClassFixture<ProviderQualificationFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;
        
        public When_ProviderQualification_Row_Is_Valid(ProviderQualificationFileImportValidationTestFixture fixture)
        {
            fixture.ProviderVenueRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Domain.Models.ProviderVenue, bool>>>())
                .Returns(new Domain.Models.ProviderVenue
                {
                    Id = 1,
                    Source = "Test"
                });

            fixture.QualificationRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Qualification, bool>>>())
                .Returns(new Qualification
                {
                    Id = 1,
                    Title = "Test"
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