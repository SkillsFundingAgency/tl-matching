using System.Collections.Generic;
using System.Linq;
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
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Tests.Common;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class When_RoutePathService_Is_Called_To_Get_Route_Summary
    {
        private readonly IList<RouteSummaryViewModel> _result;

        public When_RoutePathService_Is_Called_To_Get_Route_Summary()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Route>>>();

            var config = new MapperConfiguration(c => { c.AddMaps(typeof(RouteMapper).Assembly); });
            var mapper = new Mapper(config);

            using var dbContext = InMemoryDbContext.Create();
            dbContext.AddRange(new ValidRouteListBuilder().Build());
            dbContext.SaveChanges();

            IRepository<Route> routeRepository = new GenericRepository<Route>(logger, dbContext);

            IRoutePathService service = new RoutePathService(mapper, routeRepository);

            _result = service.GetRouteSummaryAsync().GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Items_Is_Returned()
        {
            Assert.Equal(2, _result.Count);
        }

        [Fact]
        public void Then_Route_Summary_Data_Is_As_Expected()
        {
            var firstResult = _result.First();
            firstResult.Id.Should().Be(1);
            firstResult.IsSelected.Should().BeFalse();
            firstResult.Name.Should().Be("Route 1");
            firstResult.Summary.Should().Be("Route 1 summary");

            var secondResult = _result.Skip(1).First();
            secondResult.Id.Should().Be(2);
            secondResult.IsSelected.Should().BeFalse();
            secondResult.Name.Should().Be("Route 2");
            secondResult.Summary.Should().Be("Route 2 summary");
        }
    }
}
