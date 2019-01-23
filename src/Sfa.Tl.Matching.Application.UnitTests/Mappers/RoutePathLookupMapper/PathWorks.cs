using AutoMapper;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Core.DomainModels;
using Sfa.Tl.Matching.Data.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Mappers.RoutePathLookupMapper
{
    public class PathWorks
    {
        private IMapper _mapper;
        private Path _path;
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

            _path = _mapper.Map<Path>(_entity);
        }

        [Test]
        public void PathNameIsMapped()
        {
            Assert.AreEqual(_entity.Path, _path.Name);
        }
        
        [Test]
        public void PathIdIsMapped()
        {
            Assert.AreEqual(_entity.Id, _path.RoutePathId);
        }
    }
}
