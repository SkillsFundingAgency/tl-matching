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
    public class When_RoutePathMapping_Row_Has_No_Title
    {
        private ValidationResult _validationResult;

        [SetUp]
        public void Setup()
        {
            var routePathMapping = new ValidRoutePathMappingBuilder().Build();
            var routePathMappingStringArray = routePathMapping.ToStringArray();
            routePathMappingStringArray[RoutePathMappingColumnIndex.Title] = "";

            var repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();

            var validator = new RoutePathMappingDataValidator(repository);
            _validationResult = validator.Validate(routePathMappingStringArray);
        }

        [Test]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Test]
        public void Then_Error_Count_Is_One() =>
            Assert.AreEqual(1, _validationResult.Errors.Count);

        [Test]
        public void Then_Error_Code_Is_MissingMandatoryData() =>
            Assert.AreEqual(ValidationErrorCode.MissingMandatoryData.ToString(), _validationResult.Errors[0].ErrorCode);

        [Test]
        public void Then_Error_Message_Is_MissingMandatoryData() =>
            Assert.AreEqual($"'{nameof(RoutePathMappingColumnIndex.Title)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}", _validationResult.Errors[0].ErrorMessage);
    }
}