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
                .ForMember(m => m.Route, config => config.Ignore())
                .ForMember(m => m.ProvisionGap, config => config.Ignore())
                .ForAllMembers(expression => expression.Condition((src, dest, sourceMember) => sourceMember != null && !sourceMember.GetType().IsDefaultValue(sourceMember)));
        }
    }
}