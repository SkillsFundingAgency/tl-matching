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
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.NoSuitableStudent, config => config.Ignore())
                .ForMember(m => m.HadBadExperience, config => config.Ignore())
                .ForMember(m => m.ProvidersTooFarAway, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForPath(m => m.OpportunityItem, config => config.Ignore())
                ;

            CreateMap<PlacementInformationSaveDto, ProvisionGap>()
                .ForMember(m => m.NoSuitableStudent, o => o.MapFrom(s => s.NoSuitableStudent))
                .ForMember(m => m.HadBadExperience, o => o.MapFrom(s => s.HadBadExperience))
                .ForMember(m => m.ProvidersTooFarAway, o => o.MapFrom(s => s.ProvidersTooFarAway))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForPath(m => m.OpportunityItem, config => config.Ignore())
                ;
        }
    }
}