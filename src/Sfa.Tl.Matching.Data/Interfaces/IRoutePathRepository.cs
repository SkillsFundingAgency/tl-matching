using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IRoutePathRepository
    {
        Task<IEnumerable<Path>> GetPathsAsync();
        Task<IEnumerable<Route>> GetRoutesAsync();
    }
}
