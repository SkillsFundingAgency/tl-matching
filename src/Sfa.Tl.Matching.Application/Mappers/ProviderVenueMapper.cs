using AutoMapper;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class ProviderVenueMapper : Profile
    {
        public ProviderVenueMapper()
        {
            CreateMap<ProviderVenueDto, Domain.Models.ProviderVenue>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.Provider, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;
        }
    }
}