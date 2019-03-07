using System.Collections.Generic;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface ISearchProvider
    {
        IEnumerable<ProviderVenueSearchResultDto> SearchProvidersByPostcodeProximity(ProviderSearchParametersDto dto);
    }
}
