using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class ProvisionGapMapper : Profile
    {
        public ProvisionGapMapper()
        {
            CreateMap<CheckAnswersGapViewModel, ProvisionGap>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.Opportunity, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore());
        }
    }
}