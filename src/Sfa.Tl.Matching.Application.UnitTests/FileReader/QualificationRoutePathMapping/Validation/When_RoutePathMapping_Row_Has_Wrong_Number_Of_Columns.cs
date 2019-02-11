using FluentValidation.Results;
using Humanizer;
using NSubstitute;

using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Constants;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Has_Wrong_Number_Of_Columns
    {
        private ValidationResult _validationResult;

        
        public void Setup()
        {
            var repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();
            var validator = new QualificationRoutePathMappingDataValidator(repository);
            _validationResult = validator.Validate(new QualificationRoutePathMappingFileImportDto
            {
                LarsId = RoutePathMappingConstants.LarsId,
                Title = "Column 2",
                ShortTitle = "Column 3",
            });
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Fact]
        public void Then_Error_Count_Is_One() =>
            Assert.Equal(1, _validationResult.Errors.Count);

        [Fact]
        public void Then_Error_Code_Is_WrongNumberOfColumns() =>
            Assert.Equal(ValidationErrorCode.WrongNumberOfColumns.ToString(),
                _validationResult.Errors[0].ErrorCode);

        [Fact]
        public void Then_Error_Message_Is_WrongNumberOfColumns() =>
            Assert.Equal(ValidationErrorCode.WrongNumberOfColumns.Humanize(),
                _validationResult.Errors[0].ErrorMessage);
    }
}