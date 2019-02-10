using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class RoutePathMappingMapper : Profile
    {
        public RoutePathMappingMapper()
        {
            CreateMap<RoutePathMappingDto, Domain.Models.RoutePathMapping>()
                .ForMember(dest => dest.Id, mapping => mapping.Ignore())
                .ForMember(dest => dest.CreatedOn, mapping => mapping.Ignore())
                .ForMember(dest => dest.ModifiedOn, mapping => mapping.Ignore())
                .ForMember(dest => dest.Path, mapping => mapping.Ignore());
        }
    }
}