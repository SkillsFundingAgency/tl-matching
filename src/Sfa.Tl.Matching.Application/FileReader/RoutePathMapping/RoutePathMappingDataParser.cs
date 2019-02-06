using System.Collections.Generic;
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
            for (var i = RoutePathMappingColumnIndex.FirstPathId; i < cells.Length; i++)
            {
                if (cells[i].IsInt())
                {
                    var pathId = cells[i].ToInt();
                    if (pathId > 0)
                    {
                        fileRoutePathMapping.PathId = pathId;
                    }
                }
            }

            return fileRoutePathMapping;
        }

        public IEnumerable<RoutePathMappingDto> ParseToMany(string[] cells)
        {
            //TODO: Get a list based on paths
            throw new System.NotImplementedException();
        }
    }
}
