﻿using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Constants;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Has_Wrong_Number_Of_Columns
    {
        private ValidationResult _validationResult;

        [SetUp]
        public void Setup()
        {
            var repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();
            var routePathRepository = Substitute.For<IRoutePathRepository>();

            var validator = new QualificationRoutePathMappingDataValidator(repository, routePathRepository);
            _validationResult = validator.Validate(new QualificationRoutePathMappingFileImportDto
            {
                LarsId = RoutePathMappingConstants.LarsId,
                Title = "Column 2",
                ShortTitle = "Column 3",
                Source = "Test"
            });
        }

        [Test]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Test]
        public void Then_Error_Count_Is_One() =>
            Assert.AreEqual(1, _validationResult.Errors.Count);

        [Test]
        public void Then_Error_Code_Is_WrongNumberOfColumns() =>
            Assert.AreEqual(ValidationErrorCode.WrongNumberOfColumns.ToString(),
                _validationResult.Errors[0].ErrorCode);

        [Test]
        public void Then_Error_Message_Is_WrongNumberOfColumns() =>
            Assert.AreEqual(ValidationErrorCode.WrongNumberOfColumns.Humanize(),
                _validationResult.Errors[0].ErrorMessage);
    }
}