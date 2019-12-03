using System.Collections.Generic;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class When_RoutePathService_Is_Called_To_Get_Route_Ids
    {
        private readonly IList<int> _result;

        public When_RoutePathService_Is_Called_To_Get_Route_Ids()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Route>>>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(RouteMapper).Assembly);
            });
            var mapper = new Mapper(config);

            using (var dbContext = InMemoryDbContext.Create())
            {
                dbContext.AddRange(new ValidRouteListBuilder().Build());
                dbContext.SaveChanges();

                IRepository<Route> routeRepository = new GenericRepository<Route>(logger, dbContext);

                IRoutePathService service = new RoutePathService(mapper, routeRepository);

                _result = service.GetRouteIdsAsync().GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Items_Is_Returned()
        {
            Assert.Equal(2, _result.Count);
        }

        [Fact]
        public void Then_The_Expected_Ids_Are_Returned()
        {
            _result[0].Should().Be(1);
            _result[1].Should().Be(2);
        }
    }
}
