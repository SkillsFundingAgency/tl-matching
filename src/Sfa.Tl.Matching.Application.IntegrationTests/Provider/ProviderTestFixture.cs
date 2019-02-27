using System;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Data.SearchProviders;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Provider
{
    public class ProviderTestFixture : IDisposable
    {
        public readonly IFileImportService<ProviderFileImportDto, ProviderDto, Domain.Models.Provider> FileImportService;
        public MatchingDbContext MatchingDbContext;

        public ProviderTestFixture()
        {
            var loggerRepository = new Logger<ProviderRepository>(new NullLoggerFactory());
            var loggerExcelFileReader = new Logger<ExcelFileReader<ProviderFileImportDto, ProviderDto>>(new NullLoggerFactory());
            var logger = new Logger<FileImportService<ProviderFileImportDto, ProviderDto, Domain.Models.Provider>>(new NullLoggerFactory());

            MatchingDbContext = new TestConfiguration().GetDbContext();

            var repository = new ProviderRepository(loggerRepository, MatchingDbContext);
            var dataValidator = new ProviderDataValidator(repository);
            var dataParser = new ProviderDataParser();

            var excelFileReader = new ExcelFileReader<ProviderFileImportDto, ProviderDto>(loggerExcelFileReader, dataParser, dataValidator);

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);

            FileImportService = new FileImportService<ProviderFileImportDto, ProviderDto, Domain.Models.Provider>(logger, mapper, excelFileReader, repository);
        }

        public void ResetData()
        {
            var provider = MatchingDbContext.Provider.FirstOrDefault(p => p.CreatedBy == nameof(ProviderTestFixture));
            if (provider != null) MatchingDbContext.Provider.Remove(provider);

            MatchingDbContext.SaveChanges();
        }

        public int GetCountBy(string name)
        {
            var providerCount = MatchingDbContext.Provider.Count(p => p.Name == name);

            return providerCount;
        }

        public void Dispose()
        {
            ResetData();
            MatchingDbContext?.Dispose();
        }
    }
}