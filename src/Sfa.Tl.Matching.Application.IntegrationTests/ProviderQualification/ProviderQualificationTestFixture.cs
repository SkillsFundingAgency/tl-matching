using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.ProviderQualification;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data;
using Sfa.Tl.Matching.Data.Repositories;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.ProviderQualification
{
    public class ProviderQualificationTestFixture : IDisposable
    {
        public readonly IFileImportService<ProviderQualificationFileImportDto> FileImportService;
        public MatchingDbContext MatchingDbContext;

        public const string CreatedByUser = "TestUser";
        public static DateTime CreatedOn = DateTime.Parse("2019/02/15 11:12:13");

        public ProviderQualificationTestFixture()
        {
            var providerQualificationloggerRepository = new Logger<GenericRepository<Domain.Models.ProviderQualification>>(new NullLoggerFactory());
            var providerVenueloggerRepository = new Logger<GenericRepository<Domain.Models.ProviderVenue>>(new NullLoggerFactory());
            var qualificationloggerRepository = new Logger<GenericRepository<Qualification>>(new NullLoggerFactory());
            var loggerExcelFileReader = new Logger<ExcelFileReader<ProviderQualificationFileImportDto, ProviderQualificationDto>>(new NullLoggerFactory());

            var logger = new Logger<FileImportService<ProviderQualificationFileImportDto, ProviderQualificationDto, Domain.Models.ProviderQualification>>(new NullLoggerFactory());

            MatchingDbContext = new TestConfiguration().GetDbContext();

            var providerVenuerepository = new GenericRepository<Domain.Models.ProviderVenue>(providerVenueloggerRepository, MatchingDbContext);
            var providerQualificationrepository = new GenericRepository<Domain.Models.ProviderQualification>(providerQualificationloggerRepository, MatchingDbContext);
            var qualificationrepository = new GenericRepository<Qualification>(qualificationloggerRepository, MatchingDbContext);
            var functionLogRepository = new GenericRepository<FunctionLog>(new NullLogger<GenericRepository<FunctionLog>>(), MatchingDbContext);
            
            var dataValidator = new ProviderQualificationDataValidator(providerVenuerepository, providerQualificationrepository, qualificationrepository);
            var dataParser = new ProviderQualificationDataParser();
            var nullDataProcessor = new NullDataProcessor<Domain.Models.ProviderQualification>();
            var excelFileReader = new ExcelFileReader<ProviderQualificationFileImportDto, ProviderQualificationDto>(loggerExcelFileReader, dataParser, dataValidator, functionLogRepository);

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));

            var mapper = new Mapper(config);

            FileImportService = new FileImportService<ProviderQualificationFileImportDto, ProviderQualificationDto, Domain.Models.ProviderQualification>(logger, mapper, excelFileReader, providerQualificationrepository, nullDataProcessor);
        }

        public void ResetData()
        {
            var providerQualification = MatchingDbContext.ProviderQualification.FirstOrDefault(p => p.CreatedBy == nameof(ProviderQualificationTestFixture));
            if (providerQualification != null) MatchingDbContext.Remove(providerQualification);

            var providerVenue = MatchingDbContext.ProviderVenue.FirstOrDefault(p => p.Source == nameof(ProviderQualificationTestFixture));
            if (providerVenue != null) MatchingDbContext.Remove(providerVenue);

            var routePathMapping = MatchingDbContext.QualificationRoutePathMapping.FirstOrDefault(p => p.Source == nameof(ProviderQualificationTestFixture));
            if (routePathMapping != null) MatchingDbContext.Remove(routePathMapping);
            
            var qualification = MatchingDbContext.Qualification.FirstOrDefault(p => p.ShortTitle == nameof(ProviderQualificationTestFixture));
            if (qualification != null) MatchingDbContext.Remove(qualification);

            var provider = MatchingDbContext.Provider.FirstOrDefault(p => p.Source == nameof(ProviderQualificationTestFixture));
            if (provider != null) MatchingDbContext.Remove(provider);

            MatchingDbContext.SaveChanges();
        }

        public Domain.Models.Provider CreateVenueAndQualification(int ukPrn, string postCode, string larsId)
        {
            var provider = new Domain.Models.Provider
            {
                UkPrn = ukPrn,
                Name = "Name",
                OfstedRating = 3,
                Status = true,
                PrimaryContact = "PrimaryContact",
                PrimaryContactEmail = "primary@contact.com",
                SecondaryContact = "SecondaryContact",
                SecondaryContactEmail = "secondary@contact.com",
                Source = nameof(ProviderQualificationTestFixture),
                CreatedOn = CreatedOn,
                CreatedBy = CreatedByUser,

                ProviderVenue = new List<Domain.Models.ProviderVenue>
                {
                    new Domain.Models.ProviderVenue
                    {
                        Postcode = postCode,
                        Source = nameof(ProviderQualificationTestFixture),
                        CreatedOn = CreatedOn,
                        CreatedBy = CreatedByUser
                    }
                }
            };

            var qualification = new Qualification
            {
                LarsId = larsId,
                Title = "Title",
                ShortTitle = nameof(ProviderQualificationTestFixture),
                CreatedOn = CreatedOn,
                CreatedBy = CreatedByUser,
                QualificationRoutePathMapping = new List<Domain.Models.QualificationRoutePathMapping>
                {
                    new Domain.Models.QualificationRoutePathMapping
                    {
                        PathId = 10,
                        Source = nameof(ProviderQualificationTestFixture),
                        CreatedOn = CreatedOn,
                        CreatedBy = CreatedByUser                        
                    }
                }
            };

            MatchingDbContext.Add(provider);
            MatchingDbContext.Add(qualification);

            MatchingDbContext.SaveChanges();

            return provider;
        }

        public void Dispose()
        {
            ResetData();
            MatchingDbContext?.Dispose();
        }
    }
}