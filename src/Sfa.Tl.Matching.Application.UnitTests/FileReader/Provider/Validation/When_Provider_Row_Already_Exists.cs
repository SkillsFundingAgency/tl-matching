using System;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class When_Provider_Row_Already_Exists
    {
        private ValidationResult _validationResult;
        
        [SetUp]
        public void Setup()
        {
            var provider = new Domain.Models.Provider();

            var providerFileImportDto = new ValidProviderBuilder().Build();

            var repository = Substitute.For<IRepository<Domain.Models.Provider>>();
            repository.GetSingleOrDefault(Arg.Any<Func<Domain.Models.Provider, bool>>()).Returns(provider);

            var validator = new ProviderDataValidator(repository);
            _validationResult = validator.Validate(providerFileImportDto);
        }

        [Test]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Test]
        public void Then_Error_Count_Is_One() =>
            Assert.AreEqual(1, _validationResult.Errors.Count);

        [Test]
        public void Then_Error_Code_Is_RecordExists() =>
            Assert.AreEqual(ValidationErrorCode.RecordAlreadyExists.ToString(), 
                _validationResult.Errors[0].ErrorCode);

        [Test]
        public void Then_Error_Message_Is_RecordExists() =>
            Assert.AreEqual($"'{nameof(Domain.Models.Provider.UkPrn)}' - {ValidationErrorCode.RecordAlreadyExists.Humanize()}", _validationResult.Errors[0].ErrorMessage);
    }
}