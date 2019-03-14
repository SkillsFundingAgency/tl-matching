using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.FileReader.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.FileReader.QualificationRoutePathMapping
{
    public class QualificationRoutePathMappingDataParser : IDataParser<QualificationRoutePathMappingDto>
    {
        public IEnumerable<QualificationRoutePathMappingDto> Parse(FileImportDto fileImportDto)
        {
            if (!(fileImportDto is QualificationRoutePathMappingFileImportDto data)) return null;

            var pathIdProperties = data.GetQualificationRoutePathMappingPathIdColumnProperties();
            var routePathMappingDto = pathIdProperties.Select(prop =>
            {
                var qualificationRoutePathMapping  = new QualificationRoutePathMappingDto
                {
                    PathId = prop.GetValue(data).ToString().ToInt(),
                    Source = data.Source,
                    CreatedBy = data.CreatedBy
                };

                if (data.QualificationId != 0)
                {
                    qualificationRoutePathMapping.QualificationId = data.QualificationId;
                }
                else
                {
                    qualificationRoutePathMapping.Qualification = new QualificationDto
                    {
                        LarsId = data.LarsId,
                        Title = data.Title,
                        ShortTitle = data.ShortTitle,
                        CreatedBy = data.CreatedBy
                    };
                }

                return qualificationRoutePathMapping;
            });

            return routePathMappingDto;
        }
    }
}
