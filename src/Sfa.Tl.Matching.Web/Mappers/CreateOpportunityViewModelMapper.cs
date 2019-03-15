using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class CreateOpportunityViewModelMapper : Profile
    {
        public CreateOpportunityViewModelMapper()
        {
            CreateMap<CreateProvisionGapViewModel, OpportunityDto>()
                .ForMember(m => m.CreatedBy, o => o.MapFrom<LoggedInUserNameResolver<CreateProvisionGapViewModel, OpportunityDto>>())
                .ForMember(m => m.UserEmail, o => o.MapFrom<LoggedInUserEmailResolver<CreateProvisionGapViewModel, OpportunityDto>>())
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.SelectedRouteId))
                .ForMember(m => m.Id, o => o.Ignore())
                .ForMember(m => m.DropOffStage, o => o.Ignore())
                .ForMember(m => m.EmployerId, o => o.Ignore())
                .ForMember(m => m.EmployerName, o => o.Ignore())
                .ForMember(m => m.EmployerContact, o => o.Ignore())
                .ForMember(m => m.EmployerContactEmail, o => o.Ignore())
                .ForMember(m => m.EmployerContactPhone, o => o.Ignore())
                .ForMember(m => m.ConfirmationSelected, o => o.Ignore())
                .ForMember(m => m.EmployerName, o => o.Ignore())
                .ForMember(m => m.IsReferral, o => o.Ignore())
                .ForMember(m => m.JobTitle, o => o.Ignore())
                .ForMember(m => m.ModifiedBy, o => o.Ignore())
                .ForMember(m => m.ModifiedOn, o => o.Ignore())
                .ForMember(m => m.Referral, o => o.Ignore())
                .ForMember(m => m.Placements, o => o.Ignore())
                .ForMember(m => m.PlacementsKnown, o => o.Ignore())
                .ForMember(m => m.RouteName, o => o.Ignore())
                ;

            CreateMap<CreateReferralViewModel, OpportunityDto>()
                .ForMember(m => m.CreatedBy, o => o.MapFrom<LoggedInUserNameResolver<CreateReferralViewModel, OpportunityDto>>())
                .ForMember(m => m.UserEmail, o => o.MapFrom<LoggedInUserEmailResolver<CreateReferralViewModel, OpportunityDto>>())
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.SelectedRouteId))
                .ForMember(m => m.Id, o => o.Ignore())
                .ForMember(m => m.DropOffStage, o => o.Ignore())
                .ForMember(m => m.EmployerId, o => o.Ignore())
                .ForMember(m => m.EmployerContact, o => o.Ignore())
                .ForMember(m => m.EmployerContactEmail, o => o.Ignore())
                .ForMember(m => m.EmployerContactPhone, o => o.Ignore())
                .ForMember(m => m.ConfirmationSelected, o => o.Ignore())
                .ForMember(m => m.EmployerName, o => o.Ignore())
                .ForMember(m => m.IsReferral, o => o.Ignore())
                .ForMember(m => m.JobTitle, o => o.Ignore())
                .ForMember(m => m.ModifiedBy, o => o.Ignore())
                .ForMember(m => m.ModifiedOn, o => o.Ignore())
                .ForMember(m => m.Referral, o => o.Ignore())
                .ForMember(m => m.Referral, o => o.Ignore())
                .ForMember(m => m.Placements, o => o.Ignore())
                .ForMember(m => m.PlacementsKnown, o => o.Ignore())
                .ForMember(m => m.RouteName, o => o.Ignore())
                ;

            CreateMap<SelectedProviderViewModel, ReferralDto>();
        }
    }
}
