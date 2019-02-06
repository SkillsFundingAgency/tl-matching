using AutoMapper.Configuration;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class RoutePathMappingMapper : MapperConfigurationExpression
    {
        public RoutePathMappingMapper()
        {
            CreateMap<RoutePathMappingDto, Domain.Models.RoutePathMapping>();
        }
    }
}