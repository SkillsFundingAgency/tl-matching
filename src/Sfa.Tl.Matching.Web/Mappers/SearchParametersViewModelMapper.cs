using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class SearchParametersViewModelMapper : Profile
    {
        public SearchParametersViewModelMapper()
        {
            CreateMap<Route, SelectListItem>()
                .ForMember(dest => dest.Text, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.Value, opt => opt.MapFrom((source) => source.Id))
                ;
        }
    }
}