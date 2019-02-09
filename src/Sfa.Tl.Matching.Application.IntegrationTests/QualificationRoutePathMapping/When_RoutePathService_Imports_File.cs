using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.RoutePathMapping;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.QualificationRoutePathMapping
{
    public class When_RoutePathService_Imports_File
    {
        private const string DataFilePath = @"QualificationRoutePathMapping\RoutePathMapping-Simple.xlsx";
        private MatchingDbContext _matchingDbContext;
        private int _createdRecordCount;

        [SetUp]
        public async Task Setup()
        {
            var loggerRepository = new Logger<RoutePathMappingRepository>(new NullLoggerFactory());
            var loggerExcelFileReader = new Logger<ExcelFileReader<RoutePathMappingDto>>(new NullLoggerFactory());
            var loggerRoutePathService = new Logger<RoutePathService>(new NullLoggerFactory());
            var loggerDataImportService = new Logger<DataImportService<RoutePathMappingDto>>(new NullLoggerFactory());

            _matchingDbContext = TestConfiguration.GetDbContext();

            await ResetData();

            var repository = new RoutePathMappingRepository(loggerRepository, _matchingDbContext);
            var routePathRepository = new RoutePathRepository(_matchingDbContext);
            var dataValidator = new RoutePathMappingDataValidator();
            var dataParser = new RoutePathMappingDataParser();

            var excelFileReader = new ExcelFileReader<RoutePathMappingDto>(loggerExcelFileReader, dataParser, dataValidator);

            var mappingProfile = new RoutePathMappingMapper();

            var config = new MapperConfiguration(mappingProfile);
            var mapper = new Mapper(config);

            var dataImportService = new DataImportService<RoutePathMappingDto>(
                loggerDataImportService,
                excelFileReader);

            var routeMappingService = new RoutePathService(
                loggerRoutePathService,
                mapper,
                dataImportService,
                routePathRepository,
                repository);

            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = await routeMappingService.ImportQualificationPathMapping(stream);
            }
        }

        [Test]
        public void Then_Record_Is_Saved()
        {
            Assert.AreEqual(3, _createdRecordCount);
        }

        public async Task ResetData()
        {
            await _matchingDbContext.Database.ExecuteSqlCommandAsync("DELETE FROM dbo.RoutePathMapping");
            await _matchingDbContext.SaveChangesAsync();
        }
    }
}
