using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_QualificationRoutePathMapping_Row_Already_Exists : IClassFixture<QualificationRoutePathMappingFileImportValidationTestFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_QualificationRoutePathMapping_Row_Already_Exists(QualificationRoutePathMappingFileImportValidationTestFixture fixture)
        {
            fixture.QualificationRepository.GetSingleOrDefault(Arg.Any<Expression<Func<Qualification, bool>>>())
                .Returns(new Qualification
                {
                        Id = 1,
                        LarsId = "100000",
                        Title = "Title",
                        ShortTitle = "ShortTitle"
                });

            fixture.QualificationRoutePathMappingRepository.GetMany(Arg.Any<Expression<Func<Domain.Models.QualificationRoutePathMapping, bool>>>())
                .Returns(new List<Domain.Models.QualificationRoutePathMapping>
                        {
                            new Domain.Models.QualificationRoutePathMapping
                            {
                                Id = 1,
                                QualificationId = 1,
                                PathId = 10
                            }
                        }.AsQueryable());

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
                .Be(ValidationErrorCode.QualificationRoutePathMappingAlreadyExists.ToString());

        [Fact]
        public void Then_Error_Message_Is_RecordExists() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(QualificationRoutePathMappingFileImportDto.LarsId)}' - {ValidationErrorCode.QualificationRoutePathMappingAlreadyExists.Humanize()}");
    }
}