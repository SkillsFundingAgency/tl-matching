using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Application.IntegrationTests.OpportunityProximity
{
    public class OpportunityProximityControllerFixture : LocationApiClientFixture
    {
        public OpportunityProximityController OpportunityProximityController;
        public IRoutePathService RoutePathService; 
        public void GetOpportunityProximityController(string requestPostcode)
        {
            GetLocationApiClient(requestPostcode);

            var locationService = new LocationService(LocationApiClient);

            var opportunityProximityService = new OpportunityProximityService(Substitute.For<ISearchProvider>(), locationService);

            RoutePathService = Substitute.For<IRoutePathService>();

            var opportunityService = Substitute.For<IOpportunityService>();

            OpportunityProximityController = new OpportunityProximityController(RoutePathService, opportunityProximityService, opportunityService, locationService);
        }
    }
}