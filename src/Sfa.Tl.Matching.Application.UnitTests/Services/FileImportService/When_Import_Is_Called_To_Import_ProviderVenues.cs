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
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.FileImportService
{
    public class When_Import_Is_Called_To_Import_ProviderVenues
    {
        private readonly ProviderVenueFileImportDto _fileImportDto;
        private readonly IList<ProviderVenueDto> _fileReaderResults;
        private readonly IFileReader<ProviderVenueFileImportDto, ProviderVenueDto> _fileReader;
        private readonly IRepository<ProviderVenue> _repository;
        private readonly int _result;
        private readonly IDataProcessor<ProviderVenue> _dataProcessor;

        public When_Import_Is_Called_To_Import_ProviderVenues()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);
            var logger = Substitute.For<ILogger<FileImportService<ProviderVenueFileImportDto, ProviderVenueDto, ProviderVenue>>>();
            _fileReader = Substitute.For<IFileReader<ProviderVenueFileImportDto, ProviderVenueDto>>();
            _repository = Substitute.For<IRepository<ProviderVenue>>();
            _dataProcessor = Substitute.For<IDataProcessor<ProviderVenue>>();

            _repository
                .CreateMany(Arg.Any<IList<ProviderVenue>>())
                .Returns(callinfo =>
                {
                    var passedEntities = callinfo.ArgAt<IEnumerable<ProviderVenue>>(0);
                    return passedEntities.Count();
                });

            _fileImportDto = new ProviderVenueFileImportDto
            {
                FileDataStream = new MemoryStream()
            };

            _fileReaderResults = Build(2);

            _fileReader.ValidateAndParseFile(_fileImportDto)
                .Returns(Task.FromResult(_fileReaderResults));

            var service = new FileImportService<ProviderVenueFileImportDto, ProviderVenueDto, ProviderVenue>(logger, mapper, _fileReader, _repository, _dataProcessor);

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
                .CreateMany(Arg.Any<IList<ProviderVenue>>());
        }

        [Fact]
        public void Then_PreProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PreProcessingHandler(Arg.Any<IList<ProviderVenue>>());
        }

        [Fact]
        public void Then_PostProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PostProcessingHandler(Arg.Any<IList<ProviderVenue>>());
        }
        [Fact]
        public void Then_The_Expected_Number_Of_Created_Records_Is_Returned()
        {
            _fileReaderResults.Count.Should().Be(_result);
        }

        public IList<ProviderVenueDto> Build(int numberOfItems)
        {
            var providerVenueDtos = new List<ProviderVenueDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                providerVenueDtos.Add(new ProviderVenueDto
                {
                    ProviderId = 10000546 + i,
                    Postcode = "AA1 1AA",
                    Source = "PMF_1018",
                    CreatedBy = "Test"
                });
            }

            return providerVenueDtos;
        }
    }
}
