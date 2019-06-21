using AutoMapper;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    // ReSharper disable once UnusedMember.Global
    public class ProvisionGapMapper : Profile
    {
        public ProvisionGapMapper()
        {
            CreateMap<CheckAnswersProvisionGapViewModel, ProvisionGap>()
                .ForMember(m => m.OpportunityId, config => config.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<CheckAnswersProvisionGapViewModel, ProvisionGap>>())
                .ForAllOtherMembers(config => config.Ignore())
            ;

            CreateMap<OpportunityDto, ProvisionGap>()
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<OpportunityDto, ProvisionGap>>())
                .ForAllOtherMembers(config => config.Ignore())
                ;
        }
    }
}