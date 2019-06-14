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
    public class When_Import_Is_Called_To_Import_LearningAimReferenceStaging
    {
        private readonly LearningAimReferenceStagingFileImportDto _stagingFileImportDto;
        private readonly IList<LearningAimReferenceStagingDto> _fileReaderResults;
        private readonly IFileReader<LearningAimReferenceStagingFileImportDto, LearningAimReferenceStagingDto> _fileReader;
        private readonly IBulkInsertRepository<LearningAimReferenceStaging> _repository;
        private readonly int _result;
        private readonly IDataProcessor<LearningAimReferenceStaging> _dataProcessor;

        public When_Import_Is_Called_To_Import_LearningAimReferenceStaging()
        {
            var config = new MapperConfiguration(c => c.AddMaps(typeof(LearningAimReferenceStagingMapper).Assembly));
            var mapper = new Mapper(config);
            var logger = Substitute.For<ILogger<FileImportService<LearningAimReferenceStagingFileImportDto, LearningAimReferenceStagingDto, LearningAimReferenceStaging>>>();
            _fileReader = Substitute.For<IFileReader<LearningAimReferenceStagingFileImportDto, LearningAimReferenceStagingDto>>();
            _dataProcessor = Substitute.For<IDataProcessor<LearningAimReferenceStaging>>();

            _repository = Substitute.For<IBulkInsertRepository<LearningAimReferenceStaging>>();
            _repository.MergeFromStaging().Returns(2);
            
            _stagingFileImportDto = new LearningAimReferenceStagingFileImportDto
            {
                FileDataStream = new MemoryStream()
            };

            _fileReaderResults = Build(2);

            _fileReader.ValidateAndParseFile(_stagingFileImportDto)
                .Returns(Task.FromResult(_fileReaderResults));

            var service = new FileImportService<LearningAimReferenceStagingFileImportDto, LearningAimReferenceStagingDto, LearningAimReferenceStaging>(logger, mapper, _fileReader, _repository, _dataProcessor);

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
                .BulkInsert(Arg.Any<IList<LearningAimReferenceStaging>>());
        }

        [Fact]
        public void Then_PreProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PreProcessingHandler(Arg.Any<IList<LearningAimReferenceStaging>>());
        }

        [Fact]
        public void Then_PostProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PostProcessingHandler(Arg.Any<IList<LearningAimReferenceStaging>>());
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Created_Records_Is_Returned()
        {
            _fileReaderResults.Count.Should().Be(_result);
        }

        public IList<LearningAimReferenceStagingDto> Build(int numberOfItems)
        {
            var learningAimReferenceDtos = new List<LearningAimReferenceStagingDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                learningAimReferenceDtos.Add(new LearningAimReferenceStagingDto
                {
                   Title = "LearningAimReference",
                   LarId = (10000000 + i).ToString(),
                   AwardOrgLarId = (10000000 + i).ToString(),
                   SourceCreatedOn = DateTime.UtcNow,
                   SourceModifiedOn = DateTime.UtcNow,
                   CreatedBy = "Test"
                });
            }

            return learningAimReferenceDtos;
        }
    }
}
