using System.IO;
using System.Threading.Tasks;
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
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Provider
{
    public class When_Provider_Imports_File
    {
        private const string DataFilePath = @"Provider\Provider-Simple.xlsx";
        private MatchingDbContext _matchingDbContext;
        private int _createdRecordCount;

        [SetUp]
        public async Task Setup()
        {
            var loggerRepository = new Logger<ProviderRepository>(
                new NullLoggerFactory());

            var loggerImportService = new Logger<DataImportService<ProviderDto>>(
                new NullLoggerFactory());

            var loggerExcelFileReader = new Logger<ExcelFileReader<ProviderDto>>(
                new NullLoggerFactory());

            _matchingDbContext = TestConfiguration.GetDbContext();

            await ResetData();

            var repository = new ProviderRepository(loggerRepository, _matchingDbContext);
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
        public void Then_Record_Is_Saved()
        {
            Assert.AreEqual(1, _createdRecordCount);
        }

        private async Task ResetData()
        {
            await _matchingDbContext.Database.ExecuteSqlCommandAsync("DELETE FROM dbo.Provider");
            await _matchingDbContext.SaveChangesAsync();
        }
    }
}