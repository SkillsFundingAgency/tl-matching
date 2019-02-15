using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.QualificationRoutePathMapping.Builders
{
    internal class PathListBuilder
    {
        private readonly IQueryable<Path> _provider;

        public PathListBuilder()
        {
            _provider = new List<Path>
            {
                new Path
                {
                    Id = 1,
                    RouteId = 1,
                    Name = "Agriculture, land management and production"
                },
                new Path
                {
                    Id = 2,
                    RouteId = 1,
                    Name = "Animal care and management"
                },
                new Path
                {
                    Id = 5,
                    RouteId = 3,
                    Name = "Hospitality"
                },
                new Path
                {
                    Id = 25,
                    RouteId = 1,
                    Name = "Accountancy"
                }
            }.AsQueryable();
        }

        public IQueryable<Path> Build() => _provider;
    }
}