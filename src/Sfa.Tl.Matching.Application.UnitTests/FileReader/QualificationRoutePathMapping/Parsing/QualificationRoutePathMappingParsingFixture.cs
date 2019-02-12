using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Parsing
{
    public class QualificationRoutePathMappingParsingFixture
    {
        public QualificationRoutePathMappingDataParser Parser;
        public QualificationRoutePathMappingFileImportDto Dto;

        public QualificationRoutePathMappingParsingFixture()
        {
            Dto = new ValidQualificationRoutePathMappingFileImportDtoBuilder().Build();
            Parser = new QualificationRoutePathMappingDataParser();
        }
    }
}