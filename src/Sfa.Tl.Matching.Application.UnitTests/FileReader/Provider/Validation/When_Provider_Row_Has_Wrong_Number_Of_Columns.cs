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
    public class When_Provider_Row_Has_Wrong_Number_Of_Columns
    {
        private ValidationResult _validationResult;

        public void Setup()
        {
            var repository = Substitute.For<IRepository<Domain.Models.Provider>>();
            var validator = new ProviderDataValidator(repository);
            _validationResult = validator.Validate(new ProviderFileImportDto { PrimaryContactName = "Fact" });
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_21() =>
            _validationResult.Errors.Count.Should().Be(21);

        [Fact]
        public void Then_Error_Message_Is_MissingMandatoryData() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(ProviderFileImportDto.UkPrn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
    }
}