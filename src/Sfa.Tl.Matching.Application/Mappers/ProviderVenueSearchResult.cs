using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class ProviderVenueSearchResultMapper : Profile
    {
        public ProviderVenueSearchResultMapper()
        {
            CreateMap<ProviderVenueSearchResultDto, ProviderVenueSearchResult>();
        }
    }
}