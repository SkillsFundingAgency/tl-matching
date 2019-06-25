using System.Linq;
using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;
// ReSharper disable UnusedMember.Global

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class CreateOpportunityViewModelMapper : Profile
    {
        public CreateOpportunityViewModelMapper()
        {
            CreateMap<SaveProvisionGapViewModel, OpportunityDto>()
                //.ForMember(m => m.RouteId, o => o.MapFrom(s => s.SelectedRouteId))
                .ForMember(m => m.Id, o => o.MapFrom(s => s.OpportunityId))
                //.ForPath(o => o.OpportunityType, opt => opt.MapFrom(x => OpportunityType.ProvisionGap))
                .ForAllOtherMembers(config => config.Ignore())
                ;

            CreateMap<SaveReferralViewModel, OpportunityDto>()
                //.ForMember(m => m.RouteId, o => o.MapFrom(s => s.SelectedRouteId))
                //.ForMember(m => m.Referral, o => o.MapFrom(s => s.SelectedProvider.Where(p => p.IsSelected)))
                .ForMember(m => m.Id, o => o.MapFrom(s => s.OpportunityId))
                //.ForMember(m => m.ReferralCount, o => o.Ignore()) // TODO Remove when DB structure is done
                //.ForMember(m => m.ProvisionGapCount, o => o.Ignore()) // TODO Remove when DB structure is done
                //.ForPath(o => o.OpportunityType, opt => opt.MapFrom(x => OpportunityType.Referral))
                .ForAllOtherMembers(config => config.Ignore())
                ;

            CreateMap<SelectedProviderViewModel, ReferralDto>()
                .ForMember(m => m.ProviderVenueId, o => o.MapFrom(x => x.ProviderVenueId))
                .ForMember(m => m.DistanceFromEmployer, o => o.MapFrom(x => x.DistanceFromEmployer))
                .ForAllOtherMembers(config => config.Ignore())
                ;
        }
    }
}