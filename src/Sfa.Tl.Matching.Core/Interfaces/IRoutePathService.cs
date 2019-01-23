using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Core.DomainModels;

namespace Sfa.Tl.Matching.Core.Interfaces
{
    public interface IRoutePathService
    {
        Task<IEnumerable<Path>> GetPathsAsync();

        Task<IEnumerable<Route>> GetRoutesAsync();
    }
}
