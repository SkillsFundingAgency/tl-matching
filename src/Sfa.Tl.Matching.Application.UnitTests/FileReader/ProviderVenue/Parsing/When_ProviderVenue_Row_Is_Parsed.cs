using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ProviderVenue.Parsing
{
    public class When_ProviderVenue_Row_Is_Parsed : IClassFixture<ProviderVenueParsingFixture>
    {
        private readonly IEnumerable<ProviderVenueDto> _parseResult;
        private readonly ProviderVenueDto _firstProviderVenueDto;

        public When_ProviderVenue_Row_Is_Parsed(ProviderVenueParsingFixture fixture)
        {
            var dto = new ValidProviderVenueFileImportDtoBuilder().Build();
            _parseResult = fixture.Parser.Parse(fixture.Dto);
            _firstProviderVenueDto = _parseResult.First();
        }

        [Fact]
        public void Then_ParseResult_Count_Is_One() =>
            _parseResult.Count().Should().Be(1);

        [Fact]
        public void Then_First_ParseResult_ProviderId_Matches_Input() =>
            _parseResult.First().ProviderId.Should().Be(ValidProviderVenueFileImportDtoBuilder.ProviderId);

        [Fact]
        public void Then_First_ParseResult_PostCode_Matches_Input() =>
            _parseResult.First().Postcode.Should().BeEquivalentTo(ValidProviderVenueFileImportDtoBuilder.PostCode);

        [Fact]
        public void Then_First_ParseResult_ShortTitle_Matches_Input() =>
            _parseResult.First().Source.Should().BeEquivalentTo(ValidProviderVenueFileImportDtoBuilder.Source);
        [Fact]
        public void Then_First_ParseResult_CreatedBy_Matches_Input() =>
            _firstProviderVenueDto.CreatedBy.Should().Be("CreatedBy");
    }
}