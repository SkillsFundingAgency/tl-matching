using Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Constants;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders
{
    internal class ValidQualificationRoutePathMappingFileImportDtoBuilder
    {
        private readonly QualificationRoutePathMappingFileImportDto _routePathMappingDto;

        public ValidQualificationRoutePathMappingFileImportDtoBuilder()
        {
            _routePathMappingDto = new QualificationRoutePathMappingFileImportDto
            {
                LarsId = RoutePathMappingConstants.LarsId,
                Title = RoutePathMappingConstants.Title,
                ShortTitle = RoutePathMappingConstants.ShortTitle,
                Accounting = RoutePathMappingConstants.PathId.ToString()
            };
        }

        public QualificationRoutePathMappingFileImportDto Build() =>
            _routePathMappingDto;
    }
}