using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.LocalEnterprisePartnershipStaging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Extensions;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;

namespace Sfa.Tl.Matching.Application.IntegrationTests.LocalEnterprisePartnership
{
    public class LocalEnterprisePartnershipTestFixture : IDisposable
    {
        public IFileImportService<LocalEnterprisePartnershipStagingFileImportDto> FileImportService;
        public MatchingDbContext MatchingDbContext;

        public LocalEnterprisePartnershipTestFixture()
        {
            var loggerRepository = new Logger<SqlBulkInsertRepository<LocalEnterprisePartnershipStaging>>(new NullLoggerFactory());
            var loggerCsvFileReader = new Logger<CsvFileReader<LocalEnterprisePartnershipStagingFileImportDto, LocalEnterprisePartnershipStagingDto>>(new NullLoggerFactory());

            var logger = new Logger<FileImportService<LocalEnterprisePartnershipStagingFileImportDto, LocalEnterprisePartnershipStagingDto, LocalEnterprisePartnershipStaging>>(new NullLoggerFactory());

            var testConfig = new TestConfiguration();
            MatchingDbContext = testConfig.GetDbContext();
            var matchingConfiguration = TestConfiguration.MatchingConfiguration;

            var policyRegistry = new Polly.Registry.PolicyRegistry();
            policyRegistry.AddSqlRetryPolicy();

            var repository = new SqlBulkInsertRepository<LocalEnterprisePartnershipStaging>(loggerRepository, matchingConfiguration, policyRegistry);
            var functionLogRepository = new GenericRepository<FunctionLog>(new NullLogger<GenericRepository<FunctionLog>>(), MatchingDbContext);
            
            var dataValidator = new LocalEnterprisePartnershipStagingDataValidator();
            var dataParser = new LocalEnterprisePartnershipStagingDataParser();
            var nullDataProcessor = new NullDataProcessor<LocalEnterprisePartnershipStaging>();
            var csvFileReader = new CsvFileReader<LocalEnterprisePartnershipStagingFileImportDto, LocalEnterprisePartnershipStagingDto>(loggerCsvFileReader, dataParser, dataValidator, functionLogRepository);

            var config = new MapperConfiguration(c => c.AddMaps(typeof(LocalEnterprisePartnershipStagingMapper).Assembly));

            var mapper = new Mapper(config);

            FileImportService = new FileImportService<LocalEnterprisePartnershipStagingFileImportDto, LocalEnterprisePartnershipStagingDto, LocalEnterprisePartnershipStaging>(logger, mapper, csvFileReader, repository, nullDataProcessor);
        }

        public void ResetData(string name = null)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                MatchingDbContext.Database.ExecuteSqlRaw("DELETE FROM LocalEnterprisePartnership");
                MatchingDbContext.SaveChanges();
                return;
            }

            var localEnterprisePartnership = MatchingDbContext.LocalEnterprisePartnership.FirstOrDefault(e => e.Name == name);

            if (localEnterprisePartnership == null) return;
            
            MatchingDbContext.LocalEnterprisePartnership.Remove(localEnterprisePartnership);
            var count = MatchingDbContext.SaveChanges();
            count.Should().Be(1);
        }

        public void AddExisting(string code, string name)
        {
            var localEnterprisePartnership = MatchingDbContext.LocalEnterprisePartnership.FirstOrDefault(e => e.Name == name);

            if (localEnterprisePartnership != null) return;
            
            MatchingDbContext.LocalEnterprisePartnership.Add(new Domain.Models.LocalEnterprisePartnership
            {
                Code = code,
                Name = name
            });

            var count = MatchingDbContext.SaveChanges();
            count.Should().Be(1);
        }

        public void Dispose()
        {
            MatchingDbContext?.Dispose();
        }
    }
}