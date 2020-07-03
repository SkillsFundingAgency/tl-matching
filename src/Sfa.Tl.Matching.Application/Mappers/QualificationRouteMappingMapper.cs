using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    // ReSharper disable once UnusedMember.Global
    public class QualificationRouteMappingMapper : Profile
    {
        public QualificationRouteMappingMapper()
        {
            CreateMap<QualificationRouteMappingDto, QualificationRouteMapping>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.Route, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;

            CreateMap<QualificationRouteMappingViewModel, QualificationRouteMapping>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.Route, config => config.Ignore())
                .ForMember(m => m.Qualification, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore());
        }
    }
}