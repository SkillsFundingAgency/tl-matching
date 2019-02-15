using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Parsing
{
    public class When_RoutePathMapping_Row_With_One_PathId_Is_Parsed : IClassFixture<QualificationRoutePathMappingParsingFixture>
    {
        private readonly IEnumerable<RoutePathMappingDto> _parseResult;

        public When_RoutePathMapping_Row_With_One_PathId_Is_Parsed(QualificationRoutePathMappingParsingFixture fixture)
        {
            _parseResult = fixture.Parser.Parse(fixture.Dto);
        }

        [Fact]
        public void Then_ParseResult_Count_Is_One() =>
            _parseResult.Count().Should().Be(int.Parse(ValidQualificationRoutePathMappingFileImportDtoBuilder.AgricultureLandManagementandProduction));

        [Fact]
        public void Then_ParseResult_LarsId_Matches_Input() =>
            _parseResult.First().LarsId.Should().BeEquivalentTo(ValidQualificationRoutePathMappingFileImportDtoBuilder.LarsId);

        [Fact]
        public void Then_ParseResult_Title_Matches_Input() =>
            _parseResult.First().Title.Should().BeEquivalentTo(ValidQualificationRoutePathMappingFileImportDtoBuilder.Title);

        [Fact]
        public void Then_ParseResult_ShortTitle_Matches_Input() =>
            _parseResult.First().ShortTitle.Should().BeEquivalentTo(ValidQualificationRoutePathMappingFileImportDtoBuilder.ShortTitle);

        [Fact]
        public void Then_ParseResult_PathId_Matches_Input() =>
            _parseResult.First().PathId.Should().Be(ValidQualificationRoutePathMappingFileImportDtoBuilder.PathId);
    }
}