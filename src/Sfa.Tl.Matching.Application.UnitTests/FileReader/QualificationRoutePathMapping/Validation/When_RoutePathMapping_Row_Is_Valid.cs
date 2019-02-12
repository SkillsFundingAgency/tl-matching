using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Is_Valid
    {
        private ValidationResult _validationResult;

        [SetUp]
        public void Setup()
        {
            var routePathMapping = new ValidRoutePathMappingBuilder().Build();
            var dto = routePathMapping.ToDto();

            var repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();
            var routePathRepository = Substitute.For<IRoutePathRepository>();

            var validator = new QualificationRoutePathMappingDataValidator(repository, routePathRepository);
            _validationResult = validator.Validate(dto);
        }

        [Test]
        public void Then_Validation_Result_Is_Valid() =>
            Assert.True(_validationResult.IsValid);

        [Test]
        public void Then_Error_Count_Is_Zero() =>
            Assert.Zero(_validationResult.Errors.Count);
    }
}