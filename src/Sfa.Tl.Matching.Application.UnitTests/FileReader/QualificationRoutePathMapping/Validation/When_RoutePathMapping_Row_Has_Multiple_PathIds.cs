using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Constants;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Has_Multiple_PathIds
    {
        private ValidationResult _validationResult;

        [SetUp]
        public void Setup()
        {
            var routePathMapping = new ValidRoutePathMappingBuilder().Build();
            var dto = routePathMapping.ToDto();
            dto.AgricultureLandManagementandProduction = RoutePathMappingConstants.AgricultureLandManagementandProduction;
            dto.AnimalCareandManagement = RoutePathMappingConstants.AnimalCareandManagement;
            dto.Hospitality = RoutePathMappingConstants.Hospitality;

            var repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();

            var validator = new QualificationRoutePathMappingDataValidator(repository);
            _validationResult = validator.Validate(dto);
        }

        [Test]
        public void Then_Validation_Result_Is_Valid() =>
            Assert.True(_validationResult.IsValid);
    }
}