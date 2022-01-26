using System;
using System.Linq;
using AutoMapper;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Mappers.Resolver;
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
            CreateMap<Opportunity, EmployerOpportunityViewModel>()
                .ForMember(m => m.Name, o => o.MapFrom(s => s.Employer.CompanyName))
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.LastUpdated, o => o.MapFrom(s => s.ModifiedOn != null ? s.ModifiedOn.Value.GetTimeWithDate("on") : s.CreatedOn.GetTimeWithDate("on")))
                ;

            CreateMap<OpportunityDto, Opportunity>()
                .ForMember(m => m.EmployerCrmId, o => o.MapFrom(s => s.EmployerCrmId))
                .ForMember(m => m.EmployerContact, o => o.MapFrom(s => s.PrimaryContact.ToTitleCase()))
                .ForMember(m => m.EmployerContactEmail, o => o.MapFrom(s => s.Email))
                .ForMember(m => m.EmployerContactPhone, o => o.MapFrom(s => s.Phone))
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<OpportunityDto, Opportunity>>())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForPath(m => m.Employer, config => config.Ignore())
                .ForPath(m => m.EmailHistory, config => config.Ignore())
                .ForPath(m => m.OpportunityItem, config => config.Ignore())
                ;

            CreateMap<OpportunityItemDto, OpportunityItem>()
                .ForMember(m => m.Placements, opt => opt.MapFrom(src => src.PlacementsKnown.HasValue && src.PlacementsKnown.Value ? src.Placements : null))
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<OpportunityItemDto, OpportunityItem>>())
                .ForMember(m => m.Opportunity, config => config.Ignore())
                .ForMember(m => m.Route, config => config.Ignore())
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.IsDeleted, config => config.Ignore());

            CreateMap<ReferralDto, Referral>()
                .ForMember(m => m.OpportunityItemId, o => o.Ignore())
                .ForMember(m => m.OpportunityItem, o => o.Ignore())
                .ForMember(m => m.ProviderVenue, o => o.Ignore())
                .ForMember(m => m.Id, o => o.Ignore())
                .ForMember(m => m.CreatedBy, config => config.MapFrom<LoggedInUserNameResolver<ReferralDto, Referral>>())
                .ForMember(m => m.CreatedOn, o => o.Ignore())
                .ForMember(m => m.ModifiedOn, o => o.Ignore())
                .ForMember(m => m.ModifiedBy, o => o.Ignore())
                ;

            CreateMap<ProvisionGapDto, ProvisionGap>()
                .ForMember(m => m.CreatedBy,
                    config => config.MapFrom<LoggedInUserNameResolver<ProvisionGapDto, ProvisionGap>>())
                .ForMember(m => m.NoSuitableStudent, config => config.MapFrom(s => s.NoSuitableStudent))
                .ForMember(m => m.HadBadExperience, config => config.MapFrom(s => s.HadBadExperience))
                .ForMember(m => m.ProvidersTooFarAway, config => config.MapFrom(s => s.ProvidersTooFarAway))
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.OpportunityItemId, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForPath(m => m.OpportunityItem, config => config.Ignore())
                ;

            CreateMap<EmployerDetailDto, Opportunity>()
                .ForMember(m => m.EmployerContact, o => o.MapFrom(s => s.PrimaryContact.ToTitleCase()))
                .ForMember(m => m.EmployerContactEmail, o => o.MapFrom(s => s.Email))
                .ForMember(m => m.EmployerContactPhone, o => o.MapFrom(s => s.Phone))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.EmployerCrmId, config => config.Ignore())
                .ForPath(m => m.Employer, config => config.Ignore())
                .ForPath(m => m.OpportunityItem, config => config.Ignore())
                .ForPath(m => m.EmailHistory, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                ;

            CreateMap<CompanyNameDto, Opportunity>()
                .ForMember(m => m.EmployerCrmId, o => o.MapFrom(s => s.EmployerCrmId))
                .ForMember(m => m.EmployerContact, o => o.MapFrom(s => s.CompanyName))
                .ForMember(dest => dest.EmployerContact,
                    opt => opt.MapFrom((src, dest) => src.HasChanged ? string.Empty : dest.EmployerContact))
                .ForMember(dest => dest.EmployerContactEmail,
                    opt => opt.MapFrom((src, dest) => src.HasChanged ? string.Empty : dest.EmployerContactEmail))
                .ForMember(dest => dest.EmployerContactPhone,
                    opt => opt.MapFrom((src, dest) => src.HasChanged ? string.Empty : dest.EmployerContactPhone))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.Id, config => config.Ignore())
                .ForPath(m => m.Employer, config => config.Ignore())
                .ForPath(m => m.EmailHistory, config => config.Ignore())
                .ForPath(m => m.OpportunityItem, config => config.Ignore())
                ;

            CreateMap<Opportunity, OpportunityDto>()
                .ForMember(m => m.Id, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.EmployerCrmId, o => o.MapFrom(s => s.EmployerCrmId))
                .ForMember(m => m.PrimaryContact, o => o.MapFrom(s => s.EmployerContact))
                .ForMember(m => m.Email, o => o.MapFrom(s => s.EmployerContactEmail))
                .ForMember(m => m.Phone, o => o.MapFrom(s => s.EmployerContactPhone))
                .ForPath(m => m.EmployerCrmId, o => o.MapFrom(s => s.Employer.CrmId))
                .ForPath(m => m.CompanyName, o => o.MapFrom(s => s.Employer.CompanyName))
                ;

            CreateMap<OpportunityItem, OpportunityItemDto>()
                .ForMember(m => m.OpportunityType,
                    config => config.MapFrom(s =>
                        ((OpportunityType)Enum.Parse(typeof(OpportunityType), s.OpportunityType))))
                .ForMember(m => m.OpportunityItemId, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.IsReferral, opt => opt.MapFrom(source => source.Referral.Any()))
                .ForMember(m => m.EmployerFeedbackSent, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ProvisionGap, config => config.Ignore())
                .ForMember(m => m.Referral, config => config.Ignore());

            CreateMap<OpportunityItem, CheckAnswersViewModel>()
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.OpportunityItemId, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.RouteId))
                .ForMember(m => m.CompanyName, o => o.MapFrom(s => s.Opportunity.Employer.CompanyName))
                .ForMember(m => m.CompanyNameAka, o => o.MapFrom(s => s.Opportunity.Employer.AlsoKnownAs))
                .ForMember(m => m.JobRole, o => o.MapFrom(s => s.JobRole))
                .ForMember(m => m.Placements, o => o.MapFrom(s => s.Placements))
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                .ForMember(m => m.RouteName, o => o.MapFrom(s => s.Route.Name))
                .ForPath(m => m.Providers, config => config.Ignore())
                ;

            CreateMap<CheckAnswersDto, OpportunityItem>()
                .ForMember(m => m.IsSaved, o => o.MapFrom(s => s.IsSaved))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForMember(m => m.OpportunityType, config => config.Ignore())
                .ForMember(m => m.Town, config => config.Ignore())
                .ForMember(m => m.SearchResultProviderCount, config => config.Ignore())
                .ForMember(m => m.IsSelectedForReferral, config => config.Ignore())
                .ForMember(m => m.IsCompleted, config => config.Ignore())
                .ForMember(m => m.IsDeleted, config => config.Ignore())
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForPath(m => m.Opportunity, config => config.Ignore())
                .ForPath(m => m.ProvisionGap, config => config.Ignore())
                .ForPath(m => m.Referral, config => config.Ignore())
                .ForPath(m => m.Route, config => config.Ignore())
                ;

            CreateMap<OpportunityItem, PlacementInformationSaveDto>()
                .ForMember(m => m.OpportunityItemId, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.OpportunityId))
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.RouteId))
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                .ForMember(m => m.SearchResultProviderCount, o => o.MapFrom(s => s.SearchResultProviderCount))
                .ForMember(m => m.JobRole, o => o.MapFrom(s => s.JobRole))
                .ForMember(m => m.PlacementsKnown, o => o.MapFrom(s => s.PlacementsKnown))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForMember(m => m.OpportunityType, config =>
                    config.MapFrom(s => ((OpportunityType)Enum.Parse(typeof(OpportunityType), s.OpportunityType))))
                .ForMember(m => m.Placements, opt =>
                    opt.MapFrom(src =>
                        src.PlacementsKnown.HasValue && src.PlacementsKnown.Value
                        ? src.Placements
                        : null))
                .ForPath(m => m.CompanyName, opt =>
                    opt.MapFrom(source =>
                        source.Opportunity.Employer != null
                            ? source.Opportunity.Employer.CompanyName
                            : null))
                .ForPath(m => m.CompanyNameAka, opt =>
                    opt.MapFrom(source =>
                        source.Opportunity.Employer != null
                            ? source.Opportunity.Employer.AlsoKnownAs
                            : null))
                .ForPath(m => m.NoSuitableStudent, o => o.MapFrom(s =>
                    s.ProvisionGap != null && s.ProvisionGap.Any()
                        ? s.ProvisionGap.First().NoSuitableStudent
                        : null))
                .ForPath(m => m.HadBadExperience, o => o.MapFrom(s =>
                    s.ProvisionGap != null && s.ProvisionGap.Any()
                        ? s.ProvisionGap.First().HadBadExperience
                        : null))
                .ForPath(m => m.ProvidersTooFarAway, o => o.MapFrom(s =>
                    s.ProvisionGap != null && s.ProvisionGap.Any()
                        ? s.ProvisionGap.First().ProvidersTooFarAway
                        : null))
                ;

            CreateMap<PlacementInformationSaveDto, OpportunityItem>()
                .ForMember(m => m.PlacementsKnown, o => o.MapFrom(s => s.PlacementsKnown))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForMember(m => m.JobRole,
                    o => o.MapFrom(s =>
                        string.IsNullOrEmpty(s.JobRole)
                            ? "None given"
                            : s.JobRole))
                .ForMember(m => m.Placements,
                    opt => opt.MapFrom(s =>
                        s.PlacementsKnown.HasValue && s.PlacementsKnown.Value
                            ? s.Placements
                            : 1))
                .ForMember(m => m.Town, config => config.Ignore())
                .ForMember(m => m.IsSelectedForReferral, config => config.Ignore())
                .ForMember(m => m.IsCompleted, config => config.Ignore())
                .ForMember(m => m.IsSaved, config => config.Ignore())
                .ForMember(m => m.IsDeleted, config => config.Ignore())
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForPath(m => m.Opportunity, config => config.Ignore())
                .ForPath(m => m.ProvisionGap, config => config.Ignore())
                .ForPath(m => m.Referral, config => config.Ignore())
                .ForPath(m => m.Route, config => config.Ignore())
                ;

            CreateMap<ProviderSearchDto, OpportunityItem>()
                .ForMember(m => m.Id, o => o.MapFrom(s => s.OpportunityItemId))
                .ForMember(m => m.Postcode, o => o.MapFrom(s => s.Postcode))
                .ForMember(m => m.RouteId, o => o.MapFrom(s => s.RouteId))
                .ForMember(m => m.SearchRadius, o => o.MapFrom(s => s.SearchRadius))
                .ForMember(m => m.SearchResultProviderCount, o => o.MapFrom(s => s.SearchResultProviderCount))
                .ForMember(m => m.Town, config => config.Ignore())
                .ForMember(m => m.IsSelectedForReferral, config => config.Ignore())
                .ForMember(m => m.IsCompleted, config => config.Ignore())
                .ForMember(m => m.IsSaved, config => config.Ignore())
                .ForMember(m => m.IsDeleted, config => config.Ignore())
                .ForMember(m => m.OpportunityType, config => config.Ignore())
                .ForMember(m => m.JobRole, config => config.Ignore())
                .ForMember(m => m.PlacementsKnown, config => config.Ignore())
                .ForMember(m => m.Placements, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForPath(m => m.Opportunity, config => config.Ignore())
                .ForPath(m => m.ProvisionGap, config => config.Ignore())
                .ForPath(m => m.Referral, config => config.Ignore())
                .ForPath(m => m.Route, config => config.Ignore())
                ;

            CreateMap<OpportunityItem, FindEmployerViewModel>()
                .ForMember(m => m.OpportunityItemId, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.OpportunityId, o => o.MapFrom(s => s.OpportunityId))
                .ForPath(m => m.SelectedEmployerCrmId, o => o.MapFrom(s => s.Opportunity.EmployerCrmId))
                .ForPath(m => m.CompanyName,
                    opt => opt.MapFrom(source =>
                        source.Opportunity.Employer != null
                            ? source.Opportunity.Employer.CompanyName
                            : null))
                .ForPath(m => m.PreviousCompanyName,
                    opt => opt.MapFrom(source =>
                        source.Opportunity.Employer != null
                            ? source.Opportunity.Employer.CompanyName
                            : null))
                .ForMember(m => m.AlsoKnownAs, config => config.Ignore())
                ;

            CreateMap<OpportunityItemIsSelectedForReferralDto, OpportunityItem>()
                .ForMember(m => m.Id, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.IsSelectedForReferral, o => o.MapFrom(s => s.IsSelectedForReferral))
                .ForMember(m => m.ModifiedBy, config => config.MapFrom<LoggedInUserNameResolver<OpportunityItemIsSelectedForReferralDto, OpportunityItem>>())
                .ForMember(m => m.ModifiedOn, config => config.MapFrom<UtcNowResolver<OpportunityItemIsSelectedForReferralDto, OpportunityItem>>())
                .ForMember(m => m.OpportunityId, config => config.Ignore())
                .ForMember(m => m.RouteId, config => config.Ignore())
                .ForMember(m => m.OpportunityType, config => config.Ignore())
                .ForMember(m => m.Town, config => config.Ignore())
                .ForMember(m => m.Postcode, config => config.Ignore())
                .ForMember(m => m.SearchRadius, config => config.Ignore())
                .ForMember(m => m.JobRole, config => config.Ignore())
                .ForMember(m => m.PlacementsKnown, config => config.Ignore())
                .ForMember(m => m.Placements, config => config.Ignore())
                .ForMember(m => m.SearchResultProviderCount, config => config.Ignore())
                .ForMember(m => m.IsCompleted, config => config.Ignore())
                .ForMember(m => m.IsSaved, config => config.Ignore())
                .ForMember(m => m.IsSelectedForReferral, config => config.Ignore())
                .ForMember(m => m.IsDeleted, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForPath(m => m.Opportunity, config => config.Ignore())
                .ForPath(m => m.ProvisionGap, config => config.Ignore())
                .ForPath(m => m.Referral, config => config.Ignore())
                .ForPath(m => m.Route, config => config.Ignore())
                ;

            CreateMap<OpportunityItemIsSelectedForCompleteDto, OpportunityItem>()
                .ForMember(m => m.Id, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.IsCompleted, o => o.MapFrom(s => true))
                .ForMember(m => m.ModifiedBy, config => config.MapFrom<LoggedInUserNameResolver<OpportunityItemIsSelectedForCompleteDto, OpportunityItem>>())
                .ForMember(m => m.ModifiedOn, config => config.MapFrom<UtcNowResolver<OpportunityItemIsSelectedForCompleteDto, OpportunityItem>>())
                .ForMember(m => m.OpportunityId, config => config.Ignore())
                .ForMember(m => m.RouteId, config => config.Ignore())
                .ForMember(m => m.OpportunityType, config => config.Ignore())
                .ForMember(m => m.Town, config => config.Ignore())
                .ForMember(m => m.Postcode, config => config.Ignore())
                .ForMember(m => m.SearchRadius, config => config.Ignore())
                .ForMember(m => m.JobRole, config => config.Ignore())
                .ForMember(m => m.PlacementsKnown, config => config.Ignore())
                .ForMember(m => m.Placements, config => config.Ignore())
                .ForMember(m => m.SearchResultProviderCount, config => config.Ignore())
                .ForMember(m => m.IsSaved, config => config.Ignore())
                .ForMember(m => m.IsSelectedForReferral, config => config.Ignore())
                .ForMember(m => m.IsDeleted, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForPath(m => m.Opportunity, config => config.Ignore())
                .ForPath(m => m.ProvisionGap, config => config.Ignore())
                .ForPath(m => m.Referral, config => config.Ignore())
                .ForPath(m => m.Route, config => config.Ignore())
                ;

            CreateMap<OpportunityItemIsSelectedWithUsernameForCompleteDto, OpportunityItem>()
                .ForMember(m => m.Id, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.IsCompleted, o => o.MapFrom(s => true))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.Username))
                .ForMember(m => m.ModifiedOn, config => config.MapFrom<UtcNowResolver<OpportunityItemIsSelectedWithUsernameForCompleteDto, OpportunityItem>>())
                .ForMember(m => m.OpportunityId, config => config.Ignore())
                .ForMember(m => m.RouteId, config => config.Ignore())
                .ForMember(m => m.Postcode, config => config.Ignore())
                .ForMember(m => m.SearchRadius, config => config.Ignore())
                .ForMember(m => m.SearchResultProviderCount, config => config.Ignore())
                .ForMember(m => m.Town, config => config.Ignore())
                .ForMember(m => m.IsSelectedForReferral, config => config.Ignore())
                .ForMember(m => m.IsSaved, config => config.Ignore())
                .ForMember(m => m.IsDeleted, config => config.Ignore())
                .ForMember(m => m.OpportunityType, config => config.Ignore())
                .ForMember(m => m.JobRole, config => config.Ignore())
                .ForMember(m => m.PlacementsKnown, config => config.Ignore())
                .ForMember(m => m.Placements, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForPath(m => m.Opportunity, config => config.Ignore())
                .ForPath(m => m.ProvisionGap, config => config.Ignore())
                .ForPath(m => m.Referral, config => config.Ignore())
                .ForPath(m => m.Route, config => config.Ignore())
                ;

            CreateMap<UsernameForFeedbackSentDto, Opportunity>()
                .ForMember(m => m.Id, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.Username))
                .ForMember(m => m.ModifiedOn,
                    config => config.MapFrom<UtcNowResolver<UsernameForFeedbackSentDto, Opportunity>>())
                .ForMember(m => m.EmployerCrmId, config => config.Ignore())
                .ForMember(m => m.EmployerContact, config => config.Ignore())
                .ForMember(m => m.EmployerContactEmail, config => config.Ignore())
                .ForMember(m => m.EmployerContactPhone, config => config.Ignore())
                .ForMember(m => m.CreatedBy, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForPath(m => m.Employer, config => config.Ignore())
                .ForPath(m => m.EmailHistory, config => config.Ignore())
                .ForPath(m => m.OpportunityItem, config => config.Ignore())
                ;
        }
    }
}