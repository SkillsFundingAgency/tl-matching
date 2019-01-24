using System.Collections.Generic;
using System.Threading.Tasks;
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

        public async Task<IEnumerable<Path>> GetPathsAsync()
        {
            return await _repository.GetPathsAsync();
        }

        public async Task<IEnumerable<Route>> GetRoutesAsync()
        {
            return await _repository.GetRoutesAsync();
        }
    }
}
