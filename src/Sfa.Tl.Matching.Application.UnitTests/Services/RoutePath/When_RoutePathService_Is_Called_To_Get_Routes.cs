using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath
{
    public class When_RoutePathService_Is_Called_To_Get_Routes
    {
        private readonly IRoutePathRepository _repository;
        private readonly IQueryable<Route> _result;

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

        public readonly IEnumerable<Route> Expected
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

        public When_RoutePathService_Is_Called_To_Get_Routes()
        {
            var logger = Substitute.For<ILogger<RoutePathService>>();
            var mapper = Substitute.For<IMapper>();
            var fileReader = Substitute.For<IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto>>();
            _repository = Substitute.For<IRoutePathRepository>();
            var routePathMappingRepository = Substitute.For<IRepository<RoutePathMapping>>();

            _repository
                .GetRoutes()
                .Returns(_routeData);

            IRoutePathService service = new RoutePathService(logger, mapper, fileReader, _repository, routePathMappingRepository);

            _result = service.GetRoutes();
        }

        [Fact]
        public void Then_GetRoutes_Is_Called_Exactly_Once()
        {
            _repository
                .Received(1)
                .GetRoutes();
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Items_Is_Returned()
        {
            Assert.Equal(_routeData.Count(), _result.Count());
        }

        [Fact]
        public void Then_Route_Id_Is_Returned()
        {
            Assert.Equal(Expected.First().Id, _result.First().Id);
        }

        [Fact]
        public void Then_Route_Name_Is_Returned()
        {
            Assert.Equal(Expected.First().Name, _result.First().Name);
        }

        [Fact]
        public void Then_Route_Keywords_Is_Returned()
        {
            Assert.Equal(Expected.First().Keywords, _result.First().Keywords);
        }

        [Fact]
        public void Then_Route_Summary_Is_Returned()
        {
            Assert.Equal(Expected.First().Summary, _result.First().Summary);
        }
    }
}
