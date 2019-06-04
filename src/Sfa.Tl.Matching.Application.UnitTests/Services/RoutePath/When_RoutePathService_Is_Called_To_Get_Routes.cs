using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class When_RoutePathService_Is_Called_To_Get_Routes
    {
        private readonly IRepository<Route> _routeRepository;
        private readonly IQueryable<Route> _result;

        private readonly IQueryable<Route> _routeData
            = new List<Route>
                {
                    new Route
                    {
                        Id = 1,
                        Name = "Route 1",
                        Keywords = "Keyword1",
                        Summary = "Route 1 summary"
                    },
                    new Route
                    {
                        Id = 2,
                        Name = "Route 2",
                        Keywords = "Keyword2",
                        Summary = "Route 2 summary"
                    }
                }
                .AsQueryable();

        public readonly IEnumerable<Route> Expected
            = new List<Route>
            {
                new Route
                {
                    Id = 1,
                    Name = "Route 1",
                    Keywords = "Keyword1",
                    Summary = "Route 1 summary"
                },
                new Route
                { Id = 2,
                    Name = "Route 2",
                    Keywords = "Keyword2",
                    Summary = "Route 2 summary"
                }
            };

        public When_RoutePathService_Is_Called_To_Get_Routes()
        {
            _routeRepository = Substitute.For<IRepository<Route>>();

            _routeRepository.GetMany().Returns(_routeData);

            IRoutePathService service = new RoutePathService(_routeRepository);

            _result = service.GetRoutes();
        }

        [Fact]
        public void Then_Route_Repository_GetMany_Is_Called_Exactly_Once()
        {
            _routeRepository
                .Received(1)
                .GetMany();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Items_Is_Returned()
        {
            Assert.Equal(_routeData.Count(), _result.Count());
        }

        [Fact]
        public void Then_Route_Id_Is_Returned()
        {
            Assert.Equal(Expected.First().Id, _result.First().Id);
        }

        [Fact]
        public void Then_Route_Name_Is_Returned()
        {
            Assert.Equal(Expected.First().Name, _result.First().Name);
        }

        [Fact]
        public void Then_Route_Keywords_Is_Returned()
        {
            Assert.Equal(Expected.First().Keywords, _result.First().Keywords);
        }

        [Fact]
        public void Then_Route_Summary_Is_Returned()
        {
            Assert.Equal(Expected.First().Summary, _result.First().Summary);
        }
    }
}
