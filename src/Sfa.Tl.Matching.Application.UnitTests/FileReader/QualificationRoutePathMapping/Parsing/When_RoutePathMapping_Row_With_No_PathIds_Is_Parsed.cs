using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Parsing
{
    public class When_RoutePathMapping_Row_With_No_PathIds_Is_Parsed
    {
        private IEnumerable<RoutePathMappingDto> _parseResult;

        [SetUp]
        public void Setup()
        {
            var routePathMappingDto = new ValidQualificationRoutePathMappingFileImportDtoBuilder().Build();
            routePathMappingDto.AgricultureLandManagementandProduction = null;
            
            var parser = new QualificationRoutePathMappingDataParser();
            _parseResult = parser.Parse(routePathMappingDto);
        }

        [Test]
        public void Then_ParseResult_Count_Is_Zero() =>
            Assert.Zero(_parseResult.Count());
    }
}