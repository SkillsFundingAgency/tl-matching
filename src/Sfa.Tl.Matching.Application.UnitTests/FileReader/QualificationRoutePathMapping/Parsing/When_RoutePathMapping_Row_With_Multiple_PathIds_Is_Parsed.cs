using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Constants;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Parsing
{
    public class When_RoutePathMapping_Row_With_Multiple_PathIds_Is_Parsed
    {
        private IEnumerable<RoutePathMappingDto> _parseResult;

        [SetUp]
        public void Setup()
        {
            var routePathMapping = new ValidRoutePathMappingBuilder().Build();
            var routePathMappingStringArray = routePathMapping.ToStringArray(15);
            routePathMappingStringArray[RoutePathMappingColumnIndex.PathStartIndex] = "";
            routePathMappingStringArray[RoutePathMappingColumnIndex.PathStartIndex + 1] = "2";
            routePathMappingStringArray[RoutePathMappingColumnIndex.PathStartIndex + 4] = "5";

            var parser = new RoutePathMappingDataParser();
            _parseResult = parser.Parse(routePathMappingStringArray);
        }

        [Test]
        public void Then_ParseResult_Count_Is_Two() =>
            Assert.AreEqual(2, _parseResult.Count());
        
        [Test]
        public void Then_First_ParseResult_LarsId_Matches_Input() =>
            Assert.AreEqual(RoutePathMappingConstants.LarsId, _parseResult.First().LarsId);
        
        [Test]
        public void Then_First_ParseResult_Title_Matches_Input() =>
            Assert.AreEqual(RoutePathMappingConstants.Title, _parseResult.First().Title);

        [Test]
        public void Then_First_ParseResult_ShortTitle_Matches_Input() =>
            Assert.AreEqual(RoutePathMappingConstants.ShortTitle, _parseResult.First().ShortTitle);

        [Test]
        public void Then_First_ParseResult_PathId_Matches_Input() =>
            Assert.AreEqual(2, _parseResult.First().PathId);

        [Test]
        public void Then_Second_ParseResult_LarsId_Matches_Input() =>
            Assert.AreEqual(RoutePathMappingConstants.LarsId, _parseResult.Skip(1).First().LarsId);

        [Test]
        public void Then_Second_ParseResult_Title_Matches_Input() =>
            Assert.AreEqual(RoutePathMappingConstants.Title, _parseResult.Skip(1).First().Title);

        [Test]
        public void Then_Second_ParseResult_ShortTitle_Matches_Input() =>
            Assert.AreEqual(RoutePathMappingConstants.ShortTitle, _parseResult.Skip(1).First().ShortTitle);

        [Test]
        public void Then_Second_First_ParseResult_PathId_Matches_Input() =>
            Assert.AreEqual(5, _parseResult.Skip(1).First().PathId);
    }
}