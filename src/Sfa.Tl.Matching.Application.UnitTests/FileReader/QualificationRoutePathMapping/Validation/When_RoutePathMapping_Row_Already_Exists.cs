using System;
using System.Linq;
using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Already_Exists : IClassFixture<QualificationRoutePathMappingFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_RoutePathMapping_Row_Already_Exists(QualificationRoutePathMappingFileImportValidationTestFixture fixture)
        {
            fixture.Repository.GetMany(Arg.Any<Func<RoutePathMapping, bool>>())
                .Returns(
                    new System.Collections.Generic.List<RoutePathMapping>
                        {
                            new RoutePathMapping{Id = 1, Title = "Title", ShortTitle = "ShortTitle", PathId = 1}
                        }
                        .AsQueryable());

            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Code_Is_RecordExists() =>
            _validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.RecordAlreadyExists.ToString());

        [Fact]
        public void Then_Error_Message_Is_RecordExists() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(RoutePathMapping.LarsId)}' - {ValidationErrorCode.RecordAlreadyExists.Humanize()}");
    }
}