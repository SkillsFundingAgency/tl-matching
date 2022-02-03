using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.LearningAimReferenceStaging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Extensions;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;

namespace Sfa.Tl.Matching.Application.IntegrationTests.LearningAimReference
{
    public class LearningAimReferenceTestFixture : IDisposable
    {
        public IFileImportService<LearningAimReferenceStagingFileImportDto> FileImportService;
        public MatchingDbContext MatchingDbContext;

        public LearningAimReferenceTestFixture()
        {
            var loggerRepository = new Logger<SqlBulkInsertRepository<LearningAimReferenceStaging>>(new NullLoggerFactory());
            var loggerCsvFileReader = new Logger<CsvFileReader<LearningAimReferenceStagingFileImportDto, LearningAimReferenceStagingDto>>(new NullLoggerFactory());

            var logger = new Logger<FileImportService<LearningAimReferenceStagingFileImportDto, LearningAimReferenceStagingDto, LearningAimReferenceStaging>>(new NullLoggerFactory());

            var testConfig = new TestConfiguration();
            MatchingDbContext = testConfig.GetDbContext();
            var matchingConfiguration = TestConfiguration.MatchingConfiguration;

            var policyRegistry = new Polly.Registry.PolicyRegistry();
            policyRegistry.AddSqlRetryPolicy();

            var repository = new SqlBulkInsertRepository<LearningAimReferenceStaging>(loggerRepository, matchingConfiguration, policyRegistry);
            var functionLogRepository = new GenericRepository<FunctionLog>(new NullLogger<GenericRepository<FunctionLog>>(), MatchingDbContext);
            
            var dataValidator = new LearningAimReferenceStagingDataValidator();
            var dataParser = new LearningAimReferenceStagingDataParser();
            var nullDataProcessor = new NullDataProcessor<LearningAimReferenceStaging>();
            var csvFileReader = new CsvFileReader<LearningAimReferenceStagingFileImportDto, LearningAimReferenceStagingDto>(loggerCsvFileReader, dataParser, dataValidator, functionLogRepository);

            var config = new MapperConfiguration(c => c.AddMaps(typeof(LearningAimReferenceStagingMapper).Assembly));

            var mapper = new Mapper(config);

            FileImportService = new FileImportService<LearningAimReferenceStagingFileImportDto, LearningAimReferenceStagingDto, LearningAimReferenceStaging>(logger, mapper, csvFileReader, repository, nullDataProcessor);
        }

        public void ResetData(string title = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                MatchingDbContext.Database.ExecuteSqlRaw("DELETE FROM LearningAimReference");
                MatchingDbContext.SaveChanges();
                return;
            }

            var learningAimReference = MatchingDbContext.LearningAimReference.FirstOrDefault(e => e.Title == title);

            if (learningAimReference == null) return;
            
            MatchingDbContext.LearningAimReference.Remove(learningAimReference);
            var count = MatchingDbContext.SaveChanges();
            count.Should().Be(1);
        }

        public void AddExisting(string larId, string title)
        {
            var learningAimReference = MatchingDbContext.LearningAimReference.FirstOrDefault(e => e.Title == title);

            if (learningAimReference != null) return;
            
            MatchingDbContext.LearningAimReference.Add(new Domain.Models.LearningAimReference
            {
                Title = title,
                AwardOrgLarId = "",
                LarId = larId,
                SourceCreatedOn = new DateTime(2019, 01, 01),
                SourceModifiedOn = new DateTime(2019, 01, 01)
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