using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.FileImportService
{
    public class When_Import_Is_Called_To_Import_Employers
    {
        private readonly EmployerFileImportDto _fileImportDto;
        private readonly IList<EmployerDto> _fileReaderResults;
        private readonly IFileReader<EmployerFileImportDto, EmployerDto> _fileReader;
        private readonly IRepository<Domain.Models.Employer> _repository;
        private readonly int _result;
        private readonly IDataProcessor<Domain.Models.Employer> _dataProcessor;

        public When_Import_Is_Called_To_Import_Employers()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);
            var logger = Substitute.For<ILogger<FileImportService<EmployerFileImportDto, EmployerDto, Domain.Models.Employer>>>();
            _fileReader = Substitute.For<IFileReader<EmployerFileImportDto, EmployerDto>>();
            _repository = Substitute.For<IRepository<Domain.Models.Employer>>();
            _dataProcessor = Substitute.For<IDataProcessor<Domain.Models.Employer>>();

            _repository
                .CreateMany(Arg.Any<IList<Domain.Models.Employer>>())
                .Returns(callinfo =>
                {
                    var passedEntities = callinfo.ArgAt<IEnumerable<Domain.Models.Employer>>(0);
                    return passedEntities.Count();
                });

            _fileImportDto = new EmployerFileImportDto
            {
                FileDataStream = new MemoryStream()
            };

            _fileReaderResults = Build(2);

            _fileReader.ValidateAndParseFile(_fileImportDto)
                .Returns(Task.FromResult(_fileReaderResults));

            var service = new FileImportService<EmployerFileImportDto, EmployerDto, Domain.Models.Employer>(logger, mapper, _fileReader, _repository, _dataProcessor);

            _result = service.Import(_fileImportDto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ValidateAndParseFile_Is_Called_Exactly_Once()
        {
            _fileReader
                .Received(1)
                .ValidateAndParseFile(_fileImportDto);
        }

        [Fact]
        public void Then_CreateMany_Is_Called_Exactly_Once()
        {
            _repository
                .Received(1)
                .CreateMany(Arg.Any<IList<Domain.Models.Employer>>());
        }

        [Fact]
        public void Then_PreProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PreProcessingHandler(Arg.Any<IList<Domain.Models.Employer>>());
        }

        [Fact]
        public void Then_PostProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PostProcessingHandler(Arg.Any<IList<Domain.Models.Employer>>());
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Created_Records_Is_Returned()
        {
            _fileReaderResults.Count.Should().Be(_result);
        }

        public IList<EmployerDto> Build(int numberOfItems)
        {
            var employerDtos = new List<EmployerDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                employerDtos.Add(new EmployerDto
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
