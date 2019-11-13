using AutoMapper;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class ServiceStatusHistoryMapper : Profile
    {
        public ServiceStatusHistoryMapper()
        {
            CreateMap<ServiceStatusHistoryViewModel, ServiceStatusHistory>()
                .ForMember(m => m.IsOnline, config => config.MapFrom(s => s.IsOnline))
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<ServiceStatusHistoryViewModel, ServiceStatusHistory>>())
                .ForAllOtherMembers(config => config.Ignore());
       }
    }
}