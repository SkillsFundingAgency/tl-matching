using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Mappers
{
    public class RouteMapper : Profile
    {
        public RouteMapper()
        {
            CreateMap<Route, SelectListItem>()
                .ForMember(m => m.Text, o => o.MapFrom(s => s.Id.ToString()))
                .ForMember(m => m.Value, o => o.MapFrom(s => s.Name))
                ;

            CreateMap<Route, RouteSummaryViewModel>()
                .ForMember(dest => dest.IsSelected, opt => opt.Ignore());
        }
    }
}
