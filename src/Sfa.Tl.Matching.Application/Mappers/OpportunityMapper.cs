using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class OpportunityMapper : Profile
    {
        public OpportunityMapper()
        {
            CreateMap<OpportunityDto, Opportunity>()
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.EmailHistory, o => o.Ignore())
                .ForMember(m => m.Route, config => config.Ignore())
                .ForMember(m => m.ProvisionGap, config => config.Ignore())
                ;
            
            CreateMap<ReferralDto, Referral>()
                .ForMember(m => m.OpportunityId, o => o.Ignore())
                .ForMember(m => m.Opportunity, o => o.Ignore())
                .ForMember(m => m.ProviderVenue, o => o.Ignore())
                .ForMember(m => m.Id, o => o.Ignore())
                .ForMember(m => m.CreatedOn, o => o.Ignore())
                .ForMember(m => m.ModifiedOn, o => o.Ignore())
                .ForMember(m => m.ModifiedBy, o => o.Ignore())
                ;

            CreateMap<EmployerDetailDto, Opportunity>()
                .ForMember(m => m.EmployerContact, o => o.MapFrom(s => s.EmployerContact))
                .ForMember(m => m.EmployerContactEmail, o => o.MapFrom(s => s.EmployerContactEmail))
                .ForMember(m => m.EmployerContactPhone, o => o.MapFrom(s => s.EmployerContactPhone))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<EmployerNameDto, Opportunity>()
                .ForMember(m => m.EmployerId, o => o.MapFrom(s => s.EmployerId))
                .ForMember(m => m.EmployerCrmId, o => o.MapFrom(s => s.EmployerCrmId))
                .ForMember(m => m.EmployerName, o => o.MapFrom(s => s.EmployerName))
                .ForMember(dest => dest.EmployerContact, opt => opt.Condition(src => src.HasChanged))
                .ForMember(dest => dest.EmployerContactEmail, opt => opt.Condition(src => src.HasChanged))
                .ForMember(dest => dest.EmployerContactPhone, opt => opt.Condition(src => src.HasChanged))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<Opportunity, OpportunityDto>()
                .ForPath(m => m.RouteName, opt => opt.MapFrom(source => source.Route.Name))
                .ForPath(m => m.IsReferral, opt => opt.MapFrom(source => source.Referral.Any()));

            CreateMap<Opportunity, CheckAnswersDto>()
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.ConfirmationSelected, o => o.MapFrom(s => s.ConfirmationSelected))
                .ForMember(m => m.EmployerContact, o => o.MapFrom(s => s.EmployerContact))
                .ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                .ForMember(m => m.EmployerName, o => o.MapFrom(s => s.EmployerName))
                .ForMember(m => m.JobTitle, o => o.MapFrom(s => s.JobTitle))
                .ForMember(m => m.Placements, o => o.MapFrom(s => s.Placements))
                .ForMember(m => m.PlacementsKnown, o => o.MapFrom(s => s.PlacementsKnown))
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForPath(m => m.RouteName, o => o.MapFrom(s => s.Route.Name))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<CheckAnswersDto, Opportunity>()
                .ForMember(m => m.ConfirmationSelected, o => o.MapFrom(s => s.ConfirmationSelected))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<Opportunity, PlacementInformationSaveDto>()
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.RouteId))
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                .ForMember(m => m.JobTitle, o => o.MapFrom(s => s.JobTitle))
                .ForMember(m => m.Placements,
                    opt => opt.MapFrom(src => src.PlacementsKnown.HasValue && src.PlacementsKnown.Value ? 
                        src.Placements : null))
                .ForMember(m => m.PlacementsKnown, o => o.MapFrom(s => s.PlacementsKnown))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<PlacementInformationSaveDto, Opportunity>()
                .ForMember(m => m.JobTitle, o => o.MapFrom(s => s.JobTitle))
                .ForMember(m => m.PlacementsKnown, o => o.MapFrom(s => s.PlacementsKnown))
                .ForMember(m => m.Placements,
                    opt => opt.MapFrom(s => s.PlacementsKnown.HasValue && s.PlacementsKnown.Value ? 
                        s.Placements : 1))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());
        }
    }
}