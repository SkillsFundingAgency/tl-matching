using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class RouteViewModelMapper : Profile
    {
        public RouteViewModelMapper()
        {
            CreateMap<Route, RouteSummaryViewModel>()
                .ForMember(dest => dest.IsSelected, opt => opt.Ignore());
        }
    }
}