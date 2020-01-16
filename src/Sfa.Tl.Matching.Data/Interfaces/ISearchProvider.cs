using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface ISearchProvider
    {
        Task<IList<OpportunityProximitySearchResultViewModelItem>> SearchOpportunitiesByPostcodeProximityAsync(OpportunityProximitySearchParametersDto dto);

        Task<IList<OpportunityProximitySearchResultByRouteViewModelItem>> SearchOpportunitiesForOtherRoutesByPostcodeProximityAsync(OpportunityProximitySearchParametersDto dto);
        
        Task<IList<ProviderProximitySearchResultViewModelItem>> SearchProvidersByPostcodeProximityAsync(ProviderProximitySearchParametersDto dto);
        
        Task<ProviderProximityReportDto> SearchProvidersByPostcodeProximityForReportAsync(ProviderProximitySearchParametersDto dto);
    }
}
