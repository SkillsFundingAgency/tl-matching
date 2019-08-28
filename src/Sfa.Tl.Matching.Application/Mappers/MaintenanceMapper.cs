using AutoMapper;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class MaintenanceMapper : Profile
    {
        public MaintenanceMapper()
        {
            CreateMap<MaintenanceViewModel, MaintenanceHistory>()
                .ForMember(m => m.IsOnline, config => config.MapFrom(s => s.IsOnline))
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<MaintenanceViewModel, MaintenanceHistory>>())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                ;
       }
    }
}