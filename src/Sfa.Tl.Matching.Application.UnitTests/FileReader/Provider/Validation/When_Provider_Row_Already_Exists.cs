using System;
using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;

using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class When_Provider_Row_Already_Exists : IClassFixture<ProviderFileImportFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_Provider_Row_Already_Exists(ProviderFileImportFixture fixture)
        {
            fixture.Repository.GetSingleOrDefault(Arg.Any<Func<Domain.Models.Provider, bool>>())
                .Returns(new Domain.Models.Provider());

            var validator = new ProviderDataValidator(fixture.Repository);
            _validationResult = validator.Validate(fixture.ProviderFileImportDto);
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