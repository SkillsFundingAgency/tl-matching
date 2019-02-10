using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions
{
    public static class RoutePathMappingExtensions
    {
        public static QualificationRoutePathMappingFileImportDto ToDto(this Domain.Models.RoutePathMapping provider)
        {
            var routePathMappingArray = new QualificationRoutePathMappingFileImportDto
            {
                LarsId = provider.LarsId,
                Title = provider.Title,
                ShortTitle = provider.ShortTitle,
                Accounting = provider.PathId.ToString()
            };

            return routePathMappingArray;
        }

        public static QualificationRoutePathMappingFileImportDto[] ToDto(this Domain.Models.RoutePathMapping provider, int numberOfElements)
        {
            var routePathMappingArray = new QualificationRoutePathMappingFileImportDto[numberOfElements];

            new[]
                {
                    new QualificationRoutePathMappingFileImportDto
                        {
                            LarsId = provider.LarsId,
                            Title = provider.Title,
                            ShortTitle = provider.ShortTitle,
                            Accounting = provider.PathId.ToString()
                        }
                }
                .CopyTo(routePathMappingArray, 0);

            return routePathMappingArray;
        }

    }
}