using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions
{
    public static class RoutePathMappingExtensions
    {
        public static string[] ToStringArray(this Domain.Models.RoutePathMapping provider)
        {
            var routePathMappingArray = new[]
            {
                provider.LarsId,
                provider.Title,
                provider.ShortTitle,
                provider.PathId.ToString()
            };

            return routePathMappingArray;
        }

        public static string[] ToStringArray(this Domain.Models.RoutePathMapping provider, int numberOfElements)
        {
            var routePathMappingArray = new string[numberOfElements];

            new[] 
                {
                    provider.LarsId,
                    provider.Title,
                    provider.ShortTitle,
                    provider.PathId.ToString()
                }
                .CopyTo(routePathMappingArray, 0);

            return routePathMappingArray;
        }

    }
}