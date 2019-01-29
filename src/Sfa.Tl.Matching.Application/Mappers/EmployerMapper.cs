using AutoMapper.Configuration;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class EmployerMapper : MapperConfigurationExpression
    {
        public EmployerMapper()
        {
            CreateMap<EmployerDto, Domain.Models.Employer>();
        }
    }
}