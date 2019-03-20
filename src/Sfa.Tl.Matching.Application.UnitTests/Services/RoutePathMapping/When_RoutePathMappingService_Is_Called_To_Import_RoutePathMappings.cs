using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePathMapping
{
    public class When_QualificationRoutePathMappingService_Is_Called_To_Import_QualificationRoutePathMappings
    {
        private readonly QualificationRoutePathMappingFileImportDto _fileImportDto;
        private readonly IList<QualificationRoutePathMappingDto> _fileReaderResults;
        private readonly IFileReader<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto> _fileReader;
        private readonly IRepository<QualificationRoutePathMapping> _repository;
        private readonly int _result;
        private readonly IDataProcessor<QualificationRoutePathMapping> _dataProcessor;

        public When_QualificationRoutePathMappingService_Is_Called_To_Import_QualificationRoutePathMappings()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);

            var logger = Substitute.For<ILogger<FileImportService<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto, QualificationRoutePathMapping>>>();
            _fileReader = Substitute.For<IFileReader<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto>>();
            _dataProcessor = Substitute.For<IDataProcessor<QualificationRoutePathMapping>>();
            _repository = Substitute.For<IRepository<QualificationRoutePathMapping>>();
            _repository
                .CreateMany(Arg.Any<IList<QualificationRoutePathMapping>>())
                .Returns(callinfo =>
                {
                    var passedEntities = callinfo.ArgAt<IEnumerable<QualificationRoutePathMapping>>(0);
                    return passedEntities.Count();
                });

            _fileImportDto = new QualificationRoutePathMappingFileImportDto
            {
                FileDataStream = new MemoryStream()
            };

            _fileReaderResults = new ValidRoutePathMappingDtoListBuilder().Build(2);

            _fileReader.ValidateAndParseFile(_fileImportDto).Returns(Task.FromResult(_fileReaderResults));

            var service = new FileImportService<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto, QualificationRoutePathMapping>(logger, mapper, _fileReader, _repository, _dataProcessor);

            _result = service.Import(_fileImportDto).GetAwaiter().GetResult();
        }

        [Fact]
        public void Then_PreProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PreProcessingHandler(Arg.Any<IList<QualificationRoutePathMapping>>());
        }

        [Fact]
        public void Then_PostProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PostProcessingHandler(Arg.Any<IList<QualificationRoutePathMapping>>());
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
                .CreateMany(Arg.Any<IList<QualificationRoutePathMapping>>());
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Created_Records_Is_Returned()
        {
            Assert.Equal(_fileReaderResults.Count, _result);
        }
    }
}
