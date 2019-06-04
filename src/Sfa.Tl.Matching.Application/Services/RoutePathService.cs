using System.Linq;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.Services
{
    public class RoutePathService : IRoutePathService
    {
        private readonly IRepository<Route> _routeRepository;

        public RoutePathService(IRepository<Route> routeRepository)
        {
            _routeRepository = routeRepository;
        }

        public IQueryable<Route> GetRoutes()
        {
            return _routeRepository.GetMany();
        }
    }
}
