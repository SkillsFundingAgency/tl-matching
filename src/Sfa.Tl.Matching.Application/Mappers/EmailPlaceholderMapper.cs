using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class EmailPlaceholderMapper : Profile
    {
        public EmailPlaceholderMapper()
        {
            CreateMap<EmailPlaceholderDto, EmailPlaceholder>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.EmailHistory, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;
        }
    }
}