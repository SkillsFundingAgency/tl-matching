using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface ISearchProvider
    {
        Task<IEnumerable<ProviderVenueSearchResultDto>> SearchProvidersByPostcodeProximity(string postcode, int searchRadius, int routeId);
    }
}
