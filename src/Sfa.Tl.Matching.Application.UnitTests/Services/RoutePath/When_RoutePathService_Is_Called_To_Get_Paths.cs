using System;
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
    public class When_RoutePathService_Is_Called_To_Get_Paths
    {
        private readonly IRepository<Path> _pathRepository;

        private readonly IQueryable<Path> _result;
        
        private readonly IQueryable<Path> _pathData
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
                    Id = 2,
                    RouteId = 1,
                    Name = "Path 2",
                    Keywords = "Keyword3, Keyword4",
                    Summary = "Path 2 summary"
                }
            }
            .AsQueryable();

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

        public When_RoutePathService_Is_Called_To_Get_Paths()
        {
            var logger = Substitute.For<ILogger<RoutePathService>>();
            var mapper = Substitute.For<IMapper>();
            var fileReader = Substitute.For<IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto>>();
            var routeRepository = Substitute.For<IRepository<Route>>();
            _pathRepository = Substitute.For<IRepository<Path>>();
            var routePathMappingRepository = Substitute.For<IRepository<RoutePathMapping>>();

            _pathRepository
                .GetMany(Arg.Any<Func<Path, bool>>())
                .Returns(_pathData);

            var service = new RoutePathService(logger, mapper, fileReader, routeRepository, _pathRepository, routePathMappingRepository);

            _result = service.GetPaths();
        }

        [Fact]
        public void Then_Path_Repository_GetMany_Is_Called_Exactly_Once()
        {
            _pathRepository
                .Received(1)
                .GetMany(Arg.Any<Func<Path, bool>>());
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Items_Is_Returned()
        {
            Assert.Equal(_pathData.Count(), _result.Count());
        }

        [Fact]
        public void Then_Path_Id_Is_Returned()
        {
            Assert.Equal(_expected.First().Id, _result.First().Id);
        }

        [Fact]
        public void Then_Path_Name_Is_Returned()
        {
            Assert.Equal(_expected.First().Name, _result.First().Name);
        }

        [Fact]
        public void Then_Path_Keywords_Is_Returned()
        {
            Assert.Equal(_expected.First().Keywords, _result.First().Keywords);
        }

        [Fact]
        public void Then_Path_Summary_Is_Returned()
        {
            Assert.Equal(_expected.First().Summary, _result.First().Summary);
        }

        [Fact]
        public void Then_Path_RouteId_Is_Returned()
        {
            Assert.Equal(_expected.First().RouteId, _result.First().RouteId);
        }
    }
}
