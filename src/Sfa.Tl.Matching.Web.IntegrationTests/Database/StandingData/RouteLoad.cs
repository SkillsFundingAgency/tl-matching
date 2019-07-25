using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Web.IntegrationTests.Database.StandingData
{
    internal class RouteLoad
    {
        public static Route[] Create()
        {
            var routes = new[]
            {
                new Route
                {
                    Id = 1,
                    Name = "Route 1"
                },
                new Route
                {
                    Id = 2,
                    Name = "Route 2"
                }
            };

            return routes;
        }
    }
}