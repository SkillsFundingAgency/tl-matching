using FluentValidation.Results;
using Humanizer;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class When_Provider_Row_Has_Wrong_Number_Of_Columns
    {
        private ValidationResult _validationResult;

        [SetUp]
        public void Setup()
        {
            var validator = new ProviderDataValidator();
            _validationResult = validator.Validate(new[]
            {
                "123"
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
            Assert.AreEqual(ValidationErrorCode.WrongNumberOfColumns.ToString(), _validationResult.Errors[0].ErrorCode);

        [Test]
        public void Then_Error_Message_Is_WrongNumberOfColumnsCount() =>
            Assert.AreEqual(ValidationErrorCode.WrongNumberOfColumns.Humanize(), _validationResult.Errors[0].ErrorMessage);
    }
}