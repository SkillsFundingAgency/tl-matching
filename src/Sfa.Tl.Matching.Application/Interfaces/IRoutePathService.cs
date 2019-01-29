using System.Linq;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IRoutePathService
    {
        IQueryable<Path> GetPaths();

        IQueryable<Route> GetRoutes();
    }
}
