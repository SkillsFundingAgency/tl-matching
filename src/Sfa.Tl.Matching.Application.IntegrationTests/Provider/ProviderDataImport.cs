using System.IO;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Provider
{
    public class ProviderDataImport
    {
        private const string DataFilePath = @"Provider\Provider-Simple.xlsx";
        private int _createdRecordCount;

        [SetUp]
        public void Setup()
        {
            var loggerRepository = new Logger<ProviderRepository>(
                new NullLoggerFactory());

            var loggerImportService = new Logger<DataImportService<ProviderDto>>(
                new NullLoggerFactory());

            var loggerExcelFileReader = new Logger<ExcelFileReader<ProviderDto>>(
                new NullLoggerFactory());

            var matchingDbContext = TestConfiguration.GetDbContext();

            var repository = new ProviderRepository(loggerRepository, matchingDbContext);
            var dataValidator = new ProviderDataValidator(repository);
            var dataParser = new ProviderDataParser();

            var excelFileReader = new ExcelFileReader<ProviderDto>(loggerExcelFileReader, dataParser, dataValidator);

            var config = new MapperConfiguration(c => c.AddProfile<ProviderMapper>());

            var mapper = new Mapper(config);

            var providerService = new ProviderService(
                mapper,
                new DataImportService<ProviderDto>(
                    loggerImportService,
                    excelFileReader),
                repository);

            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, DataFilePath);
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _createdRecordCount = providerService.ImportProvider(stream).Result;
            }
        }

        [Test]
        public void RecordsSaved()
        {
            Assert.AreEqual(1, _createdRecordCount);
        }
    }
}