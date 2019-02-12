using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.ProviderVenue;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.ProviderVenue
{
    public class ProviderVenueTestBase
    {
        internal readonly IProviderVenueService ProviderVenueService;
        internal MatchingDbContext MatchingDbContext;

        public ProviderVenueTestBase()
        {
            var loggerRepository = new Logger<ProviderRepository>(
                new NullLoggerFactory());
            var providerVenueloggerRepository = new Logger<ProviderVenueRepository>(
                new NullLoggerFactory());

            var loggerExcelFileReader = new Logger<ExcelFileReader<ProviderVenueFileImportDto, ProviderVenueDto>>(
                new NullLoggerFactory());

            MatchingDbContext = TestConfiguration.GetDbContext();

            var repository = new ProviderRepository(loggerRepository, MatchingDbContext);
            var providerVenueRepository = new ProviderVenueRepository(providerVenueloggerRepository, MatchingDbContext);
            var dataValidator = new ProviderVenueDataValidator(repository, providerVenueRepository);
            var dataParser = new ProviderVenueDataParser();

            var excelFileReader = new ExcelFileReader<ProviderVenueFileImportDto, ProviderVenueDto>(loggerExcelFileReader, dataParser, dataValidator);

            var config = new MapperConfiguration(c => c.AddProfile<ProviderVenueMapper>());

            var mapper = new Mapper(config);

            ProviderVenueService = new ProviderVenueService(mapper, excelFileReader, providerVenueRepository);
        }

        internal async Task ResetData()
        {
            await MatchingDbContext.Database.ExecuteSqlCommandAsync("DELETE FROM dbo.ProviderVenue");
            await MatchingDbContext.SaveChangesAsync();
        }
    }
}