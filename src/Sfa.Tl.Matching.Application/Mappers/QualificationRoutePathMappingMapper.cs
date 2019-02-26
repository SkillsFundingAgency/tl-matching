using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class QualificationRoutePathMappingMapper : Profile
    {
        public QualificationRoutePathMappingMapper()
        {
            CreateMap<QualificationRoutePathMappingDto, QualificationRoutePathMapping>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.Path, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;
        }
    }
}