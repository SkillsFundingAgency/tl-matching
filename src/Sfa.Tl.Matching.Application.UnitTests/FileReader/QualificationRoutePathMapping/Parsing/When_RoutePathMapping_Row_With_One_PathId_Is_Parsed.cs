using System.Collections.Generic;
using System.Linq;

using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Constants;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Parsing
{
    public class When_RoutePathMapping_Row_With_One_PathId_Is_Parsed
    {
        private IEnumerable<RoutePathMappingDto> _parseResult;

        
        public void Setup()
        {
            var routePathMappingDto = new ValidQualificationRoutePathMappingFileImportDtoBuilder().Build();

            var parser = new QualificationRoutePathMappingDataParser();
            _parseResult = parser.Parse(routePathMappingDto);
        }

        [Fact]
        public void Then_ParseResult_Count_Is_One() =>
            Assert.Equal(int.Parse(RoutePathMappingConstants.AgricultureLandManagementandProduction), _parseResult.Count());
        
        [Fact]
        public void Then_ParseResult_LarsId_Matches_Input() =>
            Assert.Equal(RoutePathMappingConstants.LarsId, _parseResult.First().LarsId);
        
        [Fact]
        public void Then_ParseResult_Title_Matches_Input() =>
            Assert.Equal(RoutePathMappingConstants.Title, _parseResult.First().Title);

        [Fact]
        public void Then_ParseResult_ShortTitle_Matches_Input() =>
            Assert.Equal(RoutePathMappingConstants.ShortTitle, _parseResult.First().ShortTitle);

        [Fact]
        public void Then_ParseResult_PathId_Matches_Input() =>
            Assert.Equal(RoutePathMappingConstants.PathId, _parseResult.First().PathId);
    }
}