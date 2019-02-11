using System.Collections.Generic;
using System.Linq;

using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Constants;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Parsing
{
    public class When_RoutePathMapping_Row_With_Multiple_PathIds_Is_Parsed
    {
        private IEnumerable<RoutePathMappingDto> _parseResult;

        
        public void Setup()
        {
            var routePathMappingDto = new ValidQualificationRoutePathMappingFileImportDtoBuilder().Build();
            routePathMappingDto.AgricultureLandManagementandProduction = null;
            routePathMappingDto.AnimalCareandManagement = RoutePathMappingConstants.AnimalCareandManagement;
            routePathMappingDto.Hospitality = RoutePathMappingConstants.Hospitality;

            var parser = new QualificationRoutePathMappingDataParser();
            _parseResult = parser.Parse(routePathMappingDto);
        }

        [Fact]
        public void Then_ParseResult_Count_Is_Two() =>
            Assert.Equal(2, _parseResult.Count());
        
        [Fact]
        public void Then_First_ParseResult_LarsId_Matches_Input() =>
            Assert.Equal(RoutePathMappingConstants.LarsId, _parseResult.First().LarsId);
        
        [Fact]
        public void Then_First_ParseResult_Title_Matches_Input() =>
            Assert.Equal(RoutePathMappingConstants.Title, _parseResult.First().Title);

        [Fact]
        public void Then_First_ParseResult_ShortTitle_Matches_Input() =>
            Assert.Equal(RoutePathMappingConstants.ShortTitle, _parseResult.First().ShortTitle);

        [Fact]
        public void Then_First_ParseResult_PathId_Matches_Input() =>
            Assert.Equal(int.Parse(RoutePathMappingConstants.AnimalCareandManagement), _parseResult.First().PathId);

        [Fact]
        public void Then_Second_ParseResult_LarsId_Matches_Input() =>
            Assert.Equal(RoutePathMappingConstants.LarsId, _parseResult.Skip(1).First().LarsId);

        [Fact]
        public void Then_Second_ParseResult_Title_Matches_Input() =>
            Assert.Equal(RoutePathMappingConstants.Title, _parseResult.Skip(1).First().Title);

        [Fact]
        public void Then_Second_ParseResult_ShortTitle_Matches_Input() =>
            Assert.Equal(RoutePathMappingConstants.ShortTitle, _parseResult.Skip(1).First().ShortTitle);

        [Fact]
        public void Then_Second_First_ParseResult_PathId_Matches_Input() =>
            Assert.Equal(int.Parse(RoutePathMappingConstants.Hospitality), _parseResult.Skip(1).First().PathId);
    }
}