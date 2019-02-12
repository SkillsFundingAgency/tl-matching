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
    public class When_RoutePathMapping_Row_LarsId_Has_Invalid_Format
    {
        private ValidationResult _validationResult;

        [SetUp]
        public void Setup()
        {
            var routePathMapping = new ValidRoutePathMappingBuilder().Build();
            var dto = routePathMapping.ToDto();
            dto.LarsId = "12345";

            var repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();
            var routePathRepository = Substitute.For<IRoutePathRepository>();

            var validator = new QualificationRoutePathMappingDataValidator(repository, routePathRepository);
            _validationResult = validator.Validate(dto);
        }

        [Test]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Test]
        public void Then_Error_Count_Is_One() =>
            Assert.AreEqual(1, _validationResult.Errors.Count);

        [Test]
        public void Then_Error_Code_Is_InvalidFormat() =>
            Assert.AreEqual(ValidationErrorCode.InvalidFormat.ToString(), 
                _validationResult.Errors[0].ErrorCode);

        [Test]
        public void Then_Error_Message_Is_InvalidFormat() =>
            Assert.AreEqual($"'{nameof(Domain.Models.RoutePathMapping.LarsId)}' - {ValidationErrorCode.InvalidFormat.Humanize()}", 
                _validationResult.Errors[0].ErrorMessage);
    }
}