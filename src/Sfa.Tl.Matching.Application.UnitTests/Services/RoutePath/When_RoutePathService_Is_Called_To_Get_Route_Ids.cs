using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class When_RoutePathService_Is_Called_To_Get_Route_Ids
    {
        private readonly IRepository<Route> _routeRepository;
        private readonly IList<int> _result;

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
        
        public When_RoutePathService_Is_Called_To_Get_Route_Ids()
        {
            _routeRepository = Substitute.For<IRepository<Route>>();

            _routeRepository.GetManyAsync().Returns(_routeData);

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(RouteMapper).Assembly);
            });
            var mapper = new Mapper(config);

            IRoutePathService service = new RoutePathService(mapper, _routeRepository);

            _result = service.GetRouteIdsAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_Route_Repository_GetMany_Is_Called_Exactly_Once()
        {
            _routeRepository
                .Received(1)
                .GetManyAsync();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Items_Is_Returned()
        {
            Assert.Equal(_routeData.Count(), _result.Count());
        }

        [Fact]
        public void Then_The_Expected_Ids_Are_Returned()
        {
           _result[0].Should().Be(1);
           _result[1].Should().Be(2);
        }
    }
}
