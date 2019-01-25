using System.Linq;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.Services
{
    public class RoutePathService : IRoutePathService
    {
        private readonly IRoutePathRepository _repository;
        
        public RoutePathService(IRoutePathRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<Path> GetPaths()
        {
            return _repository.GetPaths();
        }

        public IQueryable<Route> GetRoutes()
        {
            return _repository.GetRoutes();
        }
    }
}
