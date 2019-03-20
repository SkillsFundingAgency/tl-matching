using System;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.QualificationRoutePathMapping;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.QualificationRoutePathMapping
{
    public class QualificationRoutePathMappingServiceTestFixture : IDisposable
    {
        public MatchingDbContext MatchingDbContext;
        public IFileImportService<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto, Domain.Models.QualificationRoutePathMapping> FileImportService;

        public QualificationRoutePathMappingServiceTestFixture()
        {
            var loggerRepository = new Logger<QualificationRoutePathMappingRepository>(new NullLoggerFactory());
            var loggerQualificationRepository = new Logger<GenericRepository<Qualification>>(new NullLoggerFactory());
            var loggerPathRepository = new Logger<GenericRepository<Path>>(new NullLoggerFactory());
            var loggerExcelFileReader = new Logger<ExcelFileReader<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto>>(new NullLoggerFactory());
            var logger = new Logger<FileImportService<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto, Domain.Models.QualificationRoutePathMapping>>(new NullLoggerFactory());

            MatchingDbContext = new TestConfiguration().GetDbContext();

            var qualificationRoutePathMappingRepository = new QualificationRoutePathMappingRepository(loggerRepository, MatchingDbContext);
            var qualificationRepository = new GenericRepository<Qualification>(loggerQualificationRepository, MatchingDbContext);
            var pathRepository = new GenericRepository<Path>(loggerPathRepository, MatchingDbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(new NullLogger<GenericRepository<FunctionLog>>(), MatchingDbContext);
            var dataValidator = new QualificationRoutePathMappingDataValidator(qualificationRoutePathMappingRepository, qualificationRepository, pathRepository);
            var dataParser = new QualificationRoutePathMappingDataParser();
            var nullDataProcessor = new NullDataProcessor<Domain.Models.QualificationRoutePathMapping>();
            var excelFileReader = new ExcelFileReader<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto>(loggerExcelFileReader, dataParser, dataValidator, functionLogRepository);

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);

            FileImportService = new FileImportService<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto, Domain.Models.QualificationRoutePathMapping>(
                logger,
                mapper,
                excelFileReader,
                qualificationRoutePathMappingRepository,
                nullDataProcessor);
        }

        public void ResetData(string larsId)
        {
            var qualifications = MatchingDbContext.Qualification.Include(q => q.QualificationRoutePathMapping).Where(rpm => rpm.LarsId == larsId);
            if (qualifications.Any())
            {
                MatchingDbContext.Qualification.RemoveRange(qualifications);
                MatchingDbContext.QualificationRoutePathMapping.RemoveRange(qualifications.SelectMany(q => q.QualificationRoutePathMapping));
                MatchingDbContext.SaveChanges();
            }
        }

        public void CreateQualificationRoutePathMapping(string larsId)
        {
            var routePathMapping = new Domain.Models.QualificationRoutePathMapping
            {
                Qualification = new Qualification
                {
                    LarsId = larsId, //Must match id in QualificationRoutePathMapping-Simple.xlsx
                    Title = "Test",
                    ShortTitle = "Test",
                    CreatedBy = nameof(QualificationRoutePathMappingServiceTestFixture)
                },
                PathId = 10,
                Source = "Test",
                CreatedBy = nameof(QualificationRoutePathMappingServiceTestFixture)
            };

            MatchingDbContext.Add(routePathMapping);
            MatchingDbContext.SaveChanges();
        }

        public void Dispose()
        {
            MatchingDbContext?.Dispose();
        }
    }
}