using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface ISearchProvider
    {
        Task<IEnumerable<ProviderVenueSearchResult>> SearchProvidersByPostcodeProximity(string postcode, int searchRadius, int routeId);
    }
}
