using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class EmployerMapper : Profile
    {
        public EmployerMapper()
        {
            CreateMap<EmployerDto, Domain.Models.Employer>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;
        }
    }
}