using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Web.Controllers;

namespace Sfa.Tl.Matching.Web.UnitTests.Fixtures
{
    public class OpportunityProximityControllerFixture
    {
        internal IOpportunityService OpportunityService;
        internal IRoutePathService RouteService;
        internal IOpportunityProximityService OpportunityProximityService;
        internal ILocationService LocationService;
        
        internal OpportunityProximityController Sut;

        public OpportunityProximityControllerFixture()
        {
            OpportunityService = Substitute.For<IOpportunityService>();
            RouteService = Substitute.For<IRoutePathService>();
            LocationService = Substitute.For<ILocationService>();
            OpportunityProximityService = Substitute.For<IOpportunityProximityService>();
            
            Sut = new OpportunityProximityController(RouteService, OpportunityProximityService, OpportunityService,
                LocationService);
        }
    }
}
