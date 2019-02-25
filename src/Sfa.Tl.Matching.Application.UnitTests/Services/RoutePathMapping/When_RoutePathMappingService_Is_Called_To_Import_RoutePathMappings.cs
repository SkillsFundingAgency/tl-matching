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
    public class When_RoutePathMappingService_Is_Called_To_Import_RoutePathMappings
    {
        private readonly QualificationRoutePathMappingFileImportDto _fileImportDto;
        private readonly IEnumerable<RoutePathMappingDto> _fileReaderResults;
        private readonly IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto> _fileReader;
        private readonly IRepository<Domain.Models.RoutePathMapping> _repository;
        private readonly int _result;

        public When_RoutePathMappingService_Is_Called_To_Import_RoutePathMappings()
        {
            var logger = Substitute.For<ILogger<RoutePathMappingService>>();
            var config = new MapperConfiguration(c => c.AddProfile<RoutePathMappingMapper>());
            var mapper = new Mapper(config);
            _fileReader =
                Substitute.For<IFileReader<QualificationRoutePathMappingFileImportDto, RoutePathMappingDto>>();
            _repository = Substitute.For<IRepository<Domain.Models.RoutePathMapping>>();

            _repository
                .CreateMany(Arg.Any<IEnumerable<Domain.Models.RoutePathMapping>>())
                .Returns(callinfo =>
                {
                    var passedEntities = callinfo.ArgAt<IEnumerable<Domain.Models.RoutePathMapping>>(0);
                    return passedEntities.Count();
                });

            _fileImportDto = new QualificationRoutePathMappingFileImportDto
            {
                FileDataStream = new MemoryStream()
            };

            _fileReaderResults = new ValidRoutePathMappingDtoListBuilder(2).Build();

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
                .CreateMany(Arg.Any<IEnumerable<Domain.Models.RoutePathMapping>>());
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Created_Records_Is_Returned()
        {
            Assert.Equal(_fileReaderResults.Count(), _result);
        }
    }
}
