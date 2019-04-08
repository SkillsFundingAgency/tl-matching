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
using Sfa.Tl.Matching.Models.Enums;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.FileImportService
{
    public class When_Import_Is_Called_To_Import_Providers
    {
        private readonly ProviderFileImportDto _fileImportDto;
        private readonly IList<ProviderDto> _fileReaderResults;
        private readonly IFileReader<ProviderFileImportDto, ProviderDto> _fileReader;
        private readonly IRepository<Domain.Models.Provider> _repository;
        private readonly int _result;
        private readonly IDataProcessor<Domain.Models.Provider> _dataProcessor;

        public When_Import_Is_Called_To_Import_Providers()
        {
            var config = new MapperConfiguration(c => c.AddProfiles(typeof(EmployerMapper).Assembly));
            var mapper = new Mapper(config);

            var logger = Substitute.For<ILogger<FileImportService<ProviderFileImportDto, ProviderDto, Domain.Models.Provider>>>();
            _fileReader = Substitute.For<IFileReader<ProviderFileImportDto, ProviderDto>>();
            _repository = Substitute.For<IRepository<Domain.Models.Provider>>();
            _dataProcessor = Substitute.For<IDataProcessor<Domain.Models.Provider>>();

            _repository
                .CreateMany(Arg.Any<IList<Domain.Models.Provider>>())
                .Returns(callinfo =>
                {
                    var passedEntities = callinfo.ArgAt<IEnumerable<Domain.Models.Provider>>(0);
                    return passedEntities.Count();
                });

            _fileImportDto = new ProviderFileImportDto
            {
                FileDataStream = new MemoryStream()
            };

            _fileReaderResults = Build(2);

            _fileReader.ValidateAndParseFile(_fileImportDto).Returns(Task.FromResult(_fileReaderResults));

            var service = new FileImportService<ProviderFileImportDto, ProviderDto, Domain.Models.Provider>(logger, mapper, _fileReader, _repository, _dataProcessor);

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
                .CreateMany(Arg.Any<IList<Domain.Models.Provider>>());
        }

        [Fact]
        public void Then_PreProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PreProcessingHandler(Arg.Any<IList<Domain.Models.Provider>>());
        }

        [Fact]
        public void Then_PostProcessingHandler_Is_Called_Exactly_Once()
        {
            _dataProcessor
                .Received(1)
                .PostProcessingHandler(Arg.Any<IList<Domain.Models.Provider>>());
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Created_Records_Is_Returned()
        {
            _fileReaderResults.Count.Should().Be(_result);
        }

        public IList<ProviderDto> Build(int numberOfItems)
        {
            var providerDtos = new List<ProviderDto>();

            for (var i = 0; i < numberOfItems; i++)
            {
                providerDtos.Add(new ProviderDto
                {
                    UkPrn = 10000546 + i,
                    Name = "ProviderName",
                    OfstedRating = OfstedRating.Good,
                    Status = true,
                    StatusReason = "StatusReason",
                    PrimaryContact = "PrimaryContact",
                    PrimaryContactEmail = "primary@contact.co.uk",
                    PrimaryContactPhone = "01777757777",
                    SecondaryContact = "SecondaryContact",
                    SecondaryContactEmail = "secondary@contact.co.uk",
                    SecondaryContactPhone = "01777757777",
                    Source = "PMF_1018",
                    CreatedBy = "Test"
                });
            }

            return providerDtos;
        }

    }
}
