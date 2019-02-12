using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.RoutePathMapping
{
    public class QualificationRoutePathMappingDataParser : IDataParser<RoutePathMappingDto>
    {
        public IEnumerable<RoutePathMappingDto> Parse(FileImportDto dto)
        {
            if (!(dto is QualificationRoutePathMappingFileImportDto data)) return null;

            var pathIds = data.GetQualificationRoutePathMappingPathIdColumns();
            var pathvalues = pathIds.Select(prop =>
                    new RoutePathMappingDto
                    {
                        LarsId = data.LarsId,
                        Title = data.Title,
                        ShortTitle = data.ShortTitle,
                        PathId = prop.GetValue(data).ToString().ToInt(),
                        Source = data.Source,
                        CreatedBy = data.CreatedBy
                    }
                );

            return pathvalues;
        }
    }
}
