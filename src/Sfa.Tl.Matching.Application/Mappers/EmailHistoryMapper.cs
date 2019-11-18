using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class EmailHistoryMapper : Profile
    {
        public EmailHistoryMapper()
        {
            CreateMap<EmailHistory, EmailHistoryDto>().ForPath(m => m.EmailTemplateName,
                o => o.MapFrom(s => s.EmailTemplate.TemplateName));
        }
    }
}