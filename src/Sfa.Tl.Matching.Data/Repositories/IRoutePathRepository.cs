using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Data.Models;

namespace Sfa.Tl.Matching.Data.Repositories
{
    public interface IRoutePathRepository
    {
        Task<IEnumerable<RoutePathLookup>> GetListAsync();
    }
}
