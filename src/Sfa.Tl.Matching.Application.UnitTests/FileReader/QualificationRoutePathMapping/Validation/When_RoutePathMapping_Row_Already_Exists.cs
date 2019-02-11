using System;
using System.Linq;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;

using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Already_Exists
    {
        private ValidationResult _validationResult;
        
        public void Setup()
        {
            var routePathMapping = new ValidRoutePathMappingBuilder().Build();

            var repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();
            repository.GetMany(Arg.Is<Func<Domain.Models.RoutePathMapping, bool>>(p => p(routePathMapping)))
                .Returns(
                    new System.Collections.Generic.List<Domain.Models.RoutePathMapping>
                        {
                            routePathMapping
                        }
                        .AsQueryable());

            var validator = new QualificationRoutePathMappingDataValidator(repository);
            _validationResult = validator.Validate(routePathMapping.ToDto());
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Fact]
        public void Then_Error_Count_Is_One() =>
            Assert.Same(1, _validationResult.Errors.Count);

        [Fact]
        public void Then_Error_Code_Is_RecordExists() =>
            Assert.Equal(ValidationErrorCode.RecordAlreadyExists.ToString(), 
                _validationResult.Errors[0].ErrorCode);

        [Fact]
        public void Then_Error_Message_Is_RecordExists() =>
            Assert.Equal($"'{nameof(Domain.Models.RoutePathMapping.LarsId)}' - {ValidationErrorCode.RecordAlreadyExists.Humanize()}", _validationResult.Errors[0].ErrorMessage);
    }
}