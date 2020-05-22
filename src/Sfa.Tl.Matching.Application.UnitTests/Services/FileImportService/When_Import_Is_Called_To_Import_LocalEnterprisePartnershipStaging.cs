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
    public class When_Import_Is_Called_To_Import_LocalEnterprisePartnershipStaging
    {
        private readonly LocalEnterprisePartnershipStagingFileImportDto _stagingFileImportDto;
        private readonly IList<LocalEnterprisePartnershipStagingDto> _fileReaderResults;
        private readonly IFileReader<LocalEnterprisePartnershipStagingFileImportDto, LocalEnterprisePartnershipStagingDto> _fileReader;
        private readonly IBulkInsertRepository<LocalEnterprisePartnershipStaging> _repository;
        private readonly int _result;
        private readonly IDataProcessor<LocalEnterprisePartnershipStaging> _dataProcessor;

        public When_Import_Is_Called_To_Import_LocalEnterprisePartnershipStaging()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(LocalEnterprisePartnershipStagingMapper).Assembly));
            var mapper = new Mapper(config);
            var logger = Substitute.For<ILogger<FileImportService<LocalEnterprisePartnershipStagingFileImportDto, LocalEnterprisePartnershipStagingDto, LocalEnterprisePartnershipStaging>>>();
            _fileReader = Substitute.For<IFileReader<LocalEnterprisePartnershipStagingFileImportDto, LocalEnterprisePartnershipStagingDto>>();
            _dataProcessor = Substitute.For<IDataProcessor<LocalEnterprisePartnershipStaging>>();

            _repository = Substitute.For<IBulkInsertRepository<LocalEnterprisePartnershipStaging>>();
            _repository.MergeFromStagingAsync().Returns(2);
            
            _stagingFileImportDto = new LocalEnterprisePartnershipStagingFileImportDto
            {
                FileDataStream = new MemoryStream()
            };

            _fileReaderResults = Build(2);

            _fileReader.ValidateAndParseFileAsync(_stagingFileImportDto)
                .Returns(Task.FromResult(_fileReaderResults));

            var service = new FileImportService<LocalEnterprisePartnershipStagingFileImportDto, LocalEnterprisePartnershipStagingDto, LocalEnterprisePartnershipStaging>(logger, mapper, _fileReader, _repository, _dataProcessor);

            _result = service.BulkImportAsync(_stagingFileImportDto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_ValidateAndParseFile_Is_Called_Exactly_Once()
        {
            _fileReader
                .Received(1)
                .ValidateAndParseFileAsync(_stagingFileImportDto);
        }

        [Fact]
        public void Then_CreateMany_Is_Called_Exactly_Once()
        {
            _repository
                .Received(1)
                .BulkInsertAsync(Arg.Any<IList<LocalEnterprisePartnershipStaging>>());
        }

        [Fact]
        public void Then_PreProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PreProcessingHandler(Arg.Any<IList<LocalEnterprisePartnershipStaging>>());
        }

        [Fact]
        public void Then_PostProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PostProcessingHandler(Arg.Any<IList<LocalEnterprisePartnershipStaging>>());
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Created_Records_Is_Returned()
        {
            _fileReaderResults.Count.Should().Be(_result);
        }

        public IList<LocalEnterprisePartnershipStagingDto> Build(int numberOfItems)
        {
            var learningAimReferenceDtos = new List<LocalEnterprisePartnershipStagingDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                learningAimReferenceDtos.Add(new LocalEnterprisePartnershipStagingDto
                {
                   Code = $"E00000000{i}",
                   Name = $"LEP {i}",
                   CreatedBy = "Test"
                });
            }

            return learningAimReferenceDtos;
        }
    }
}
