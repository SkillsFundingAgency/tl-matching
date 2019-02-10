using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Constants;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Parsing
{
    public class When_RoutePathMapping_Row_With_One_PathId_Is_Parsed
    {
        private IEnumerable<RoutePathMappingDto> _parseResult;

        [SetUp]
        public void Setup()
        {
            var routePathMappingDto = new ValidQualificationRoutePathMappingFileImportDtoBuilder().Build();

            var parser = new QualificationRoutePathMappingDataParser();
            _parseResult = parser.Parse(routePathMappingDto);
        }

        [Test]
        public void Then_ParseResult_Count_Is_One() =>
            Assert.AreEqual(int.Parse(RoutePathMappingConstants.AgricultureLandManagementandProduction), _parseResult.Count());
        
        [Test]
        public void Then_ParseResult_LarsId_Matches_Input() =>
            Assert.AreEqual(RoutePathMappingConstants.LarsId, _parseResult.First().LarsId);
        
        [Test]
        public void Then_ParseResult_Title_Matches_Input() =>
            Assert.AreEqual(RoutePathMappingConstants.Title, _parseResult.First().Title);

        [Test]
        public void Then_ParseResult_ShortTitle_Matches_Input() =>
            Assert.AreEqual(RoutePathMappingConstants.ShortTitle, _parseResult.First().ShortTitle);

        [Test]
        public void Then_ParseResult_PathId_Matches_Input() =>
            Assert.AreEqual(RoutePathMappingConstants.PathId, _parseResult.First().PathId);
    }
}