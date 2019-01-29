using System.Linq;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IRoutePathRepository
    {
        IQueryable<Path> GetPaths();
        IQueryable<Route> GetRoutes();
    }
}
