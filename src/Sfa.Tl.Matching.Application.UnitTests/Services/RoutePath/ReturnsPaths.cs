﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class ReturnsPaths
    {
        private IRoutePathRepository _repository;
        private IRoutePathService _service;
        private Task<IEnumerable<Path>> _result;
        private readonly IEnumerable<Path> _pathData
            = new List<Path>
            {
                new Path { Id = 1, Name = "Path 1" },
                new Path { Id = 2, Name = "Path 2" }
            };
        private readonly IEnumerable<Path> _expected
            = new List<Path>
            {
                new Path { Name = "Path 1"},
                new Path { Name = "Path 2"}
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

            _result = _service.GetPathsAsync();
        }

        [Test]
        public void ResultIsAsExpected()
        {
            Assert.AreEqual(_expected.First().Name, _result.Result.First().Name);
        }
    }
}
