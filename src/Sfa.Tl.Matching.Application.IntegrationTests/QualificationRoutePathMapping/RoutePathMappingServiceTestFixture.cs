using System;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.QualificationRoutePathMapping;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.QualificationRoutePathMapping
{
    public class RoutePathMappingServiceTestFixture : IDisposable
    {
        internal MatchingDbContext MatchingDbContext;
        internal RoutePathService RouteMappingService;

        public RoutePathMappingServiceTestFixture()
        {
            var loggerRepository = new Logger<RoutePathMappingRepository>(new NullLoggerFactory());
            var loggerRouteRepository = new Logger<RouteRepository>(new NullLoggerFactory());
            var loggerPathRepository = new Logger<PathRepository>(new NullLoggerFactory());
            var loggerExcelFileReader = new Logger<ExcelFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto>>(new NullLoggerFactory());
            var loggerRoutePathService = new Logger<RoutePathService>(new NullLoggerFactory());

            MatchingDbContext = new TestConfiguration().GetDbContext();

            var repository = new RoutePathMappingRepository(loggerRepository, MatchingDbContext);
            var routeRepository = new RouteRepository(loggerRouteRepository, MatchingDbContext);
            var pathRepository = new PathRepository(loggerPathRepository, MatchingDbContext);
            var dataValidator = new QualificationRoutePathMappingDataValidator(repository, pathRepository);
            var dataParser = new QualificationRoutePathMappingDataParser();

            var excelFileReader = new ExcelFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto>(loggerExcelFileReader, dataParser, dataValidator);

            var config = new MapperConfiguration(c => c.AddProfile<RoutePathMappingMapper>());
            var mapper = new Mapper(config);

            RouteMappingService = new RoutePathService(
                loggerRoutePathService,
                mapper,
                excelFileReader,
                routeRepository,
                pathRepository,
                repository);
        }

        internal void ResetData(string larsId)
        {
            var routePathMappings = MatchingDbContext.RoutePathMapping.Where(rpm => rpm.LarsId == larsId);
            if (routePathMappings.Any())
            {
                MatchingDbContext.RoutePathMapping.RemoveRange(routePathMappings);
                MatchingDbContext.SaveChanges();
            }
        }

        internal void CreateRoutePathMapping(string larsId)
        {
            var routePathMapping = new RoutePathMapping
            {
                LarsId = larsId, //Must match id in RoutePathMapping-Simple.xlsx
                Title = "Test",
                PathId = 1,
                Source = "Test",
                CreatedBy = nameof(RoutePathMappingServiceTestFixture)
            };

            MatchingDbContext.Add(routePathMapping);
            MatchingDbContext.SaveChanges();
        }

        public void Dispose()
        {
            MatchingDbContext?.Dispose();
        }
    }
}