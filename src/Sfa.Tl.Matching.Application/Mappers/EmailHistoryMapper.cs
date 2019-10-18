using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class EmailHistoryMapper : Profile
    {
        public EmailHistoryMapper()
        {
            CreateMap<EmailHistoryDto, EmailHistory>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.NotificationId, config => config.Ignore())
                .ForMember(m => m.Status, config => config.Ignore())
                .ForMember(m => m.EmailTemplate, config => config.Ignore())
                .ForMember(m => m.Opportunity, config => config.Ignore())
                .ForMember(m => m.EmailPlaceholder, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;

            CreateMap<EmailHistory, EmailHistoryDto>()
                .ForMember(m => m.NotificationId, o => o.MapFrom(s => s.NotificationId))
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.EmailTemplateId, o => o.MapFrom(s => s.EmailTemplateId))
                .ForPath(m => m.EmailTemplateName, o => o.MapFrom(s => s.EmailTemplate.TemplateName))
                .ForMember(m => m.SentTo, o => o.MapFrom(s => s.SentTo))
                .ForMember(m => m.CopiedTo, o => o.MapFrom(s => s.CopiedTo))
                .ForMember(m => m.BlindCopiedTo, o => o.MapFrom(s => s.BlindCopiedTo))
                .ForMember(m => m.CreatedBy, o => o.MapFrom(s => s.CreatedBy))
                .ForAllOtherMembers(config => config.Ignore())
                ;
        }
    }
}