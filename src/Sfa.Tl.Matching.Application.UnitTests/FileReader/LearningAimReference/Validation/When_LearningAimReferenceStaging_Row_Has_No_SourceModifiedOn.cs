﻿using FluentAssertions;
using Humanizer;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.LearningAimReference.Validation
{
    public class When_LearningAimReferenceStaging_Row_Has_No_SourceModifiedOn : IClassFixture<LearningAimReferenceStagingFileImportFixture>
    {
        private readonly LearningAimReferenceStagingFileImportFixture _fixture;

        public When_LearningAimReferenceStaging_Row_Has_No_SourceModifiedOn(LearningAimReferenceStagingFileImportFixture fixture)
        {
            _fixture = fixture;
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void Then_Validation_Result_Is_Not_Valid(string sourceModifiedOn)
        {
            _fixture.Dto.SourceModifiedOn = sourceModifiedOn;

            var validationResult = _fixture.Validator.Validate(_fixture.Dto);

            validationResult.IsValid.Should().BeFalse();

            validationResult.Errors.Count.Should().Be(2);

            validationResult.Errors[0].ErrorCode.Should().Be(ValidationErrorCode.MissingMandatoryData.ToString());
            validationResult.Errors[0].ErrorMessage.Should().Be($"'{nameof(LearningAimReferenceStagingFileImportDto.SourceModifiedOn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
        }
    }
}