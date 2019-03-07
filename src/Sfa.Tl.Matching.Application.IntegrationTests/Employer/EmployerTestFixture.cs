﻿using System;
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
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.IntegrationTests.Employer
{
    public class EmployerTestFixture : IDisposable
    {
        public FileImportService<EmployerFileImportDto, EmployerDto, Domain.Models.Employer> FileImportService;
        public MatchingDbContext MatchingDbContext;

        public EmployerTestFixture()
        {
            var loggerRepository = new Logger<EmployerRepository>(new NullLoggerFactory());
            var loggerExcelFileReader = new Logger<ExcelFileReader<EmployerFileImportDto, EmployerDto>>(new NullLoggerFactory());

            var logger = new Logger<FileImportService<EmployerFileImportDto, EmployerDto, Domain.Models.Employer>>(new NullLoggerFactory());

            MatchingDbContext = new TestConfiguration().GetDbContext();

            var repository = new EmployerRepository(loggerRepository, MatchingDbContext);
            var dataValidator = new EmployerDataValidator();
            var dataParser = new EmployerDataParser();

            var excelFileReader = new ExcelFileReader<EmployerFileImportDto, EmployerDto>(dataParser, dataValidator);
            excelFileReader._logger = loggerExcelFileReader;

            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));

            var mapper = new Mapper(config);

            FileImportService = new FileImportService<EmployerFileImportDto, EmployerDto, Domain.Models.Employer>(mapper, excelFileReader, repository);
            FileImportService._logger = logger;
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