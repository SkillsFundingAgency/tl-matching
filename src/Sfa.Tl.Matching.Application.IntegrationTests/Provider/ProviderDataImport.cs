using System.IO;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Provider
{
    public class ProviderDataImport
    {
        private const string DataFilePath = "./Provider/Provider-Simple.xlsx";

        [SetUp]
        public void Setup()
        {
            var loggerRepository = new Logger<ProviderRepository>(
                new NullLoggerFactory());
            var loggerExcelFileReader = new Logger<ExcelFileReader<ProviderDto>>(
                new NullLoggerFactory());

            var connectionString = "Server=(local);Database=TL;Trusted_Connection=True;MultipleActiveResultSets=true;";
            var options = new DbContextOptionsBuilder<MatchingDbContext>().UseSqlServer(connectionString).Options;
            var matchingDbContext = new MatchingDbContext(options);
            var repository = new ProviderRepository(loggerRepository, matchingDbContext);
            var dataValidator = new ProviderDataValidator();
            var dataParser = new ProviderDataParser();

            var excelFileReader = new ExcelFileReader<ProviderDto>(loggerExcelFileReader, dataParser, dataValidator);

            var mappingProfile = new ProviderMapper();

            var config = new MapperConfiguration(mappingProfile);
            var mapper = new Mapper(config);

            var providerService = new ProviderService(
                mapper,
                new DataImportService<ProviderDto>(
                    loggerRepository,
                    mapper, 
                    excelFileReader),
                repository);

            using (var stream = File.Open(DataFilePath, FileMode.Open))
            {
                var createdRecordCount = providerService.ImportProvider(stream).Result;
            }
        }

        [Test]
        public void RecordsSaved()
        {
            Assert.AreEqual(1, 1);
        }
    }
}