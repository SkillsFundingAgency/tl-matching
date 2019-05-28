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
    public class When_Import_Is_Called_To_Import_ProviderQualifications
    {
        private readonly ProviderQualificationFileImportDto _fileImportDto;
        private readonly IList<ProviderQualificationDto> _fileReaderResults;
        private readonly IFileReader<ProviderQualificationFileImportDto, ProviderQualificationDto> _fileReader;
        private readonly IRepository<Domain.Models.ProviderQualification> _repository;
        private readonly int _result;
        private readonly IDataProcessor<Domain.Models.ProviderQualification> _dataProcessor;

        public When_Import_Is_Called_To_Import_ProviderQualifications()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);
            var logger = Substitute.For<ILogger<FileImportService<ProviderQualificationFileImportDto, ProviderQualificationDto, Domain.Models.ProviderQualification>>>();
            _fileReader = Substitute.For<IFileReader<ProviderQualificationFileImportDto, ProviderQualificationDto>>();
            _repository = Substitute.For<IRepository<Domain.Models.ProviderQualification>>();
            _dataProcessor = Substitute.For<IDataProcessor<Domain.Models.ProviderQualification>>();

            _repository
                .CreateMany(Arg.Any<IList<Domain.Models.ProviderQualification>>())
                .Returns(callinfo =>
                {
                    var passedEntities = callinfo.ArgAt<IEnumerable<Domain.Models.ProviderQualification>>(0);
                    return passedEntities.Count();
                });

            _fileImportDto = new ProviderQualificationFileImportDto
            {
                FileDataStream = new MemoryStream()
            };

            _fileReaderResults = Build(2);

            _fileReader.ValidateAndParseFile(_fileImportDto)
                .Returns(Task.FromResult(_fileReaderResults));

            var service = new FileImportService<ProviderQualificationFileImportDto, ProviderQualificationDto, Domain.Models.ProviderQualification>(logger, mapper, _fileReader, _repository, _dataProcessor);

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
                .CreateMany(Arg.Any<IList<Domain.Models.ProviderQualification>>());
        }

        [Fact]
        public void Then_PreProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PreProcessingHandler(Arg.Any<IList<Domain.Models.ProviderQualification>>());
        }

        [Fact]
        public void Then_PostProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PostProcessingHandler(Arg.Any<IList<Domain.Models.ProviderQualification>>());
        }
        [Fact]
        public void Then_The_Expected_Number_Of_Created_Records_Is_Returned()
        {
            _fileReaderResults.Count.Should().Be(_result);
        }

        public IList<ProviderQualificationDto> Build(int numberOfItems)
        {
            var providerQualificationDtos = new List<ProviderQualificationDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                providerQualificationDtos.Add(new ProviderQualificationDto
                {
                    ProviderVenueId = 10000546 + i,
                    QualificationId = 1,
                    NumberOfPlacements = 1,
                    Source = "PMF_1018",
                    CreatedBy = "Test"
                });
            }

            return providerQualificationDtos;
        }
    }
}
