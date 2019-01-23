using AutoMapper;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Core.DomainModels;
using Sfa.Tl.Matching.Data.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Mappers.RoutePathLookupMapper
{
    public class RouteWorks
    {
        private IMapper _mapper;
        private Route _route;
        private RoutePathLookup _entity =
            new RoutePathLookup
            {
                Id = 1,
                Route = "Route 1",
                Path = "Path 1"
            };

        [SetUp]
        public void Setup()
        {
            var config = new MapperConfiguration(
                cfg =>
                    cfg.AddProfile<RoutePathLookupMappingProfile>());
            _mapper = config.CreateMapper();

            _route = _mapper.Map<Route>(_entity);
        }

        [Test]
        public void RouteNameIsMapped()
        {
            Assert.AreEqual(_entity.Route, _route.Name);
        }
    }
}
