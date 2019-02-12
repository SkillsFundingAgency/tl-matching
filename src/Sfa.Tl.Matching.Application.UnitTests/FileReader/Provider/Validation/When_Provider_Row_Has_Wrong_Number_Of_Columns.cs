using System;
using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.Provider.Validation
{
    public class When_Provider_Row_Has_Wrong_Number_Of_Columns : IClassFixture<ProviderFileImportFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_Provider_Row_Has_Wrong_Number_Of_Columns(ProviderFileImportFixture fixture)
        {
            fixture.ProviderFileImportDto.UkPrn = null;
            fixture.Repository.GetSingleOrDefault(Arg.Any<Func<Domain.Models.Provider, bool>>()).Returns((Domain.Models.Provider)null);
            _validationResult = fixture.ProviderDataValidator.Validate(fixture.ProviderFileImportDto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Message_Is_MissingMandatoryData() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(ProviderFileImportDto.UkPrn)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}");
    }
}