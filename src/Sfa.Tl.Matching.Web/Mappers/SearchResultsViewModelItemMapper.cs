using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class SearchResultsViewModelItemMapper : Profile
    {
        public SearchResultsViewModelItemMapper()
        {
            CreateMap <ProviderVenueSearchResultDto, SearchResultsViewModelItem>()
                .ForMember(dest => dest.IsSelected, opt => opt.Ignore())
                ;
        }
    }
}