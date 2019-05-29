using AutoMapper;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Mappers
{
    public class RouteViewModelMapper : Profile
    {
        public RouteViewModelMapper()
        {
            CreateMap<Route, RouteViewModel>()
                .ForMember(dest => dest.PathNames, opt => opt.Ignore())
                .ForMember(dest => dest.IsSelected, opt => opt.Ignore());
        }
    }
}