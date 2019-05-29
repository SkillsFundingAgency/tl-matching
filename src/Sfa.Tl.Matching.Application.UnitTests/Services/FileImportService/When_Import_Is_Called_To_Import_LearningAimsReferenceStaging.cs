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
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.FileImportService
{
    public class When_Import_Is_Called_To_Import_LearningAimsReferenceStaging
    {
        private readonly LearningAimsReferenceStagingFileImportDto _stagingFileImportDto;
        private readonly IList<LearningAimsReferenceStagingDto> _fileReaderResults;
        private readonly IFileReader<LearningAimsReferenceStagingFileImportDto, LearningAimsReferenceStagingDto> _fileReader;
        private readonly IRepository<LearningAimsReferenceStaging> _repository;
        private readonly int _result;
        private readonly IDataProcessor<LearningAimsReferenceStaging> _dataProcessor;

        public When_Import_Is_Called_To_Import_LearningAimsReferenceStaging()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(LearningAimsReferenceStagingMapper).Assembly));
            var mapper = new Mapper(config);
            var logger = Substitute.For<ILogger<FileImportService<LearningAimsReferenceStagingFileImportDto, LearningAimsReferenceStagingDto, LearningAimsReferenceStaging>>>();
            _fileReader = Substitute.For<IFileReader<LearningAimsReferenceStagingFileImportDto, LearningAimsReferenceStagingDto>>();
            _repository = Substitute.For<IRepository<LearningAimsReferenceStaging>>();
            _dataProcessor = Substitute.For<IDataProcessor<LearningAimsReferenceStaging>>();

            _repository
                .CreateMany(Arg.Any<IList<LearningAimsReferenceStaging>>())
                .Returns(callinfo =>
                {
                    var passedEntities = callinfo.ArgAt<IEnumerable<LearningAimsReferenceStaging>>(0);
                    return passedEntities.Count();
                });

            _stagingFileImportDto = new LearningAimsReferenceStagingFileImportDto
            {
                FileDataStream = new MemoryStream()
            };

            _fileReaderResults = Build(2);

            _fileReader.ValidateAndParseFile(_stagingFileImportDto)
                .Returns(Task.FromResult(_fileReaderResults));

            var service = new FileImportService<LearningAimsReferenceStagingFileImportDto, LearningAimsReferenceStagingDto, LearningAimsReferenceStaging>(logger, mapper, _fileReader, _repository, _dataProcessor);

            _result = service.Import(_stagingFileImportDto).GetAwaiter().GetResult();
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
                .CreateMany(Arg.Any<IList<LearningAimsReferenceStaging>>());
        }

        [Fact]
        public void Then_PreProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PreProcessingHandler(Arg.Any<IList<LearningAimsReferenceStaging>>());
        }

        [Fact]
        public void Then_PostProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PostProcessingHandler(Arg.Any<IList<LearningAimsReferenceStaging>>());
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Created_Records_Is_Returned()
        {
            _fileReaderResults.Count.Should().Be(_result);
        }

        public IList<LearningAimsReferenceStagingDto> Build(int numberOfItems)
        {
            var learningAimsReferenceDtos = new List<LearningAimsReferenceStagingDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                learningAimsReferenceDtos.Add(new LearningAimsReferenceStagingDto
                {
                   Title = "LearningAimsReference",
                   LarId = (10000000 + i).ToString(),
                   AwardOrgLarId = (10000000 + i).ToString(),
                   SourceCreatedOn = DateTime.UtcNow,
                   SourceModifiedOn = DateTime.UtcNow,
                   CreatedBy = "Test"
                });
            }

            return learningAimsReferenceDtos;
        }
    }
}
