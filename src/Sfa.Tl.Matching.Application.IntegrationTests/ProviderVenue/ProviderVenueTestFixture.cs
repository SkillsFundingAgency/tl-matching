using System;
using System.Linq;
using AutoMapper;
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
    public class ProviderVenueTestFixture : IDisposable
    {
        public readonly IFileImportService<ProviderVenueFileImportDto, ProviderVenueDto, Domain.Models.ProviderVenue> FileImportService;
        public MatchingDbContext MatchingDbContext;

        public ProviderVenueTestFixture()
        {
            var loggerRepository = new Logger<ProviderRepository>(new NullLoggerFactory());
            var providerVenueloggerRepository = new Logger<ProviderVenueRepository>(new NullLoggerFactory());
            var loggerExcelFileReader = new Logger<ExcelFileReader<ProviderVenueFileImportDto, ProviderVenueDto>>(new NullLoggerFactory());

            var logger = new Logger<FileImportService<ProviderVenueFileImportDto, ProviderVenueDto, Domain.Models.ProviderVenue>>(new NullLoggerFactory());

            MatchingDbContext = new TestConfiguration().GetDbContext();

            var repository = new ProviderRepository(loggerRepository, MatchingDbContext);
            var providerVenuerepository = new ProviderVenueRepository(providerVenueloggerRepository, MatchingDbContext);
            var dataValidator = new ProviderVenueDataValidator(repository, providerVenuerepository);
            var dataParser = new ProviderVenueDataParser();

            var excelFileReader = new ExcelFileReader<ProviderVenueFileImportDto, ProviderVenueDto>(loggerExcelFileReader, dataParser, dataValidator);

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));

            var mapper = new Mapper(config);

            FileImportService = new FileImportService<ProviderVenueFileImportDto, ProviderVenueDto, Domain.Models.ProviderVenue>(logger, mapper, excelFileReader, providerVenuerepository);
        }

        public void ResetData()
        {
            var providerVenue = MatchingDbContext.ProviderVenue.FirstOrDefault(pv => pv.CreatedBy == nameof(ProviderVenueTestFixture));
            if (providerVenue != null) MatchingDbContext.ProviderVenue.Remove(providerVenue);

            var provider = MatchingDbContext.Provider.FirstOrDefault(p => p.Source == nameof(ProviderVenueTestFixture));
            if (provider != null) MatchingDbContext.Provider.Remove(provider);

            MatchingDbContext.SaveChanges();
        }

        public Domain.Models.Provider CreateProvider(int ukprn)
        {
            var provider = new Domain.Models.Provider
            {
                UkPrn = ukprn,
                Name = "Name",
                OfstedRating = 3,
                Status = true,
                PrimaryContact = "PrimaryContact",
                PrimaryContactEmail = "primary@contact.com",
                SecondaryContact = "SecondaryContact",
                SecondaryContactEmail = "secondary@contact.com",
                Source = nameof(ProviderVenueTestFixture),
                CreatedOn = DateTime.Now
            };

            MatchingDbContext.Add(provider);
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