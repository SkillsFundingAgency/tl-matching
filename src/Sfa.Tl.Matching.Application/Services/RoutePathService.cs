using System.Linq;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.Services
{
    public class RoutePathService : IRoutePathService
    {
        private readonly IRepository<Route> _routeRepository;
        private readonly IRepository<Path> _pathRepository;

        public RoutePathService(
            IRepository<Route> routeRepository,
            IRepository<Path> pathRepository)
        {
            _pathRepository = pathRepository;
            _routeRepository = routeRepository;
        }

        public IQueryable<Path> GetPaths()
        {
            return _pathRepository.GetMany();
        }

        public IQueryable<Route> GetRoutes()
        {
            return _routeRepository.GetMany();
        }
    }
}
