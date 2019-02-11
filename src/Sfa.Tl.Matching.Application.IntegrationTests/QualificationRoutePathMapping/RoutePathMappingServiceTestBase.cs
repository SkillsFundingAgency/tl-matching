using System;
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
    public class QualificationRoutePathMappingTestFixture : IDisposable
    {
        protected MatchingDbContext MatchingDbContext;
        protected RoutePathService RouteMappingService;

        public RoutePathMappingServiceTestFixture()
        {
            var loggerRepository = new Logger<RoutePathMappingRepository>(new NullLoggerFactory());
            var loggerExcelFileReader = new Logger<ExcelFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto>>(new NullLoggerFactory());
            var loggerRoutePathService = new Logger<RoutePathService>(new NullLoggerFactory());

            MatchingDbContext = TestConfiguration.GetDbContext();

            //await ResetData();

            var repository = new RoutePathMappingRepository(loggerRepository, MatchingDbContext);
            var routePathRepository = new RoutePathRepository(MatchingDbContext);
            var dataValidator = new QualificationRoutePathMappingDataValidator(repository);
            var dataParser = new QualificationRoutePathMappingDataParser();

            var excelFileReader = new ExcelFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto>(loggerExcelFileReader, dataParser, dataValidator);

            var config = new MapperConfiguration(c => c.AddProfile<RoutePathMappingMapper>());
            var mapper = new Mapper(config);

            RouteMappingService = new RoutePathService(
                loggerRoutePathService,
                mapper,
                excelFileReader,
                routePathRepository,
                repository);
        }
        
        //public async Task ResetData()
        //{
        //    await MatchingDbContext.Database.ExecuteSqlCommandAsync("DELETE FROM dbo.RoutePathMapping");
        //    await MatchingDbContext.SaveChangesAsync();
        //}
        public void Dispose()
        {
            MatchingDbContext?.Dispose();
        }
    }
}
