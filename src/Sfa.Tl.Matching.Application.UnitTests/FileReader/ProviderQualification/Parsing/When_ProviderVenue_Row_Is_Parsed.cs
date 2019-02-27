using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderQualification.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderQualification.Parsing
{
    public class When_ProviderQualification_Row_Is_Parsed : IClassFixture<ProviderQualificationParsingFixture>
    {
        private readonly IEnumerable<ProviderQualificationDto> _parseResult;
        private readonly ProviderQualificationDto _firstProviderQualificationDto;

        public When_ProviderQualification_Row_Is_Parsed(ProviderQualificationParsingFixture fixture)
        {
            _parseResult = fixture.Parser.Parse(fixture.Dto);
            _firstProviderQualificationDto = _parseResult.First();
        }

        [Fact]
        public void Then_ParseResult_Count_Is_One() =>
            _parseResult.Count().Should().Be(1);

        [Fact]
        public void Then_First_ParseResult_ProviderVenueId_Matches_Input() =>
            _parseResult.First().ProviderVenueId.Should().Be(ValidProviderQualificationFileImportDtoBuilder.ProviderVenueId);

        [Fact]
        public void Then_First_ParseResult_QualificationId_Matches_Input() =>
            _parseResult.First().QualificationId.Should().Be(ValidProviderQualificationFileImportDtoBuilder.QualificationId);

        [Fact]
        public void Then_First_ParseResult_NumberOfPlacements_Matches_Input() =>
            _parseResult.First().NumberOfPlacements.Should().Be(ValidProviderQualificationFileImportDtoBuilder.NumberOfPlacements);

        [Fact]
        public void Then_First_ParseResult_Source_Matches_Input() =>
            _parseResult.First().Source.Should().BeEquivalentTo(ValidProviderQualificationFileImportDtoBuilder.Source);

        [Fact]
        public void Then_First_ParseResult_CreatedBy_Matches_Input() =>
            _firstProviderQualificationDto.CreatedBy.Should().Be(ValidProviderQualificationFileImportDtoBuilder.CreatedBy);
    }
}