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
        }
    }
}