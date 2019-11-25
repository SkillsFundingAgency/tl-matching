using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IOpportunityProximityService
    {
        Task<IList<OpportunityProximitySearchResultViewModelItem>> SearchOpportunitiesByPostcodeProximityAsync(OpportunityProximitySearchParametersDto dto);
        Task<IList<OpportunityProximitySearchResultByRouteViewModelItem>> SearchOpportunitiesForOtherRoutesByPostcodeProximityAsync(OpportunityProximitySearchParametersDto dto);
    }
}