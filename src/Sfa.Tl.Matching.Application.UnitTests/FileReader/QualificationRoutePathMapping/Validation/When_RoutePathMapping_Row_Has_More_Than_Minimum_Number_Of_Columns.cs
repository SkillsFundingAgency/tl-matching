using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Constants;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Has_More_Than_Minimum_Number_Of_Columns
    {
        private ValidationResult _validationResult;

        [SetUp]
        public void Setup()
        {
            var routePathMapping = new ValidRoutePathMappingBuilder().Build();
            var routePathMappingStringArray 
                = routePathMapping.ToStringArray(10);

            var repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();

            var validator = new RoutePathMappingDataValidator(repository);
            _validationResult = validator.Validate(routePathMappingStringArray);
        }

        [Test]
        public void Then_Validation_Result_Is_Valid() =>
            Assert.True(_validationResult.IsValid);
    }
}