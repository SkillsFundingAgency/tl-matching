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
                .ForMember(m => m.CompanyName, config => config.Ignore())
                .ForMember(m => m.EmployerCrmId, config => config.Ignore())
                .ForMember(m => m.PrimaryContact, config => config.Ignore())
                .ForMember(m => m.Email, config => config.Ignore())
                .ForMember(m => m.Phone, config => config.Ignore())
                .ForMember(m => m.OpportunityItemCount, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                ;

            CreateMap<SaveProvisionGapViewModel, OpportunityItemDto>()
                .ForMember(m => m.OpportunityItemId, o => o.MapFrom(s => s.OpportunityItemId))
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.SelectedRouteId))
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                .ForMember(m => m.SearchResultProviderCount, o => o.MapFrom(s => s.SearchResultProviderCount))
                .ForMember(m => m.RouteName, config => config.Ignore())
                .ForMember(m => m.Town, config => config.Ignore())
                .ForMember(m => m.JobRole, config => config.Ignore())
                .ForMember(m => m.PlacementsKnown, config => config.Ignore())
                .ForMember(m => m.IsReferral, config => config.Ignore())
                .ForMember(m => m.Placements, config => config.Ignore())
                .ForMember(m => m.IsSaved, config => config.Ignore())
                .ForMember(m => m.IsSelectedForReferral, config => config.Ignore())
                .ForMember(m => m.IsCompleted, config => config.Ignore())
                .ForMember(m => m.EmployerFeedbackSent, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForPath(o => o.OpportunityType, opt => opt.MapFrom(x => OpportunityType.ProvisionGap))
                .ForPath(m => m.ProvisionGap, config => config.Ignore())
                .ForPath(m => m.Referral, config => config.Ignore())
                ;

            CreateMap<SaveReferralViewModel, OpportunityDto>()
                .ForMember(m => m.Id, o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.CompanyName, config => config.Ignore())
                .ForMember(m => m.EmployerCrmId, config => config.Ignore())
                .ForMember(m => m.PrimaryContact, config => config.Ignore())
                .ForMember(m => m.Email, config => config.Ignore())
                .ForMember(m => m.Phone, config => config.Ignore())
                .ForMember(m => m.OpportunityItemCount, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                ;

            CreateMap<SaveReferralViewModel, OpportunityItemDto>()
                .ForMember(m => m.OpportunityItemId, o => o.MapFrom(s => s.OpportunityItemId))
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.SelectedRouteId))
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                .ForMember(m => m.SearchResultProviderCount, o => o.MapFrom(s => s.SearchResultProviderCount))
                .ForPath(o => o.OpportunityType, opt => opt.MapFrom(x => OpportunityType.Referral))
                .ForMember(m => m.Referral, o => o.MapFrom(s => s.SelectedProvider.Where(p => p.IsSelected)))
                .ForMember(m => m.RouteName, config => config.Ignore())
                .ForMember(m => m.Town, config => config.Ignore())
                .ForMember(m => m.JobRole, config => config.Ignore())
                .ForMember(m => m.PlacementsKnown, config => config.Ignore())
                .ForMember(m => m.IsReferral, config => config.Ignore())
                .ForMember(m => m.Placements, config => config.Ignore())
                .ForMember(m => m.IsSaved, config => config.Ignore())
                .ForMember(m => m.IsSelectedForReferral, config => config.Ignore())
                .ForMember(m => m.IsCompleted, config => config.Ignore())
                .ForMember(m => m.EmployerFeedbackSent, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForPath(m => m.ProvisionGap, config => config.Ignore())
                ;

            CreateMap<SelectedProviderViewModel, ReferralDto>()
                .ForMember(m => m.ProviderVenueId, o => o.MapFrom(x => x.ProviderVenueId))
                .ForMember(m => m.DistanceFromEmployer, o => o.MapFrom(x => x.DistanceFromEmployer))
                .ForMember(m => m.Name, config => config.Ignore())
                .ForMember(m => m.Postcode, config => config.Ignore())
                .ForMember(m => m.ProviderDisplayName, config => config.Ignore())
                .ForMember(m => m.ProviderVenueName, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                ;
        }
    }
}