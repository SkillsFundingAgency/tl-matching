using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class ProviderMapper : Profile
    {
        public ProviderMapper()
        {
            CreateMap<ProviderDto, Provider>()
                .ForMember(m => m.Id, config => config.Ignore())
                .ForMember(m => m.ProviderVenue, config => config.Ignore())
                .ForMember(m => m.CreatedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedOn, config => config.Ignore())
                .ForMember(m => m.ModifiedBy, config => config.Ignore())
                ;

            CreateMap<Provider, ProviderDetailViewModel>()
                .ForMember(m => m.ProviderVenues, config => config.MapFrom(s => s.ProviderVenue));

            CreateMap<Provider, ProviderSearchResultDto>();
        }
    }
}