using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class QualificationMapper : Profile
    {
        public QualificationMapper()
        {
            CreateMap<QualificationDto, Qualification>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.ProviderQualification, config => config.Ignore())
                .ForMember(m => m.QualificationRoutePathMapping, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;
        }
    }
}