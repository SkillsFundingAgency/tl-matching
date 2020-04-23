using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.PostcodeLookupStaging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;

namespace Sfa.Tl.Matching.Application.IntegrationTests.PostcodeLookup
{
    public class PostcodeLookupTestFixture : IDisposable
    {
        public IFileImportService<PostcodeLookupStagingFileImportDto> FileImportService;
        public MatchingDbContext MatchingDbContext;

        public PostcodeLookupTestFixture()
        {
            var loggerRepository = new Logger<SqlBulkInsertRepository<PostcodeLookupStaging>>(new NullLoggerFactory());
            var loggerCsvFileReader = new Logger<CsvFileReader<PostcodeLookupStagingFileImportDto, PostcodeLookupStagingDto>>(new NullLoggerFactory());

            var logger = new Logger<FileImportService<PostcodeLookupStagingFileImportDto, PostcodeLookupStagingDto, PostcodeLookupStaging>>(new NullLoggerFactory());

            var testConfig = new TestConfiguration();
            MatchingDbContext = testConfig.GetDbContext();
            var matchingConfiguration = TestConfiguration.MatchingConfiguration;

            var repository = new SqlBulkInsertRepository<PostcodeLookupStaging>(loggerRepository, matchingConfiguration);
            var functionLogRepository = new GenericRepository<FunctionLog>(new NullLogger<GenericRepository<FunctionLog>>(), MatchingDbContext);
            
            var dataValidator = new PostcodeLookupStagingDataValidator();
            var dataParser = new PostcodeLookupStagingDataParser();
            var nullDataProcessor = new NullDataProcessor<PostcodeLookupStaging>();
            var csvFileReader = new CsvFileReader<PostcodeLookupStagingFileImportDto, PostcodeLookupStagingDto>(loggerCsvFileReader, dataParser, dataValidator, functionLogRepository);

            var config = new MapperConfiguration(c => c.AddMaps(typeof(PostcodeLookupStagingMapper).Assembly));

            var mapper = new Mapper(config);

            FileImportService = new FileImportService<PostcodeLookupStagingFileImportDto, PostcodeLookupStagingDto, PostcodeLookupStaging>(logger, mapper, csvFileReader, repository, nullDataProcessor);
        }

        public void ResetData(string lepCode = null)
        {
            if (string.IsNullOrWhiteSpace(lepCode))
            {
                MatchingDbContext.Database.ExecuteSqlCommand("DELETE FROM PostcodeLookup");
                MatchingDbContext.SaveChanges();
                return;
            }

            var postcodeLookup = MatchingDbContext.PostcodeLookup.FirstOrDefault(e => e.LepCode == lepCode);

            if (postcodeLookup == null) return;
            
            MatchingDbContext.PostcodeLookup.Remove(postcodeLookup);
            var count = MatchingDbContext.SaveChanges();
            count.Should().Be(1);
        }

        public void AddExisting(string postcode, string lepCode)
        {
            var postcodeLookup = MatchingDbContext.PostcodeLookup.FirstOrDefault(e => e.LepCode == lepCode);

            if (postcodeLookup != null) return;
            
            MatchingDbContext.PostcodeLookup.Add(new Domain.Models.PostcodeLookup
            {
                Postcode = postcode,
                LepCode = lepCode
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