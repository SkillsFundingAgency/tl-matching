using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class RepositoryCalledForGetPaths
    {
        private IRoutePathRepository _repository;
        private IRoutePathService _service;
        private readonly IEnumerable<Path> _pathData
            = new List<Path>
            {
                new Path { Id = 1, Name = "Path 1" },
                new Path { Id = 2, Name = "Path 2" }
            };

        [SetUp]
        public void Setup()
        {
            _repository = 
                Substitute
                    .For<IRoutePathRepository>();

            _repository.GetPathsAsync()
                .Returns(_pathData);

            _service = new RoutePathService(_repository);

            _service.GetPathsAsync().ConfigureAwait(false);
        }

        [Test]
        public async Task GetPathsAsyncIsCalledExactlyOnce()
        {
            await _repository
                .Received(1)
                .GetPathsAsync();
        }
    }
}
