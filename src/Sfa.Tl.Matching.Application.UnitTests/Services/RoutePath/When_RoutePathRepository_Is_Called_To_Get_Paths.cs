using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class When_RoutePathRepository_Is_Called_To_Get_Paths
    {
        private IRoutePathRepository _repository;
        private IRoutePathService _service;
        private Task<IEnumerable<Path>> _result;

        private readonly IEnumerable<Path> _pathData
            = new List<Path>
            {
                new Path
                {
                    Id = 1,
                    RouteId = 1,
                    Name = "Path 1",
                    Keywords = "Keyword1, Keyword2",
                    Summary = "Path 1 summary",
                },
                new Path
                {
                    Id = 2,
                    RouteId = 1,
                    Name = "Path 2",
                    Keywords = "Keyword3, Keyword4",
                    Summary = "Path 2 summary",
                }
            };

        private readonly IEnumerable<Path> _expected
            = new List<Path>
            {
                new Path
                {
                    Id = 1,
                    RouteId = 1,
                    Name = "Path 1",
                    Keywords = "Keyword1, Keyword2",
                    Summary = "Path 1 summary"
                },
                new Path
                {
                    Id = 1,
                    RouteId = 1,
                    Name = "Path 2",
                    Keywords = "Keyword3, Keyword4",
                    Summary = "Path 2 summary"
                }
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

            _result = Task.FromResult(_service.GetPathsAsync().Result);
        }

        [Test]
        public async Task Then_GetPathsAsync_Is_Called_Exactly_Once()
        {
            await _repository
                .Received(1)
                .GetPathsAsync();
        }

        [Test]
        public void Then_The_Expected_Number_Of_Items_Is_Returned()
        {
            Assert.AreEqual(_pathData.Count(), _result.Result.Count());
        }

        [Test]
        public void Then_Path_Id_Is_Returned()
        {
            Assert.AreEqual(_expected.First().Id, _result.Result.First().Id);
        }

        [Test]
        public void Then_Path_Name_Is_Returned()
        {
            Assert.AreEqual(_expected.First().Name, _result.Result.First().Name);
        }

        [Test]
        public void Then_Path_Keywords_Is_Returned()
        {
            Assert.AreEqual(_expected.First().Keywords, _result.Result.First().Keywords);
        }

        [Test]
        public void Then_Path_Summary_Is_Returned()
        {
            Assert.AreEqual(_expected.First().Summary, _result.Result.First().Summary);
        }

        [Test]
        public void Then_Path_RouteId_Is_Returned()
        {
            Assert.AreEqual(_expected.First().RouteId, _result.Result.First().RouteId);
        }
    }
}
