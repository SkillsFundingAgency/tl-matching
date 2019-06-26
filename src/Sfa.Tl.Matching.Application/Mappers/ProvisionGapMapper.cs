using AutoMapper;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    // ReSharper disable once UnusedMember.Global
    public class ProvisionGapMapper : Profile
    {
        public ProvisionGapMapper()
        {
            CreateMap<OpportunityItemDto, ProvisionGap>()
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<OpportunityItemDto, ProvisionGap>>())
                .ForAllOtherMembers(config => config.Ignore())
                ;
        }
    }
}