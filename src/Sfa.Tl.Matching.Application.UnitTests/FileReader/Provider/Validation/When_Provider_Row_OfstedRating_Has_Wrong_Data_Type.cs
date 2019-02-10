using System.Linq;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class When_Provider_Row_OfstedRating_Has_Wrong_Data_Type
    {
        private ValidationResult _validationResult;

        [SetUp]
        public void Setup()
        {
            var providerStringArray = new ProviderFileImportDto { OfstedRating = "A" };

            var repository = Substitute.For<IRepository<Domain.Models.Provider>>();

            var validator = new ProviderDataValidator(repository);
            _validationResult = validator.Validate(providerStringArray);
        }

        [Test]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Test]
        public void Then_Error_Count_Is_21() =>
            Assert.AreEqual(21, _validationResult.Errors.Count);

        [Test]
        public void Then_Error_Code_Is_WrongDataType() =>
            Assert.AreEqual(ValidationErrorCode.WrongDataType.ToString(),
                _validationResult.Errors.First(e => e.ErrorCode == ValidationErrorCode.WrongDataType.ToString()).ErrorCode);

        [Test]
        public void Then_Error_Message_Is_WrongDataType() =>
            Assert.AreEqual($"'{nameof(Domain.Models.Provider.OfstedRating)}' - {ValidationErrorCode.WrongDataType.Humanize()}",
                _validationResult.Errors.First(e => e.ErrorCode == ValidationErrorCode.WrongDataType.ToString()).ErrorMessage);
    }
}