using System.Collections.Generic;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.OpportunityProximity.Builders
{
    public class SearchResultsByRouteBuilder
    {
        public IList<SearchResultsByRouteViewModelItem> Build() =>
            new List<SearchResultsByRouteViewModelItem>
            {
                new SearchResultsByRouteViewModelItem
                {
                    NumberOfResults = 1,
                    RouteName = "digital"
                },
                new SearchResultsByRouteViewModelItem
                {
                    NumberOfResults = 2,
                    RouteName = "health and beauty"
                }
            };
    }
}
