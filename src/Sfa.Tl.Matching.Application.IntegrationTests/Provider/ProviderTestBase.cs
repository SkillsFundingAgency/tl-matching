using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Provider;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Provider
{
    public class ProviderTestBase
    {
        internal readonly IProviderService _providerService;
        internal MatchingDbContext _matchingDbContext;

        public ProviderTestBase()
        {
            var loggerRepository = new Logger<ProviderRepository>(
                new NullLoggerFactory());

            var loggerImportService = new Logger<DataImportService<ProviderDto>>(
                new NullLoggerFactory());

            var loggerExcelFileReader = new Logger<ExcelFileReader<ProviderDto>>(
                new NullLoggerFactory());

            _matchingDbContext = TestConfiguration.GetDbContext();

            var repository = new ProviderRepository(loggerRepository, _matchingDbContext);
            var dataValidator = new ProviderDataValidator(repository);
            var dataParser = new ProviderDataParser();

            var excelFileReader = new ExcelFileReader<ProviderFileImportDto, ProviderDto>(loggerExcelFileReader, dataParser, dataValidator);

            var config = new MapperConfiguration(c => c.AddProfile<ProviderMapper>());

            var mapper = new Mapper(config);

            _providerService = new ProviderService(
                mapper,
                new DataImportService<ProviderDto>(
                    loggerImportService,
                    excelFileReader),
                repository);
        }

        internal async Task ResetData()
        {
            await _matchingDbContext.Database.ExecuteSqlCommandAsync("DELETE FROM dbo.Provider");
            await _matchingDbContext.SaveChangesAsync();
        }
    }
}