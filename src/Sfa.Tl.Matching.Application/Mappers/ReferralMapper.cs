using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class ReferralMapper : Profile
    {
        public ReferralMapper()
        {
            CreateMap<CheckAnswersViewModel, Referral>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.Opportunity, config => config.Ignore())
                .ForMember(m => m.EmailHistory, config => config.Ignore())
                .ForMember(m => m.ProviderVenueId, config => config.Ignore())
                .ForMember(m => m.ProviderVenue, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore());
        }
    }
}