using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.QualificationRoutePathMapping
{
    public abstract class RoutePathMappingServiceTestBase
    {
        protected MatchingDbContext MatchingDbContext;
        protected RoutePathService RouteMappingService;

        public virtual async Task Setup()
        {
            var loggerRepository = new Logger<RoutePathMappingRepository>(new NullLoggerFactory());
            var loggerExcelFileReader = new Logger<ExcelFileReader<RoutePathMappingDto>>(new NullLoggerFactory());
            var loggerRoutePathService = new Logger<RoutePathService>(new NullLoggerFactory());
            var loggerDataImportService = new Logger<DataImportService<RoutePathMappingDto>>(new NullLoggerFactory());

            MatchingDbContext = TestConfiguration.GetDbContext();

            await ResetData();

            var repository = new RoutePathMappingRepository(loggerRepository, MatchingDbContext);
            var routePathRepository = new RoutePathRepository(MatchingDbContext);
            var dataValidator = new RoutePathMappingDataValidator(repository);
            var dataParser = new RoutePathMappingDataParser();

            var excelFileReader = new ExcelFileReader<RoutePathMappingDto>(loggerExcelFileReader, dataParser, dataValidator);

            var config = new MapperConfiguration(c => c.AddProfile<RoutePathMappingMapper>());
            var mapper = new Mapper(config);

            var dataImportService = new DataImportService<RoutePathMappingDto>(
                loggerDataImportService,
                excelFileReader);

            RouteMappingService = new RoutePathService(
                loggerRoutePathService,
                mapper,
                dataImportService,
                routePathRepository,
                repository);
        }
        
        public async Task ResetData()
        {
            await MatchingDbContext.Database.ExecuteSqlCommandAsync("DELETE FROM dbo.RoutePathMapping");
            await MatchingDbContext.SaveChangesAsync();
        }
    }
}
