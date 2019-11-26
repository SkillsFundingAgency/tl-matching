using AutoMapper;
using Notify.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class EmailDeliveryStatusMapper : Profile
    {
        public EmailDeliveryStatusMapper()
        {
            CreateMap<Notification, EmailDeliveryStatusDto>()
                .ForMember(m => m.Body, config => config.MapFrom(s => s.body))
                .ForMember(m => m.Subject, config => config.MapFrom(s => s.subject))
                .ForMember(m => m.Status, config => config.MapFrom(s => s.status))
                .ForMember(m => m.EmailDeliveryStatusType, config => config.MapFrom(s => GetEmailDeliveryStatusType(s.status)))
                .ForAllOtherMembers(c => c.Ignore())
                ;
        }

        private static EmailDeliveryStatusType GetEmailDeliveryStatusType(string status)
        {
            switch (status)
            {
                case "permanent-failure":
                    return EmailDeliveryStatusType.PermanentFailure;
                case "temporary-failure":
                    return EmailDeliveryStatusType.TemporaryFailure;
                case "technical-failure":
                    return EmailDeliveryStatusType.TechnicalFailure;
                default:
                    return EmailDeliveryStatusType.TechnicalFailure;
            }
        }
    }
}