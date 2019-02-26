using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.RoutePath.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.RoutePathMapping
{
    public class When_QualificationRoutePathMappingService_Is_Called_To_Import_QualificationRoutePathMappings
    {
        private readonly QualificationRoutePathMappingFileImportDto _fileImportDto;
        private readonly IEnumerable<QualificationRoutePathMappingDto> _fileReaderResults;
        private readonly IFileReader<QualificationRoutePathMappingFileImportDto, QualificationRoutePathMappingDto> _fileReader;
        private readonly IRepository<QualificationRoutePathMapping> _repository;
        private readonly int _result;

        public When_QualificationRoutePathMappingService_Is_Called_To_Import_QualificationRoutePathMappings()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);
            _fileReader =
                Substitute.For<IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto>>();
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

            _fileReader.ValidateAndParseFile(_fileImportDto)
                .Returns(_fileReaderResults);

            var service =
                new RoutePathMappingService(logger, mapper, _fileReader,  _repository);

            _result = service.ImportQualificationPathMapping(_fileImportDto).GetAwaiter().GetResult();
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
            Assert.Equal(_fileReaderResults.Count(), _result);
        }
    }
}
