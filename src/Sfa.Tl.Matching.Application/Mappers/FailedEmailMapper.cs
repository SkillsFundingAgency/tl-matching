using AutoMapper;
using Notify.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class FailedEmailMapper : Profile
    {
        public FailedEmailMapper()
        {
            CreateMap<Notification, FailedEmailDto>()
                .ForMember(m => m.Body, config => config.MapFrom(s => s.body))
                .ForMember(m => m.Reason, config => config.MapFrom(s => s.status))
                .ForMember(m => m.Subject, config => config.MapFrom(s => s.subject))
                .ForAllOtherMembers(c => c.Ignore())
                ;
        }
    }
}