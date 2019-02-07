using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class When_Provider_Row_PrimaryContactPhone_Has_Invalid_Format
    {
        private ValidationResult _validationResult;

        [SetUp]
        public void Setup()
        {
            var provider = new ValidProviderBuilder().Build();
            var providerStringArray = provider.ToStringArray();
            providerStringArray[(int)ProviderColumnIndex.PrimaryContactPhone] = "1";

            var repository = Substitute.For<IRepository<Domain.Models.Provider>>();

            var validator = new ProviderDataValidator(repository);
            _validationResult = validator.Validate(providerStringArray);
        }

        [Test]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Test]
        public void Then_Error_Count_Is_One() =>
            Assert.AreEqual(1, _validationResult.Errors.Count);

        [Test]
        public void Then_Error_Code_Is_InvalidFormat() =>
            Assert.AreEqual(ValidationErrorCode.InvalidFormat.ToString(), 
                _validationResult.Errors[0].ErrorCode);

        [Test]
        public void Then_Error_Message_Is_InvalidFormat() =>
            Assert.AreEqual($"'{nameof(Domain.Models.Provider.PrimaryContactPhone)}' - {ValidationErrorCode.InvalidFormat.Humanize()}", 
                _validationResult.Errors[0].ErrorMessage);
    }
}