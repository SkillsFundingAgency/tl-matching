using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class EmployerMapper : Profile
    {
        public EmployerMapper()
        {
            CreateMap<EmployerDto, Domain.Models.Employer>()
                .ForMember(dest => dest.Id, maping => maping.Ignore());
        }
    }
}