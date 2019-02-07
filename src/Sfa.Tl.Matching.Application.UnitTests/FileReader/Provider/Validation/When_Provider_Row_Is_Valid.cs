using FluentValidation.Results;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class When_Provider_Row_Is_Valid
    {
        private ValidationResult _validationResult;

        [SetUp]
        public void Setup()
        {
            var provider = new ValidProviderBuilder().Build();
            var providerStringArray = provider.ToStringArray();

            var repository = Substitute.For<IRepository<Domain.Models.Provider>>();
            var validator = new ProviderDataValidator(repository);
            _validationResult = validator.Validate(providerStringArray);
        }

        [Test]
        public void Then_Validation_Result_Is_Valid() =>
            Assert.True(_validationResult.IsValid);

        [Test]
        public void Then_Error_Count_Is_Zero() =>
            Assert.Zero(_validationResult.Errors.Count);
    }
}