using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;

using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class When_Provider_Row_Has_No_Name
    {
        private ValidationResult _validationResult;

        public void Setup()
        {
            var providerStringArray = new ProviderFileImportDto { ProviderName = "" };

            var repository = Substitute.For<IRepository<Domain.Models.Provider>>();

            var validator = new ProviderDataValidator(repository);
            _validationResult = validator.Validate(providerStringArray);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            Assert.False(_validationResult.IsValid);

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(22);

        [Fact]
        public void Then_Error_Code_Is_MissingMandatoryData() =>
            _validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.MissingMandatoryData.ToString());

        [Fact]
        public void Then_Error_Message_Is_MissingMandatoryData() =>
            _validationResult.Errors[2].ErrorMessage.Should()
                .Be($"'{nameof(ProviderFileImportDto.ProviderName)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
    }
}