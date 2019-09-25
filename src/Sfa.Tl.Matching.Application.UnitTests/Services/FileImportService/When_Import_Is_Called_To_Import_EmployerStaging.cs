using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.FileImportService
{
    public class When_Import_Is_Called_To_Import_EmployerStaging
    {
        private readonly int _result;
        private readonly EmployerStagingFileImportDto _stagingFileImportDto;
        private readonly IList<EmployerStagingDto> _fileReaderResults;
        private readonly IFileReader<EmployerStagingFileImportDto, EmployerStagingDto> _fileReader;
        private readonly IBulkInsertRepository<EmployerStaging> _repository;
        private readonly IDataProcessor<EmployerStaging> _dataProcessor;

        public When_Import_Is_Called_To_Import_EmployerStaging()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);
            var logger = Substitute.For<ILogger<FileImportService<EmployerStagingFileImportDto, EmployerStagingDto, EmployerStaging>>>();
            _fileReader = Substitute.For<IFileReader<EmployerStagingFileImportDto, EmployerStagingDto>>();
            _dataProcessor = Substitute.For<IDataProcessor<EmployerStaging>>();

            _repository = Substitute.For<IBulkInsertRepository<EmployerStaging>>();
            _repository.MergeFromStaging().Returns(2);

            _stagingFileImportDto = new EmployerStagingFileImportDto
            {
                FileDataStream = new MemoryStream()
            };

            _fileReaderResults = Build(2);

            _fileReader.ValidateAndParseFile(_stagingFileImportDto)
                .Returns(Task.FromResult(_fileReaderResults));

            var service = new FileImportService<EmployerStagingFileImportDto, EmployerStagingDto, EmployerStaging>(logger, mapper, _fileReader, _repository, _dataProcessor);

            _result = service.BulkImport(_stagingFileImportDto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ValidateAndParseFile_Is_Called_Exactly_Once()
        {
            _fileReader
                .Received(1)
                .ValidateAndParseFile(_stagingFileImportDto);
        }

        [Fact]
        public void Then_CreateMany_Is_Called_Exactly_Once()
        {
            _repository
                .Received(1)
                .BulkInsert(Arg.Any<IList<EmployerStaging>>());
        }

        [Fact]
        public void Then_PreProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PreProcessingHandler(Arg.Any<IList<EmployerStaging>>());
        }

        [Fact]
        public void Then_PostProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PostProcessingHandler(Arg.Any<IList<EmployerStaging>>());
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Created_Records_Is_Returned()
        {
            _fileReaderResults.Count.Should().Be(_result);
        }

        public IList<EmployerStagingDto> Build(int numberOfItems)
        {
            var employerDtos = new List<EmployerStagingDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                employerDtos.Add(new EmployerStagingDto
                {
                    CrmId = new Guid($"{i}F7B99CB-0FAD-4FFC-AF6A-D5537293E04B"),
                    CompanyName = "Company Name",
                    AlsoKnownAs = "Also Known As",
                    Aupa = "Aware",
                    CompanyType = "CompanyType",
                    PrimaryContact = "PrimaryContact",
                    Phone = "01777757777",
                    Email = "primary@contact.co.uk",
                    Postcode = "AA1 1AA",
                    Owner = "Owner",
                    CreatedBy = "Test"
                });
            }

            return employerDtos;
        }
    }
}
