﻿using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_QualificationRoutePathMapping_Row_Has_No_ShortTitle : IClassFixture<QualificationRoutePathMappingFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_QualificationRoutePathMapping_Row_Has_No_ShortTitle(QualificationRoutePathMappingFileImportValidationTestFixture fixture)
        {
            fixture.Dto.ShortTitle = "";
            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Code_Is_MissingMandatoryData() =>
            _validationResult.Errors[0].ErrorCode.Should()
                .BeEquivalentTo(ValidationErrorCode.MissingMandatoryData.ToString());

        [Fact]
        public void Then_Error_Message_Is_MissingMandatoryData() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .BeEquivalentTo($"'ShortTitle' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
    }
}