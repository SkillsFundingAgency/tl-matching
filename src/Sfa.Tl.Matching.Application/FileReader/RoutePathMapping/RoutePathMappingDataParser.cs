using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.RoutePathMapping
{
    public class RoutePathMappingDataParser : IDataParser<RoutePathMappingDto>
    {
        public RoutePathMappingDto Parse(string[] cells)
        {
            var fileRoutePathMapping = new RoutePathMappingDto
            {
                LarsId = cells[RoutePathMappingColumnIndex.LarsId],
                Title = cells[RoutePathMappingColumnIndex.Title],
                ShortTitle = cells[RoutePathMappingColumnIndex.ShortTitle],
            };

            //TODO: Loop over remaining columns and get a collection of path ids
            //TODO: get column count from worksheet
            //TODO: Make this code wok properly - currently returns the last value found.
            var lastColumn = 35;
            for (int i = RoutePathMappingColumnIndex.FirstPathId; i < lastColumn; i++)
            {
                var pathId = cells[i].ToInt();
                if (pathId > 0)
                {
                    fileRoutePathMapping.PathId = pathId;
                }
            }

            return fileRoutePathMapping;
        }
    }
}
