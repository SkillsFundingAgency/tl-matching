using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.FileReader.RoutePathMapping
{
    public class RoutePathMappingDataParser : IDataParser<RoutePathMappingDto>
    {
        public IEnumerable<RoutePathMappingDto> Parse(string[] cells)
        {
            var pathvalues = cells.Skip(RoutePathMappingColumnIndex.PathStartIndex)
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c =>
                    new RoutePathMappingDto
                    {
                        LarsId = cells[RoutePathMappingColumnIndex.LarsId],
                        Title = cells[RoutePathMappingColumnIndex.Title],
                        ShortTitle = cells[RoutePathMappingColumnIndex.ShortTitle],
                        PathId = c.ToInt()
                    }
                );

            return pathvalues;
        }
    }
}
