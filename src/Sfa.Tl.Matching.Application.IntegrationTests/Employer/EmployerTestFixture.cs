using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Employer
{
    public class EmployerTestFixture : IDisposable
    {
        public IFileImportService<EmployerStagingFileImportDto> FileImportService;
        public MatchingDbContext MatchingDbContext;

        public EmployerTestFixture()
        {
            var loggerRepository = new Logger<SqlBulkInsertRepository<EmployerStaging>>(new NullLoggerFactory());
            var loggerExcelFileReader = new Logger<ExcelFileReader<EmployerStagingFileImportDto, EmployerStagingDto>>(new NullLoggerFactory());

            var logger = new Logger<FileImportService<EmployerStagingFileImportDto, EmployerStagingDto, EmployerStaging>>(new NullLoggerFactory());

            var testConfig = new TestConfiguration();
            MatchingDbContext = testConfig.GetDbContext();
            var matchingConfiguration = TestConfiguration.MatchingConfiguration;

            var repository = new SqlBulkInsertRepository<EmployerStaging>(loggerRepository, matchingConfiguration);
            var functionLogRepository = new GenericRepository<FunctionLog>(new NullLogger<GenericRepository<FunctionLog>>(), MatchingDbContext);
            
            var dataValidator = new EmployerStagingDataValidator();
            var dataParser = new EmployerStagingDataParser();
            var nullDataProcessor = new NullDataProcessor<EmployerStaging>();
            var excelFileReader = new ExcelFileReader<EmployerStagingFileImportDto, EmployerStagingDto>(loggerExcelFileReader, dataParser, dataValidator, functionLogRepository);

            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerMapper).Assembly));

            var mapper = new Mapper(config);

            FileImportService = new FileImportService<EmployerStagingFileImportDto, EmployerStagingDto, EmployerStaging>(logger, mapper, excelFileReader, repository, nullDataProcessor);
        }

        public void ResetData(string companyName)
        {
            var employer = MatchingDbContext.Employer.FirstOrDefault(e => e.CompanyName == companyName);

            if (employer != null)
            {
                MatchingDbContext.Employer.Remove(employer);
                var count = MatchingDbContext.SaveChanges();
                count.Should().Be(1);
            }
        }

        public void Dispose()
        {
            MatchingDbContext?.Dispose();
        }
    }
}