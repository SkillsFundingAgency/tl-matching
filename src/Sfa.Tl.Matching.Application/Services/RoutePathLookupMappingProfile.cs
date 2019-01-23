using AutoMapper;
using Sfa.Tl.Matching.Core.DomainModels;
using Sfa.Tl.Matching.Data.Models;

namespace Sfa.Tl.Matching.Application.Services
{
    public class RoutePathLookupMappingProfile : Profile
    {
        public RoutePathLookupMappingProfile()
        {
            CreateMap<RoutePathLookup, Path>()
                .ForMember(dest => dest.Name,
                    opts => opts.MapFrom(s => s.Path))
                .ForMember(dest => dest.RoutePathId,
                    opts => opts.MapFrom(s => s.Id));

            CreateMap<RoutePathLookup, Route>()
                .ForMember(dest => dest.Name,
                    opts => opts.MapFrom(s => s.Route));
        }
    }
}
