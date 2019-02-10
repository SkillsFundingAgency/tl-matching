using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class When_RoutePathRepository_Is_Called_To_Get_Routes
    {
        private ILogger<RoutePathService> _logger;
        private IMapper _mapper;
        private IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto> _fileReader;
        private IRoutePathRepository _repository;
        private IRepository<RoutePathMapping> _routePathMappingRepository;
        private IRoutePathService _service;
        private IQueryable<Route> _result;

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

        private readonly IEnumerable<Route> _expected
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
                    { Id = 2,
                        Name = "Route 2",
                        Keywords = "Keyword2",
                        Summary = "Route 2 summary"
                    }
            };

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            _logger = Substitute.For<ILogger<RoutePathService>>();
            _mapper = Substitute.For<IMapper>();
            _fileReader = Substitute.For<IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto>>();
            _repository = Substitute.For<IRoutePathRepository>();
            _routePathMappingRepository = Substitute.For<IRepository<RoutePathMapping>>();

            _repository
                .GetRoutes()
                .Returns(_routeData);

            _service = new RoutePathService(_logger, _mapper, _fileReader, _repository, _routePathMappingRepository);

            _result = _service.GetRoutes();
        }

        [Test]
        public void Then_GetRoutes_Is_Called_Exactly_Once()
        {
            _repository
                .Received(1)
                .GetRoutes();
        }

        [Test]
        public void Then_The_Expected_Number_Of_Items_Is_Returned()
        {
            Assert.AreEqual(_routeData.Count(), _result.Count());
        }

        [Test]
        public void Then_Route_Id_Is_Returned()
        {
            Assert.AreEqual(_expected.First().Id, _result.First().Id);
        }

        [Test]
        public void Then_Route_Name_Is_Returned()
        {
            Assert.AreEqual(_expected.First().Name, _result.First().Name);
        }

        [Test]
        public void Then_Route_Keywords_Is_Returned()
        {
            Assert.AreEqual(_expected.First().Keywords, _result.First().Keywords);
        }

        [Test]
        public void Then_Route_Summary_Is_Returned()
        {
            Assert.AreEqual(_expected.First().Summary, _result.First().Summary);
        }
    }
}
