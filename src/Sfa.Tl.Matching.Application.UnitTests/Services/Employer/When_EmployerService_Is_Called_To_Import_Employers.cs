using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using NSubstitute;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Services.Employer.Builders;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.Employer
{
    public class When_EmployerService_Is_Called_To_Import_Employers
    {
        private readonly EmployerFileImportDto _fileImportDto;
        private readonly IEnumerable<EmployerDto> _fileReaderResults;
        private readonly IFileReader<EmployerFileImportDto, EmployerDto> _fileReader;
        private readonly IRepository<Domain.Models.Employer> _repository;
        private readonly int _result;

        public When_EmployerService_Is_Called_To_Import_Employers()
        {
            var config = new MapperConfiguration(c => c.AddProfile<EmployerMapper>());
            var mapper = new Mapper(config);
            var logger = Substitute.For<ILogger<FileImportService<EmployerFileImportDto, EmployerDto, Domain.Models.Employer>>>();
            _fileReader = Substitute.For<IFileReader<EmployerFileImportDto, EmployerDto>>();
            _repository = Substitute.For<IRepository<Domain.Models.Employer>>();

            _repository
                .CreateMany(Arg.Any<IList<Domain.Models.Employer>>())
                .Returns(callinfo =>
                {
                    var passedEntities = callinfo.ArgAt<IEnumerable<Domain.Models.Employer>>(0);
                    return passedEntities.Count();
                });

            _fileImportDto = new EmployerFileImportDto
            {
                FileDataStream = new MemoryStream()
            };

            _fileReaderResults = new ValidEmployerDtoListBuilder(2).Build();

            _fileReader.ValidateAndParseFile(_fileImportDto)
                .Returns(_fileReaderResults);

            var service = new FileImportService<EmployerFileImportDto, EmployerDto, Domain.Models.Employer>(logger, mapper, _fileReader, _repository);

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
                .CreateMany(Arg.Any<IList<Domain.Models.Employer>>());
        }

        [Fact]
        public void Then_The_Expected_Number_Of_Created_Records_Is_Returned()
        {
            Assert.Equal(_fileReaderResults.Count(), _result);
        }
    }
}
