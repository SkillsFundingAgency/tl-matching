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
                .ForMember(m => m.CreatedBy, o => o.MapFrom<LoggedInUserNameResolver<SaveProvisionGapViewModel, OpportunityDto>>())
                .ForMember(m => m.UserEmail, o => o.MapFrom<LoggedInUserEmailResolver<SaveProvisionGapViewModel, OpportunityDto>>())
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.SelectedRouteId))
                .ForMember(m => m.Id, o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.DropOffStage, o => o.Ignore())
                .ForMember(m => m.EmployerId, o => o.Ignore())
                .ForMember(m => m.EmployerCrmId, o => o.Ignore())
                .ForMember(m => m.CompanyName, o => o.Ignore())
                .ForMember(m => m.EmployerContact, o => o.Ignore())
                .ForMember(m => m.EmployerContactEmail, o => o.Ignore())
                .ForMember(m => m.EmployerContactPhone, o => o.Ignore())
                .ForMember(m => m.ConfirmationSelected, o => o.Ignore())
                .ForMember(m => m.CompanyName, o => o.Ignore())
                .ForMember(m => m.IsReferral, o => o.Ignore())
                .ForMember(m => m.JobTitle, o => o.Ignore())
                .ForMember(m => m.ModifiedBy, o => o.Ignore())
                .ForMember(m => m.ModifiedOn, o => o.Ignore())
                .ForMember(m => m.Referral, o => o.Ignore())
                .ForMember(m => m.Placements, o => o.Ignore())
                .ForMember(m => m.PlacementsKnown, o => o.Ignore())
                .ForMember(m => m.RouteName, o => o.Ignore())
                .ForMember(m => m.ReferralCount, o => o.Ignore()) // TODO Remove when DB structure is done
                .ForMember(m => m.ProvisionGapCount, o => o.Ignore()) // TODO Remove when DB structure is done
                .ForPath(o => o.OpportunityType, opt => opt.MapFrom(x => OpportunityType.ProvisionGap))
                ;

            CreateMap<SaveReferralViewModel, OpportunityDto>()
                .ForMember(m => m.CreatedBy, o => o.MapFrom<LoggedInUserNameResolver<SaveReferralViewModel, OpportunityDto>>())
                .ForMember(m => m.UserEmail, o => o.MapFrom<LoggedInUserEmailResolver<SaveReferralViewModel, OpportunityDto>>())
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.SelectedRouteId))
                .ForMember(m => m.Referral, o => o.MapFrom(s => s.SelectedProvider.Where(p => p.IsSelected)))
                .ForMember(m => m.Id, o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.DropOffStage, o => o.Ignore())
                .ForMember(m => m.EmployerId, o => o.Ignore())
                .ForMember(m => m.EmployerCrmId, o => o.Ignore())
                .ForMember(m => m.EmployerContact, o => o.Ignore())
                .ForMember(m => m.EmployerContactEmail, o => o.Ignore())
                .ForMember(m => m.EmployerContactPhone, o => o.Ignore())
                .ForMember(m => m.ConfirmationSelected, o => o.Ignore())
                .ForMember(m => m.CompanyName, o => o.Ignore())
                .ForMember(m => m.IsReferral, o => o.Ignore())
                .ForMember(m => m.JobTitle, o => o.Ignore())
                .ForMember(m => m.ModifiedBy, o => o.Ignore())
                .ForMember(m => m.ModifiedOn, o => o.Ignore())
                .ForMember(m => m.Placements, o => o.Ignore())
                .ForMember(m => m.PlacementsKnown, o => o.Ignore())
                .ForMember(m => m.RouteName, o => o.Ignore())
                .ForMember(m => m.ReferralCount, o => o.Ignore()) // TODO Remove when DB structure is done
                .ForMember(m => m.ProvisionGapCount, o => o.Ignore()) // TODO Remove when DB structure is done
                .ForPath(o => o.OpportunityType, opt => opt.MapFrom(x => OpportunityType.Referral))
                ;

            CreateMap<SelectedProviderViewModel, ReferralDto>()
                .ForMember(m => m.CreatedBy,
                    o => o.MapFrom<LoggedInUserNameResolver<SelectedProviderViewModel, ReferralDto>>())
                .ForMember(m => m.Name, o => o.Ignore())
                .ForMember(m => m.Postcode, o => o.Ignore())
                ;
        }
    }
}