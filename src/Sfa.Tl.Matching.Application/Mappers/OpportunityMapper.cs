using AutoMapper;
using Microsoft.EntityFrameworkCore.Internal;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class OpportunityMapper : Profile
    {
        public OpportunityMapper()
        {
            CreateMap<OpportunityDto, Opportunity>()
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.EmailHistory, o => o.Ignore())
                ;

            CreateMap<ReferralDto, Referral>()
                .ForMember(m => m.OpportunityItemId, o => o.Ignore())
                .ForMember(m => m.OpportunityItem, o => o.Ignore())
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
                //.ForPath(m => m.RouteName, opt => opt.MapFrom(source => source.Route.Name))
                //.ForPath(m => m.IsReferral, opt => opt.MapFrom(source => source.Referral.Any()))
                //.ForPath(m => m.OpportunityType,
                //    opt => opt.MapFrom(source => source.Referral.Any() ?
                //        OpportunityType.Referral : OpportunityType.ProvisionGap))
                ;

            CreateMap<Opportunity, CheckAnswersDto>()
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.Id))
                //.ForMember(m => m.ConfirmationSelected, o => o.MapFrom(s => s.ConfirmationSelected))
                //.ForMember(m => m.EmployerContact, o => o.MapFrom(s => s.EmployerContact))
                //.ForMember(m => m.RouteId, o => o.MapFrom(s => s.RouteId))
                //.ForMember(m => m.EmployerName, o => o.MapFrom(s => s.EmployerName))
                //.ForMember(m => m.JobTitle, o => o.MapFrom(s => s.JobTitle))
                //.ForMember(m => m.Placements, o => o.MapFrom(s => s.Placements))
                //.ForMember(m => m.PlacementsKnown, o => o.MapFrom(s => s.PlacementsKnown))
                //.ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                //.ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                //.ForPath(m => m.RouteName, o => o.MapFrom(s => s.Route.Name))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<CheckAnswersDto, Opportunity>()
                //.ForMember(m => m.ConfirmationSelected, o => o.MapFrom(s => s.ConfirmationSelected))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<Opportunity, PlacementInformationSaveDto>()
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.Id))
                //.ForMember(m => m.RouteId, o => o.MapFrom(s => s.RouteId))
                //.ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                //.ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                //.ForMember(m => m.CompanyName, o => o.MapFrom(s => s.EmployerName))
                //.ForPath(m => m.OpportunityType, 
                //    opt => opt.MapFrom(source => source.Referral.Any() ? 
                //        OpportunityType.Referral : OpportunityType.ProvisionGap))
                //.ForMember(m => m.JobTitle, o => o.MapFrom(s => s.JobTitle))
                //.ForMember(m => m.Placements,
                //    opt => opt.MapFrom(src => src.PlacementsKnown.HasValue && src.PlacementsKnown.Value ?
                //        src.Placements : null))
                //.ForMember(m => m.PlacementsKnown, o => o.MapFrom(s => s.PlacementsKnown))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<PlacementInformationSaveDto, Opportunity>()
                //.ForMember(m => m.JobTitle,
                //    o => o.MapFrom(s => string.IsNullOrEmpty(s.JobTitle) ? 
                //        "None given" : s.JobTitle))
                //.ForMember(m => m.PlacementsKnown, o => o.MapFrom(s => s.PlacementsKnown))
                //.ForMember(m => m.Placements,
                //    opt => opt.MapFrom(s => s.PlacementsKnown.HasValue && s.PlacementsKnown.Value ?
                //        s.Placements : 1))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<ProviderSearchDto, Opportunity>()
                //.ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                //.ForMember(m => m.RouteId, o => o.MapFrom(s => s.RouteId))
                //.ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                .ForMember(m => m.Id, o => o.MapFrom(s => s.OpportunityId))
                //.ForMember(m => m.SearchResultProviderCount, o => o.MapFrom(s => s.SearchResultProviderCount))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<Opportunity, OpportunityBasketViewModel>()
                //.ForMember(m => m.CompanyName, o => 
                //    o.MapFrom(s => s.EmployerName)) // TODO This will come from Employer table and not Opportunity when DB changes are in
                .ForMember(m => m.Type, config => 
                    config.MapFrom(s => GetOpportunityBasketType(2, 2))) // TODO Put correct values when Opportunity Model is updated
                .ForAllOtherMembers(config => config.Ignore())
                ;
        }

        private static OpportunityBasketType GetOpportunityBasketType(int referralCount, int provisionGapCount)
        {
            if (referralCount == 1 && provisionGapCount == 0)
                return OpportunityBasketType.ReferralSingleOnly;
            if (referralCount == 0 && provisionGapCount > 0)
                return OpportunityBasketType.ProvisionGapOnly;
            if (referralCount > 0 && provisionGapCount == 0)
                return OpportunityBasketType.ReferralMultipleOnly;
            if (referralCount == 1 && provisionGapCount > 0)
                return OpportunityBasketType.ReferralSingleAndProvisionGap;
            if (referralCount > 1 && provisionGapCount > 0)
                return OpportunityBasketType.ReferralMultipleAndProvisionGap;

            return OpportunityBasketType.ReferralSingleOnly;
        }
    }
}