using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Constants;
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
            _parseResult.Count().Should().Be(int.Parse(RoutePathMappingConstants.AgricultureLandManagementandProduction));

        [Fact]
        public void Then_ParseResult_LarsId_Matches_Input() =>
            _parseResult.First().LarsId.Should().BeEquivalentTo(RoutePathMappingConstants.LarsId);

        [Fact]
        public void Then_ParseResult_Title_Matches_Input() =>
            _parseResult.First().Title.Should().BeEquivalentTo(RoutePathMappingConstants.Title);

        [Fact]
        public void Then_ParseResult_ShortTitle_Matches_Input() =>
            _parseResult.First().ShortTitle.Should().BeEquivalentTo(RoutePathMappingConstants.ShortTitle);

        [Fact]
        public void Then_ParseResult_PathId_Matches_Input() =>
            _parseResult.First().PathId.Should().Be(RoutePathMappingConstants.PathId);
    }
}