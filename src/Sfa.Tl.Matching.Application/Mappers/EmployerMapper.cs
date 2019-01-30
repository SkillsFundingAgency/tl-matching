using AutoMapper.Configuration;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class EmployerMapper : MapperConfigurationExpression
    {
        public EmployerMapper()
        {
            CreateMap<CreateEmployerDto, Employer>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore());
        }
    }
}