using System.Linq;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.Services
{
    public class RoutePathService : IRoutePathService
    {
        private readonly ILogger<RoutePathService> _logger;
        private readonly IRepository<Route> _routeRepository;
        private readonly IRepository<Path> _pathRepository;

        public RoutePathService(
            ILogger<RoutePathService> logger,
            IRepository<Route> routeRepository,
            IRepository<Path> pathRepository)
        {
            _logger = logger;
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
