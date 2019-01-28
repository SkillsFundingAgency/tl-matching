using AutoMapper.Configuration;
using Sfa.Tl.Matching.Models;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class EmployerMapper : MapperConfigurationExpression
    {
        public EmployerMapper()
        {
            CreateMap<CreateEmployerDto, Domain.Models.Employer>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore());
        }
    }
}