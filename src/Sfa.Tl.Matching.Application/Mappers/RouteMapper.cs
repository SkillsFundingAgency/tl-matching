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
                .ForMember(m => m.Text, o => o.MapFrom(s => s.Name))
                .ForMember(m => m.Value, o => o.MapFrom(s => s.Id.ToString()))
                .ForMember(m => m.Disabled, config => config.Ignore())
                .ForMember(m => m.Group, config => config.Ignore())
                .ForMember(m => m.Selected, config => config.Ignore())
                ;

            CreateMap<Route, RouteSummaryViewModel>()
                .ForMember(m => m.Id, o => o.MapFrom(s => s.Id))
                .ForMember(m => m.Name, o => o.MapFrom(s => s.Name))
                .ForMember(m => m.Summary, o => o.MapFrom(s => s.Summary))
                .ForMember(m => m.IsSelected, config => config.Ignore())
                ;
        }
    }
}
