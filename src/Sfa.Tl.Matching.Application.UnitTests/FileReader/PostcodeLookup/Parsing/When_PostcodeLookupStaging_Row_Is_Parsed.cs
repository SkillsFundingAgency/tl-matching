using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.PostcodeLookup.Parsing
{
    public class When_PostcodeLookupStaging_Row_Is_Parsed : IClassFixture<PostcodeLookupStagingParsingFixture>
    {
        private readonly IEnumerable<PostcodeLookupStagingDto> _parseResult;
        private readonly PostcodeLookupStagingDto _firstPostcodeLookupNameMappingDto;

        public When_PostcodeLookupStaging_Row_Is_Parsed(PostcodeLookupStagingParsingFixture fixture)
        {
            _parseResult = fixture.Parser.Parse(fixture.Dto);
            _firstPostcodeLookupNameMappingDto = _parseResult.First();
        }

        [Fact]
        public void Then_ParseResult_Count_Is_One() =>
            _parseResult.Count().Should().Be(1);

        [Fact]
        public void Then_First_ParseResult_Fields_Match_Input()
        {
            _firstPostcodeLookupNameMappingDto.Postcode
                .Should().Be("CA1 1AA");
            _firstPostcodeLookupNameMappingDto.PrimaryLepCode
                .Should().Be("E37000007");
            _firstPostcodeLookupNameMappingDto.SecondaryLepCode
                .Should().Be("E37000008");
            _firstPostcodeLookupNameMappingDto.CreatedBy
                .Should().Be("CreatedBy");
        }
    }
}