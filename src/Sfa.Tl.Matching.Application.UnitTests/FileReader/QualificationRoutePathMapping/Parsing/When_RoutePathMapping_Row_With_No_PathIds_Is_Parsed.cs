using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Parsing
{
    public class When_RoutePathMapping_Row_With_No_PathIds_Is_Parsed
    {
        private IEnumerable<RoutePathMappingDto> _parseResult;

        [SetUp]
        public void Setup()
        {
            var routePathMapping = new ValidRoutePathMappingBuilder().Build();
            var routePathMappingStringArray = routePathMapping.ToStringArray();
            routePathMappingStringArray[RoutePathMappingColumnIndex.PathStartIndex] = "";

            var parser = new RoutePathMappingDataParser();
            _parseResult = parser.Parse(routePathMappingStringArray);
        }

        [Test]
        public void Then_ParseResult_Count_Is_Zero() =>
            Assert.Zero(_parseResult.Count());
    }
}