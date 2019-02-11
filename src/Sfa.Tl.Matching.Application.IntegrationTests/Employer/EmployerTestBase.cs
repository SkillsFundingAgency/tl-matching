using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Employer
{
    public class EmployerTestBase
    {
        internal readonly IEmployerService EmployerService;
        internal MatchingDbContext MatchingDbContext;

        public EmployerTestBase()
        {
            var loggerRepository = new Logger<EmployerRepository>(
                new NullLoggerFactory());

            var loggerExcelFileReader = new Logger<ExcelFileReader<EmployerFileImportDto, EmployerDto>>(
                new NullLoggerFactory());

            MatchingDbContext = TestConfiguration.GetDbContext();

            var repository = new EmployerRepository(loggerRepository, MatchingDbContext);
            var dataValidator = new EmployerDataValidator();
            var dataParser = new EmployerDataParser();

            var excelFileReader = new ExcelFileReader<EmployerFileImportDto, EmployerDto>(loggerExcelFileReader, dataParser, dataValidator);

            var config = new MapperConfiguration(c => c.AddProfile<EmployerMapper>());

            var mapper = new Mapper(config);

            EmployerService = new EmployerService(mapper, excelFileReader, repository);
        }

        internal async Task ResetData()
        {
            await MatchingDbContext.Database.ExecuteSqlCommandAsync("DELETE FROM dbo.Employer");
            await MatchingDbContext.SaveChangesAsync();
        }
    }
}