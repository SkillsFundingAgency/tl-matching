using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class RoutePathMappingMapper : Profile
    {
        public RoutePathMappingMapper()
        {
            CreateMap<RoutePathMappingDto, Domain.Models.RoutePathMapping>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.Path, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;
        }
    }
}