using System;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Tests.Common;
using Sfa.Tl.Matching.Tests.Common.Builders;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class RoutePathTestFixture : IDisposable
    {
        private readonly RouteBuilder _builder;
        private readonly MatchingDbContext _matchingDbContext;

        public IRoutePathService RoutePathService { get; }

        public RoutePathTestFixture()
        {
            var logger = Substitute.For<ILogger<GenericRepository<Route>>>();

            var config = new MapperConfiguration(c =>
            {
                c.AddMaps(typeof(RouteMapper).Assembly);
            });

            var mapper = new Mapper(config);
            _matchingDbContext = InMemoryDbContext.Create();

            _builder = new RouteBuilder(_matchingDbContext)
                .CreateRoutes(2)
                .SaveData();

            var routeRepository = new GenericRepository<Route>(logger, _matchingDbContext);

            RoutePathService = new RoutePathService(mapper, routeRepository);
        }

        public void Dispose()
        {
            _builder?.ClearData();
            _matchingDbContext?.Dispose();
        }
    }
}
