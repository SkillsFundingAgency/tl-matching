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
    public class When_Provider_Row_UkPrn_Has_Invalid_Format
    {
        private ValidationResult _validationResult;

        public void Setup()
        {
            var dto = new ProviderFileImportDto { UkPrn = "159856587455885" };

            var repository = Substitute.For<IRepository<Domain.Models.Provider>>();

            var validator = new ProviderDataValidator(repository);
            _validationResult = validator.Validate(dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_22() =>
            _validationResult.Errors.Count.Should().Be(22);

        [Fact]
        public void Then_Error_Code_Is_InvalidFormat() =>
            _validationResult.Errors[0].ErrorCode.Should().Be(ValidationErrorCode.InvalidFormat.ToString());

        [Fact]
        public void Then_Error_Message_Is_InvalidFormat() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(Domain.Models.Provider.UkPrn)}' - {ValidationErrorCode.InvalidFormat.Humanize()}");
    }
}