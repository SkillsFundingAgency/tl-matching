using FluentValidation.Results;
using NSubstitute;

using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Has_More_Than_Minimum_Number_Of_Columns
    {
        private ValidationResult _validationResult;

        
        public void Setup()
        {
            var routePathMapping = new ValidRoutePathMappingBuilder().Build();
            var dto = routePathMapping.ToDto();

            var repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();

            var validator = new QualificationRoutePathMappingDataValidator(repository);
            _validationResult = validator.Validate(dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Valid() =>
            Assert.True(_validationResult.IsValid);
    }
}