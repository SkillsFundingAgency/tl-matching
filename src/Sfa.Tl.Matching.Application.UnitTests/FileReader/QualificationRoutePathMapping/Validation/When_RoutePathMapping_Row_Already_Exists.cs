using System;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Already_Exists
    {
        private ValidationResult _validationResult;
        
        [SetUp]
        public void Setup()
        {
            var routePathMapping = new ValidRoutePathMappingBuilder().Build();

            var repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();
            repository.GetSingleOrDefault(Arg.Is<Func<Domain.Models.RoutePathMapping, bool>>(p => p(routePathMapping))).Returns(routePathMapping);

            var validator = new RoutePathMappingDataValidator(repository);
            _validationResult = validator.Validate(routePathMapping.ToStringArray());
        }

        [Test]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Test]
        public void Then_Error_Count_Is_One() =>
            Assert.AreEqual(1, _validationResult.Errors.Count);

        [Test]
        public void Then_Error_Code_Is_RecordExists() =>
            Assert.AreEqual(ValidationErrorCode.RecordExists.ToString(), 
                _validationResult.Errors[0].ErrorCode);

        [Test]
        public void Then_Error_Message_Is_RecordExists() =>
            Assert.AreEqual($"'{nameof(Domain.Models.RoutePathMapping.LarsId)}' - {ValidationErrorCode.RecordExists.Humanize()}", _validationResult.Errors[0].ErrorMessage);
    }
}