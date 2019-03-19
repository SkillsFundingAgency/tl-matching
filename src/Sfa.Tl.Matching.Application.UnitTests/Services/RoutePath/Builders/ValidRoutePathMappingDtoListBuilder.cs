using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath.Builders
{
    public class ValidRoutePathMappingDtoListBuilder
    {
        public IList<QualificationRoutePathMappingDto> Build(int numberOfItems)
        {
            var routePathMappingDtos = new List<QualificationRoutePathMappingDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                routePathMappingDtos.Add(new QualificationRoutePathMappingDto
                {
                    Qualification = new QualificationDto
                    {
                        LarsId = $"{i + 1:0000000#}",
                        Title = $"Test Title {i + 1}",
                        ShortTitle = $"Test Short Title {i + 1}"
                    },
                    PathId = i + 1,
                    Source = "Test",
                    CreatedBy = "Test"
                });
            }

            return routePathMappingDtos;
        }
    }
}
