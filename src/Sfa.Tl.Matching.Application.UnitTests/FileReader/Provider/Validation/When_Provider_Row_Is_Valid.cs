using FluentAssertions;
using FluentValidation.Results;
using NSubstitute;

using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class When_Provider_Row_Is_Valid
    {
        private ValidationResult _validationResult;

        
        public void Setup()
        {
            var providerFileImportDto = ValidProviderBuilder.Build();
            var repository = Substitute.For<IRepository<Domain.Models.Provider>>();
            var validator = new ProviderDataValidator(repository);
            _validationResult = validator.Validate(providerFileImportDto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Valid() =>
            _validationResult.IsValid.Should().BeTrue();

        [Fact]
        public void Then_Error_Count_Is_Zero() =>
            _validationResult.Errors.Count.Should().Be(0);
    }
}