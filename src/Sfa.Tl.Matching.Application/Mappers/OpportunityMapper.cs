using System;
using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using System.Linq;
using Sfa.Tl.Matching.Application.Mappers.Resolver;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class OpportunityMapper : Profile
    {
        public OpportunityMapper()
        {
            CreateMap<OpportunityDto, Opportunity>()
                .ForMember(m => m.EmployerId, o => o.MapFrom(s => s.EmployerId))
                .ForMember(m => m.EmployerContact, o => o.MapFrom(s => s.EmployerContact))
                .ForMember(m => m.EmployerContactEmail, o => o.MapFrom(s => s.EmployerContactEmail))
                .ForMember(m => m.EmployerContactPhone, o => o.MapFrom(s => s.EmployerContactPhone))
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<OpportunityDto, Opportunity>>())
                .ForAllOtherMembers(config => config.Ignore())
                ;

            CreateMap<OpportunityItemDto, OpportunityItem>()
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.OpportunityType, config =>
                    config.MapFrom(s => s.OpportunityType.ToString()))
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.RouteId))
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                .ForMember(m => m.JobRole, o => o.MapFrom(s => s.JobRole))
                .ForMember(m => m.Placements,
                    opt => opt.MapFrom(src => src.PlacementsKnown.HasValue && src.PlacementsKnown.Value ?
                        src.Placements : null))
                .ForMember(m => m.PlacementsKnown, o => o.MapFrom(s => s.PlacementsKnown))
                .ForMember(m => m.SearchResultProviderCount, o => o.MapFrom(s => s.SearchResultProviderCount))
                .ForMember(m => m.IsSaved, o => o.MapFrom(s => s.IsSaved))
                .ForMember(m => m.IsSelectedForReferral, o => o.MapFrom(s => s.IsSelectedForReferral))
                .ForMember(m => m.IsCompleted, o => o.MapFrom(s => s.IsCompleted))
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<OpportunityItemDto, OpportunityItem>>())
                .ForAllOtherMembers(config => config.Ignore())
                ;

            CreateMap<ReferralDto, Referral>()
                .ForMember(m => m.OpportunityItemId, o => o.Ignore())
                .ForMember(m => m.OpportunityItem, o => o.Ignore())
                .ForMember(m => m.ProviderVenue, o => o.Ignore())
                .ForMember(m => m.Id, o => o.Ignore())
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<ReferralDto, Referral>>())
                .ForMember(m => m.CreatedOn, o => o.Ignore())
                .ForMember(m => m.ModifiedOn, o => o.Ignore())
                .ForMember(m => m.ModifiedBy, o => o.Ignore())
                .ForAllOtherMembers(config => config.Ignore())
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
                //.ForMember(m => m.EmployerCrmId, o => o.MapFrom(s => s.EmployerCrmId))
                .ForMember(m => m.EmployerContact, o => o.MapFrom(s => s.CompanyName))
                .ForMember(dest => dest.EmployerContact, opt => opt.MapFrom((src, dest) =>
                    src.HasChanged ? string.Empty : dest.EmployerContact))
                .ForMember(dest => dest.EmployerContactEmail, opt => opt.MapFrom((src, dest) =>
                    src.HasChanged ? string.Empty : dest.EmployerContactEmail))
                .ForMember(dest => dest.EmployerContactPhone, opt => opt.MapFrom((src, dest) =>
                    src.HasChanged ? string.Empty : dest.EmployerContactPhone))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<Opportunity, OpportunityDto>()
                .ForMember(m => m.Id, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.EmployerId, o => o.MapFrom(s => s.EmployerId))
                .ForMember(m => m.EmployerContact, o => o.MapFrom(s => s.EmployerContact))
                .ForMember(m => m.EmployerContactEmail, o => o.MapFrom(s => s.EmployerContactEmail))
                .ForMember(m => m.EmployerContactPhone, o => o.MapFrom(s => s.EmployerContactPhone))
                //.ForPath(m => m.CompanyName, opt => opt.MapFrom(source => source.Employer.CompanyName))
                //.ForPath(m => m.EmployerCrmId, opt => opt.MapFrom(source => source.Employer.CrmId))
                .ForAllOtherMembers(config => config.Ignore())
                ;

            CreateMap<OpportunityItem, OpportunityItemDto>()
                .ForMember(m => m.Id, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.OpportunityId))
                //.ForPath(m => m.OpportunityType,
                //    opt => opt.MapFrom(source => source.Referral.Any() ?
                //        OpportunityType.Referral : OpportunityType.ProvisionGap))
                .ForMember(m => m.OpportunityType, config =>
                    config.MapFrom(s => ((OpportunityType)Enum.Parse(typeof(OpportunityType), s.OpportunityType))))
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.RouteId))
                .ForPath(m => m.RouteName, opt => opt.MapFrom(source => source.Route.Name))
                .ForPath(m => m.IsReferral, opt => opt.MapFrom(source => source.Referral.Any()))
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                .ForMember(m => m.JobRole, o => o.MapFrom(s => s.JobRole))
                .ForMember(m => m.Placements, o => o.MapFrom(s => s.Placements))
                .ForMember(m => m.PlacementsKnown, o => o.MapFrom(s => s.PlacementsKnown))
                .ForMember(m => m.SearchResultProviderCount, o => o.MapFrom(s => s.SearchResultProviderCount))
                .ForMember(m => m.IsSaved, o => o.MapFrom(s => s.IsSaved))
                .ForMember(m => m.IsSelectedForReferral, o => o.MapFrom(s => s.IsSelectedForReferral))
                .ForMember(m => m.IsCompleted, o => o.MapFrom(s => s.IsCompleted))
                .ForAllOtherMembers(config => config.Ignore())
                ;

            CreateMap<OpportunityItem, CheckAnswersDto>()
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.RouteId))
                // TODO FIX .ForMember(m => m.EmployerName, o => o.MapFrom(s => s.EmployerName))
                .ForMember(m => m.JobRole, o => o.MapFrom(s => s.JobRole))
                .ForMember(m => m.Placements, o => o.MapFrom(s => s.Placements))
                .ForMember(m => m.PlacementsKnown, o => o.MapFrom(s => s.PlacementsKnown))
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                .ForPath(m => m.RouteName, o => o.MapFrom(s => s.Route.Name))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<CheckAnswersDto, Opportunity>()
                //.ForMember(m => m.ConfirmationSelected, o => o.MapFrom(s => s.ConfirmationSelected))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<OpportunityItem, PlacementInformationSaveDto>()
                .ForMember(m => m.OpportunityItemId, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.RouteId))
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                .ForMember(m => m.SearchResultProviderCount, o => o.MapFrom(s => s.SearchResultProviderCount))
                //TODO: Will need to map path via Opportunity and Employer
                //.ForPath(m => m.CompanyName,
                //    opt => opt.MapFrom(source => source.Opportunity.Employer?.CompanyName))
                .ForMember(m => m.OpportunityType, config =>
                    config.MapFrom(s => ((OpportunityType)Enum.Parse(typeof(OpportunityType), s.OpportunityType))))
                .ForMember(m => m.JobRole, o => o.MapFrom(s => s.JobRole))
                .ForMember(m => m.Placements,
                    opt => opt.MapFrom(src => src.PlacementsKnown.HasValue && src.PlacementsKnown.Value ?
                        src.Placements : null))
                .ForMember(m => m.PlacementsKnown, o => o.MapFrom(s => s.PlacementsKnown))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<PlacementInformationSaveDto, OpportunityItem>()
                .ForMember(m => m.JobRole,
                    o => o.MapFrom(s => string.IsNullOrEmpty(s.JobRole) ?
                        "None given" : s.JobRole))
                .ForMember(m => m.PlacementsKnown, o => o.MapFrom(s => s.PlacementsKnown))
                .ForMember(m => m.Placements,
                    opt => opt.MapFrom(s => s.PlacementsKnown.HasValue && s.PlacementsKnown.Value ?
                        s.Placements : 1))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            //TODO: Probably don't need this map any more
            CreateMap<ProviderSearchDto, Opportunity>()
                .ForMember(m => m.Id, o => o.MapFrom(s => s.OpportunityId))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<ProviderSearchDto, OpportunityItem>()
                .ForMember(m => m.Id, o => o.MapFrom(s => s.OpportunityItemId))
                //.ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.RouteId))
                .ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                .ForMember(m => m.SearchResultProviderCount, o => o.MapFrom(s => s.SearchResultProviderCount))
                .ForAllOtherMembers(config => config.Ignore());
        }
    }
}