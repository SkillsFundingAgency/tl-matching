using System;
using FluentAssertions;
using FluentValidation.Results;
using Humanizer;
using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader.ProviderVenue;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Builders;
using Sfa.Tl.Matching.Application.UnitTests.Provider.Builders;
using Sfa.Tl.Matching.Application.UnitTests.ProviderVenue.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Validation
{
    public class ProviderVenueNotUniqueFixture
    {
        public ProviderVenueDataValidator Validator;
        public ProviderVenueFileImportDto Dto;
        public IRepository<Domain.Models.Provider> ProviderRepository;
        public IRepository<Domain.Models.ProviderVenue> ProviderVenueRepository;

        public ProviderVenueNotUniqueFixture()
        {
            Dto = new ProviderVenueFileImportDtoBuilder().Build();

            ProviderRepository = Substitute.For<IRepository<Domain.Models.Provider>>();
            var provider = ValidProviderBuilder.Build();
            ProviderRepository.GetSingleOrDefault(
                Arg.Is<Func<Domain.Models.Provider, bool>>(p => p(provider)))
                .Returns(provider);

            ProviderVenueRepository = Substitute.For<IRepository<Domain.Models.ProviderVenue>>();
            var providerVenue = ValidProviderVenueBuilder.Build();
            ProviderVenueRepository.GetSingleOrDefault(
                    Arg.Any<Func<Domain.Models.ProviderVenue, bool>>())
                .Returns(providerVenue);

            Validator = new ProviderVenueDataValidator(ProviderRepository,
                ProviderVenueRepository);
        }
    }

    public class When_ProviderVenue_Row_Has_Venue_Not_Unique : IClassFixture<ProviderVenueNotUniqueFixture>
    {
        private readonly ValidationResult _validationResult;

        public When_ProviderVenue_Row_Has_Venue_Not_Unique(ProviderVenueNotUniqueFixture fixture)
        {
            _validationResult = fixture.Validator.Validate(fixture.Dto);
        }

        [Fact]
        public void Then_Validation_Result_Is_Not_Valid() =>
            _validationResult.IsValid.Should().BeFalse();

        [Fact]
        public void Then_Error_Count_Is_One() =>
            _validationResult.Errors.Count.Should().Be(1);

        [Fact]
        public void Then_Error_Code_Is_RecordExists() =>
            _validationResult.Errors[0].ErrorCode.Should()
                .Be(ValidationErrorCode.VenueAlreadyExists.ToString());

        [Fact]
        public void Then_Error_Message_Is_RecordExists() =>
            _validationResult.Errors[0].ErrorMessage.Should()
                .Be($"'{nameof(ProviderVenueFileImportDto.UkPrn)}' - {ValidationErrorCode.VenueAlreadyExists.Humanize()}");
    }
}