using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions
{
    public static class RoutePathMappingExtensions
    {
        public static QualificationRoutePathMappingFileImportDto ToDto(this Domain.Models.RoutePathMapping routePathMapping)
        {
            var routePathMappingArray = new QualificationRoutePathMappingFileImportDto
            {
                LarsId = routePathMapping.LarsId,
                Title = routePathMapping.Title,
                ShortTitle = routePathMapping.ShortTitle,
                Accountancy = routePathMapping.PathId.ToString(),
                Source = "Test"
            };

            return routePathMappingArray;
        }

        public static QualificationRoutePathMappingFileImportDto[] ToDto(this Domain.Models.RoutePathMapping routePathMapping, int numberOfElements)
        {
            var routePathMappingArray = new QualificationRoutePathMappingFileImportDto[numberOfElements];

            new[]
                {
                    new QualificationRoutePathMappingFileImportDto
                        {
                            LarsId = routePathMapping.LarsId,
                            Title = routePathMapping.Title,
                            ShortTitle = routePathMapping.ShortTitle,
                            Accountancy = routePathMapping.PathId.ToString()
                        }
                }
                .CopyTo(routePathMappingArray, 0);

            return routePathMappingArray;
        }

    }
}