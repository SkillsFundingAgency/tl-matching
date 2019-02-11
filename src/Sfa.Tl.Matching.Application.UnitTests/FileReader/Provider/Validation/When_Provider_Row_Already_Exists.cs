using System;
using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;

using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class ProviderFileImportFixture
    {
        public ProviderDataValidator ProviderDataValidator;
        public ProviderFileImportDto ProviderFileImportDto;

        public ProviderFileImportFixture()
        {
            var builder = new ValidProviderBuilder();
            ProviderFileImportDto = ValidProviderBuilder.Build();
            var repository = Substitute.For<IRepository<Domain.Models.Provider>>();
            ProviderDataValidator = new ProviderDataValidator(repository);
        }
    }

    public class When_Provider_Row_Already_Exists : IClassFixture<ProviderFileImportFixture>
    {
        private ValidationResult _validationResult;
        
        public void Setup()
        {
            var provider = new Domain.Models.Provider();

            var providerFileImportDto = ValidProviderBuilder.Build();

            var repository = Substitute.For<IRepository<Domain.Models.Provider>>();
            repository.GetSingleOrDefault(Arg.Any<Func<Domain.Models.Provider, bool>>()).Returns(provider);

            var validator = new ProviderDataValidator(repository);
            _validationResult = validator.Validate(providerFileImportDto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Code_Is_RecordExists() =>
            _validationResult.Errors[0].ErrorCode.Should().Be(ValidationErrorCode.RecordAlreadyExists.ToString());

        [Fact]
        public void Then_Error_Message_Is_RecordExists() =>
            _validationResult.Errors[0].ErrorMessage.Should().Be($"'{nameof(Domain.Models.Provider.UkPrn)}' - {ValidationErrorCode.RecordAlreadyExists.Humanize()}");
    }
}