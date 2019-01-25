using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IRoutePathService
    {
        Task<IEnumerable<Path>> GetPathsAsync();

        Task<IEnumerable<Route>> GetRoutesAsync();
    }
}
