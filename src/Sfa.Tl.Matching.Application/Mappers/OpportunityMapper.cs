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
                .ForAllMembers(expression => expression.Condition((src, dest, sourceMember) => sourceMember != null && !sourceMember.GetType().IsDefaultValue(sourceMember)));
            
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
                .ForMember(m => m.EmployerName, o => o.MapFrom(s => s.CompanyName))
                .ForMember(m => m.ModifiedBy, o => o.MapFrom(s => s.ModifiedBy))
                .ForMember(m => m.ModifiedOn, o => o.MapFrom(s => s.ModifiedOn))
                .ForAllOtherMembers(config => config.Ignore());

            CreateMap<Opportunity, OpportunityDto>()
                .ForPath(m => m.RouteName, opt => opt.MapFrom(source => source.Route.Name))
                .ForPath(m => m.IsReferral, opt => opt.MapFrom(source => source.Referral.Any()));

            CreateMap<CheckAnswersDto, Opportunity>()
                .ForMember(m => m.ConfirmationSelected, o => o.MapFrom(s => s.ConfirmationSelected))
                .ForAllOtherMembers(config => config.Ignore());
        }
    }
}