using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath.Builders
{
    internal class ValidRoutePathMappingDtoListBuilder
    {
        private readonly IList<RoutePathMappingDto> _routePathMappingDtos;

        public ValidRoutePathMappingDtoListBuilder(int numberOfItems)
        {
            _routePathMappingDtos = new List<RoutePathMappingDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                _routePathMappingDtos.Add(new RoutePathMappingDto
                {
                    LarsId = $"{i + 1:0000000#}",
                    Title = $"Test Title {i + 1}",
                    ShortTitle = $"Test Short Title {i + 1}",
                    PathId = i + 1,
                    Source = "Test",
                    CreatedBy = "Test"
                });
            }
        }

        public IEnumerable<RoutePathMappingDto> Build() =>
            _routePathMappingDtos;
    }
}
