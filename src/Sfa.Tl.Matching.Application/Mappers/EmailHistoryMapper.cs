using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class EmailHistoryMapper : Profile
    {
        public EmailHistoryMapper()
        {
            CreateMap<EmailHistory, EmailHistoryDto>()
                .ForMember(m => m.NotificationId, o => o.MapFrom(s => s.NotificationId))
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.EmailTemplateId, o => o.MapFrom(s => s.EmailTemplateId))
                .ForPath(m => m.EmailTemplateName, o => o.MapFrom(s => s.EmailTemplate.TemplateName))
                .ForMember(m => m.Status, o => o.MapFrom(s => s.Status))
                .ForMember(m => m.SentTo, o => o.MapFrom(s => s.SentTo))
                .ForMember(m => m.CopiedTo, o => o.MapFrom(s => s.CopiedTo))
                .ForMember(m => m.BlindCopiedTo, o => o.MapFrom(s => s.BlindCopiedTo))
                .ForMember(m => m.CreatedBy, o => o.MapFrom(s => s.CreatedBy))
                .ForAllOtherMembers(config => config.Ignore())
                ;
        }
    }
}