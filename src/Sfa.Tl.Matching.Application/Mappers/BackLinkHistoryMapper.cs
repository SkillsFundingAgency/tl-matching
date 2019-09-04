using AutoMapper;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class BackLinkHistoryMapper : Profile
    {
        public BackLinkHistoryMapper()
        {
            CreateMap<BackLinkHistoryDto, BackLinkHistory>()
                .ForMember(m => m.Link, config => config.MapFrom(s => s.Link))
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<BackLinkHistoryDto, BackLinkHistory>>())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.MapFrom<LoggedInUserNameResolver<BackLinkHistoryDto, BackLinkHistory>>())
                .ForMember(m => m.ModifiedOn, config => config.MapFrom<UtcNowResolver<BackLinkHistoryDto, BackLinkHistory>>())
                .ForAllOtherMembers(config => config.Ignore());
        }
    }
}
