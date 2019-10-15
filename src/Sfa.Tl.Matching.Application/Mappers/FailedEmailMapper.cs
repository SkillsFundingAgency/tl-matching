using System.Text;
using AutoMapper;
using Notify.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class FailedEmailMapper : Profile
    {
        public FailedEmailMapper()
        {
            CreateMap<Notification, FailedEmailDto>()
                .ForMember(m => m.Body, config => config.MapFrom(s => s.body))
                .ForMember(m => m.Subject, config => config.MapFrom(s => s.subject))
                .ForMember(m => m.FailedEmailType, config => config.MapFrom(s => GetFailedEmailType(s.status)))
                .ForAllOtherMembers(c => c.Ignore())
                ;
        }

        private static FailedEmailType GetFailedEmailType(string status)
        {
            switch (status)
            {
                case "permanent-failure":
                    return FailedEmailType.PermanentFailure;
                case "temporary-failure":
                    return FailedEmailType.TemporaryFailure;
                case "technical-failure":
                    return FailedEmailType.TechnicalFailure;
                default:
                    return FailedEmailType.TechnicalFailure;
            }
        }
    }
}