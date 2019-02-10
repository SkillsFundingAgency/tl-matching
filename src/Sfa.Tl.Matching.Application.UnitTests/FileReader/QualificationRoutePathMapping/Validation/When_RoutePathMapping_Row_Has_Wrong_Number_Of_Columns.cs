using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Constants;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Validation
{
    public class When_RoutePathMapping_Row_Has_Wrong_Number_Of_Columns
    {
        private ValidationResult _validationResult;

        [SetUp]
        public void Setup()
        {
            var repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();
            var validator = new RoutePathMappingDataValidator(repository);
            _validationResult = validator.Validate(new[]
            {
                RoutePathMappingConstants.LarsId,
                "Column 2",
                "Column 3",
            });
        }

        [Test]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Test]
        public void Then_Error_Count_Is_One() =>
            Assert.AreEqual(1, _validationResult.Errors.Count);

        [Test]
        public void Then_Error_Code_Is_WrongNumberOfColumns() =>
            Assert.AreEqual(ValidationErrorCode.WrongNumberOfColumns.ToString(), 
                _validationResult.Errors[0].ErrorCode);

        [Test]
        public void Then_Error_Message_Is_WrongNumberOfColumns() =>
            Assert.AreEqual(ValidationErrorCode.WrongNumberOfColumns.Humanize(), 
                _validationResult.Errors[0].ErrorMessage);
    }
}