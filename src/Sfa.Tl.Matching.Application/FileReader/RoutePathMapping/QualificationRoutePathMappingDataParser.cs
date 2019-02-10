using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
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

            var props = data.GetType().GetProperties();

            var props1 = props.Where(pr => pr.GetCustomAttribute<ColumnAttribute>() != null).ToList();
            var props2 = props1.SkipWhile(info => info.Name == nameof(QualificationRoutePathMappingFileImportDto.LarsId) ||
                                                  info.Name == nameof(QualificationRoutePathMappingFileImportDto.Title) ||
                                                  info.Name == nameof(QualificationRoutePathMappingFileImportDto.ShortTitle)).ToList();
            var props3 = props2.Where(pr => pr.GetValue(data) != null && !string.IsNullOrWhiteSpace(pr.GetValue(data).ToString())).ToList();

            var pathvalues = props3.Select(prop =>
                    new RoutePathMappingDto
                    {
                        LarsId = data.LarsId,
                        Title = data.Title,
                        ShortTitle =data.ShortTitle,
                        PathId = prop.GetValue(data).ToString().ToInt()
                    }
                );

            return pathvalues;
        }
    }
}
