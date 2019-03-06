using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface ISearchProvider
    {
        IEnumerable<ProviderVenueSearchResultDto> SearchProvidersByPostcodeProximity(string postcode, int searchRadius, int routeId);
    }
}
