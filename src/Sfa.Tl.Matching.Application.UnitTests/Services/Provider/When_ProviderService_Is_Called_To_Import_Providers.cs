using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Provider.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Provider
{
    public class When_ProviderService_Is_Called_To_Import_Providers
    {
        private readonly ProviderFileImportDto _fileImportDto;
        private readonly IEnumerable<ProviderDto> _fileReaderResults;
        private readonly IFileReader<ProviderFileImportDto, ProviderDto> _fileReader;
        private readonly IRepository<Domain.Models.Provider> _repository;
        private readonly int _result;

        public When_ProviderService_Is_Called_To_Import_Providers()
        {
            var config = new MapperConfiguration(c => c.AddProfile<ProviderMapper>());
            var mapper = new Mapper(config);
            var searchResultconfig = new MapperConfiguration(c => c.AddProfile<ProviderVenueSearchResultMapper>());
            var searchResultMapper = new Mapper(searchResultconfig);

            _fileReader =
                Substitute.For<IFileReader<ProviderFileImportDto, ProviderDto>>();
            _repository = Substitute.For<IRepository<Domain.Models.Provider>>();

            _repository
                .CreateMany(Arg.Any<IEnumerable<Domain.Models.Provider>>())
                .Returns(callinfo =>
                {
                    var passedEntities = callinfo.ArgAt<IEnumerable<Domain.Models.Provider>>(0);
                    return passedEntities.Count();
                });

            _fileImportDto = new ProviderFileImportDto
            {
                FileDataStream = new MemoryStream()
            };

            _fileReaderResults = new ValidProviderDtoListBuilder(2).Build();

            _fileReader.ValidateAndParseFile(_fileImportDto)
                .Returns(_fileReaderResults);
            var searchProvider = Substitute.For<ISearchProvider>();

            var service = new ProviderService(mapper, _fileReader, _repository, searchResultMapper, searchProvider);

            _result = service.ImportProvider(_fileImportDto).GetAwaiter().GetResult();
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
                .CreateMany(Arg.Any<IEnumerable<Domain.Models.Provider>>());
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Created_Records_Is_Returned()
        {
            Assert.Equal(_fileReaderResults.Count(), _result);
        }
    }
}
