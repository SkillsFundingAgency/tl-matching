using System.Collections.Generic;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.OpportunityProximity.Builders
{
    public class SearchResultsByRouteBuilder
    {
        public IList<OpportunityProximitySearchResultByRouteViewModelItem> Build() =>
            new List<OpportunityProximitySearchResultByRouteViewModelItem>
            {
                new()
                {
                    NumberOfResults = 1,
                    RouteName = "digital"
                },
                new()
                {
                    NumberOfResults = 2,
                    RouteName = "health and beauty"
                }
            };
    }
}
