using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IRoutePathService
    {
        IQueryable<Path> GetPaths();

        IQueryable<Route> GetRoutes();

        Task<int> ImportQualificationPathMapping(System.IO.Stream stream);

        void IndexQualificationPathMapping();
    }
}
