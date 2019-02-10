using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Has_Multiple_PathIds
    {
        private ValidationResult _validationResult;

        [SetUp]
        public void Setup()
        {
            var routePathMapping = new ValidRoutePathMappingBuilder().Build();
            var routePathMappingStringArray = routePathMapping.ToStringArray(10);
            for (int i = RoutePathMappingColumnIndex.PathStartIndex; i < routePathMappingStringArray.Length; i++)
            {
                //routePathMappingStringArray[RoutePathMappingColumnIndex.PathStartIndex]
                //    = $"{i}";
            }

            var repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();

            var validator = new RoutePathMappingDataValidator(repository);
            _validationResult = validator.Validate(routePathMappingStringArray);
        }

        [Test]
        public void Then_Validation_Result_Is_Valid() =>
            Assert.True(_validationResult.IsValid);
    }
}