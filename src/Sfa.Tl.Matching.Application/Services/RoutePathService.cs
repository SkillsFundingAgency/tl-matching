using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Sfa.Tl.Matching.Core.DomainModels;
using Sfa.Tl.Matching.Core.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;

namespace Sfa.Tl.Matching.Application.Services
{
    public class RoutePathService : IRoutePathService
    {
        private readonly IRoutePathRepository _repository;
        private readonly IMapper _mapper;
        
        public RoutePathService(IRoutePathRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Path>> GetPathsAsync()
        {
            var routesAndPaths = await _repository.GetListAsync();

            var paths = _mapper.Map< IEnumerable<Path>>(routesAndPaths);

            //var paths = routesAndPaths.Select(p =>
            //    new Path { RoutePathId = p.Id, Name = p.Path });

            return paths;
        }

        public async Task<IEnumerable<Route>> GetRoutesAsync()
        {
            var routesAndPaths = await _repository.GetListAsync();

            //TODO: Add an id? Add a list of paths?
            //var routes = routesAndPaths
            //    .GroupBy(p => new { p.Route })
            //    .Select(g => new Route { Name = g.First().Route })
            //    .ToList();
            var routes = _mapper.Map<IEnumerable<Route>>(routesAndPaths);

            return routes;
        }
    }
}
