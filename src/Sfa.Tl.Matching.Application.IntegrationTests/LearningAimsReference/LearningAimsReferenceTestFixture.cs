using System;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.LearningAimsReferenceStaging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.LearningAimsReference
{
    public class LearningAimsReferenceTestFixture : IDisposable
    {
        public IFileImportService<LearningAimsReferenceStagingFileImportDto> FileImportService;
        public MatchingDbContext MatchingDbContext;

        public LearningAimsReferenceTestFixture()
        {
            var loggerRepository = new Logger<GenericRepository<LearningAimsReferenceStaging>>(new NullLoggerFactory());
            var loggerCsvFileReader = new Logger<CsvFileReader<LearningAimsReferenceStagingFileImportDto, LearningAimsReferenceStagingDto>>(new NullLoggerFactory());

            var logger = new Logger<FileImportService<LearningAimsReferenceStagingFileImportDto, LearningAimsReferenceStagingDto, LearningAimsReferenceStaging>>(new NullLoggerFactory());

            MatchingDbContext = new TestConfiguration().GetDbContext();

            var repository = new GenericRepository<LearningAimsReferenceStaging>(loggerRepository, MatchingDbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(new NullLogger<GenericRepository<FunctionLog>>(), MatchingDbContext);
            
            var dataValidator = new LearningAimsReferenceStagingDataValidator();
            var dataParser = new LearningAimsReferenceStagingDataParser();
            var nullDataProcessor = new NullDataProcessor<LearningAimsReferenceStaging>();
            var csvFileReader = new CsvFileReader<LearningAimsReferenceStagingFileImportDto, LearningAimsReferenceStagingDto>(loggerCsvFileReader, dataParser, dataValidator, functionLogRepository);

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(LearningAimsReferenceStagingMapper).Assembly));

            var mapper = new Mapper(config);

            FileImportService = new FileImportService<LearningAimsReferenceStagingFileImportDto, LearningAimsReferenceStagingDto, LearningAimsReferenceStaging>(logger, mapper, csvFileReader, repository, nullDataProcessor);
        }

        public void ResetData(string title = null)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                MatchingDbContext.Database.ExecuteSqlCommand("DELETE FROM LearningAimsReference");
                MatchingDbContext.SaveChanges();
                return;
            }

            var learningAimsReference = MatchingDbContext.LearningAimsReference.FirstOrDefault(e => e.Title == title);

            if (learningAimsReference == null) return;
            
            MatchingDbContext.LearningAimsReference.Remove(learningAimsReference);
            var count = MatchingDbContext.SaveChanges();
            count.Should().Be(1);
        }

        public void AddExisting(string larId, string title)
        {
            var learningAimsReference = MatchingDbContext.LearningAimsReference.FirstOrDefault(e => e.Title == title);

            if (learningAimsReference != null) return;
            
            MatchingDbContext.LearningAimsReference.Add(new Domain.Models.LearningAimsReference
            {
                Title = title,
                AwardOrgLarId = "",
                LarId = larId,
                SourceCreatedOn = new DateTime(2019, 01, 01),
                SourceModifiedOn = new DateTime(2019, 01, 01),
            });

            var count = MatchingDbContext.SaveChanges();
            count.Should().Be(1);
        }

        public void UpdateExisting(string title)
        {
            var learningAimsReference = MatchingDbContext.LearningAimsReference.FirstOrDefault(e => e.Title == title);

            if (learningAimsReference != null) return;
            
            MatchingDbContext.LearningAimsReference.Add(new Domain.Models.LearningAimsReference
            {
                Title = title,
                AwardOrgLarId = "",
                LarId = "12345678",
                SourceCreatedOn = new DateTime(2019, 01, 01),
                SourceModifiedOn = new DateTime(2019, 01, 01),
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