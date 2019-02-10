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
    public class When_Provider_Row_UkPrn_Has_Invalid_Format
    {
        private ValidationResult _validationResult;

        [SetUp]
        public void Setup()
        {
            var providerStringArray = new ProviderFileImportDto { UkPrn = "159856587455885" };

            var repository = Substitute.For<IRepository<Domain.Models.Provider>>();

            var validator = new ProviderDataValidator(repository);
            _validationResult = validator.Validate(providerStringArray);
        }

        [Test]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Test]
        public void Then_Error_Count_Is_22() =>
            Assert.AreEqual(22, _validationResult.Errors.Count);

        [Test]
        public void Then_Error_Code_Is_InvalidFormat() =>
            Assert.AreEqual(ValidationErrorCode.InvalidFormat.ToString(),
                _validationResult.Errors[0].ErrorCode);

        [Test]
        public void Then_Error_Message_Is_InvalidFormat() =>
            Assert.AreEqual($"'{nameof(Domain.Models.Provider.UkPrn)}' - {ValidationErrorCode.InvalidFormat.Humanize()}",
                _validationResult.Errors[0].ErrorMessage);
    }
}