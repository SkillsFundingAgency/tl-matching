using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.Extensions
{
    public static class FileImportDtoExtensions
    {
        public static IEnumerable<PropertyInfo> GetQualificationRoutePathMappingPathIdColumns(this QualificationRoutePathMappingFileImportDto data)
        {
            var pathIds = data.GetType().GetProperties()
                .Where(pr => pr.GetCustomAttribute<ColumnAttribute>() != null)
                .SkipWhile(info => info.Name == nameof(QualificationRoutePathMappingFileImportDto.LarsId) ||
                                   info.Name == nameof(QualificationRoutePathMappingFileImportDto.Title) ||
                                   info.Name == nameof(QualificationRoutePathMappingFileImportDto.ShortTitle))
                .TakeWhile(info => info.Name != nameof(QualificationRoutePathMappingFileImportDto.Source))
                .Where(pr => pr.GetValue(data) != null && !string.IsNullOrWhiteSpace(pr.GetValue(data).ToString()));

            return pathIds;
        }
    }
}
