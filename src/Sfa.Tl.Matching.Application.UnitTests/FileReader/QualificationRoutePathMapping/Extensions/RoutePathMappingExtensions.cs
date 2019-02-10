using Humanizer;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Extensions
{
    public static class RoutePathMappingExtensions
    {
        private const string Yes = "Yes";
        private const string No = "No";

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
    }
}