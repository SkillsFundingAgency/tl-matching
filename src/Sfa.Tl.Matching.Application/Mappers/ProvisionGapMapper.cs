using AutoMapper;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    // ReSharper disable once UnusedMember.Global
    public class ProvisionGapMapper : Profile
    {
        public ProvisionGapMapper()
        {
            CreateMap<CheckAnswersProvisionGapViewModel, ProvisionGap>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.Opportunity, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<CheckAnswersProvisionGapViewModel, ProvisionGap>>())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore());
        }
    }
}