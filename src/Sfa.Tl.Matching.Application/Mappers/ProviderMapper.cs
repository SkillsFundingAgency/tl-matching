using AutoMapper.Configuration;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class ProviderMapper : MapperConfigurationExpression
    {
        public ProviderMapper()
        {
            CreateMap<ProviderDto, Domain.Models.Provider>();
        }
    }
}