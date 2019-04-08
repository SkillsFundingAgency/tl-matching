using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.Employer;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;
using Path = System.IO.Path;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ExcelFileReader
{
    public class When_Validate_And_Parse_File_Is_Called_For_Valid_File
    {
        private readonly IValidator<EmployerFileImportDto> _dataValidator;
        private readonly IDataParser<EmployerDto> _dataParser;
        private readonly IRepository<FunctionLog> _functionLogRepository;
        private readonly IList<EmployerDto> _result;

        public When_Validate_And_Parse_File_Is_Called_For_Valid_File()
        {
            _dataValidator = Substitute.For<IValidator<EmployerFileImportDto>>();
            _dataValidator
                .ValidateAsync(Arg.Any<EmployerFileImportDto>())
                .Returns(Task.FromResult(new ValidationResult()));

            _dataParser = Substitute.For<IDataParser<EmployerDto>>();
            _dataParser.Parse(Arg.Any<EmployerFileImportDto>()).Returns(info => new EmployerDataParser().Parse(info.Arg<EmployerFileImportDto>()));

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var excelfileReader = new ExcelFileReader<EmployerFileImportDto, EmployerDto>
            (
                new NullLogger<ExcelFileReader<EmployerFileImportDto, EmployerDto>>(),
                _dataParser,
                _dataValidator,
                _functionLogRepository
            );

            var filePath = Path.Combine(TestHelper.GetTestExecutionDirectory(), @"FileReader\ExcelFileReader\Employer-Simple.xlsx");
            using (var stream = File.Open(filePath, FileMode.Open))
            {

                _result = excelfileReader.ValidateAndParseFile(new EmployerFileImportDto
                {
                    FileDataStream = stream
                }).GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Data_Validator_Validate_Is_called_Exactly_Once()
        {
            _dataValidator.Received(1).ValidateAsync(Arg.Is<EmployerFileImportDto>(arg =>
                arg.CompanyName == "Employer-Simple"));
        }

        [Fact]
        public void Then_FunctionLog_Repository_Create_Many_Is_Called_With_Empty_List()
        {
            _functionLogRepository.Received(1).CreateMany(Arg.Is<List<FunctionLog>>(logs => logs.Count == 0));
        }

        [Fact]
        public void Then_Data_Parser_Parse_Is_Called_Exactly_Once()
        {
            _dataParser.Received(1).Parse(Arg.Is<EmployerFileImportDto>(dto =>
                dto.CompanyName == "Employer-Simple"));
        }

        [Fact]
        public void Then_Result_Contains_Exactly_Once_Item_And_Has_Correct_Data()
        {
            _result.Should().NotBeNullOrEmpty();
            
            _result.Count.Should().Be(1);
            
            var dto = _result.ElementAt(0);

            dto.CrmId.Should().Be("9082609f-9cf8-e811-80e0-000d3a214f60");
            dto.CompanyName.Should().Be("Employer-Simple");
            dto.AlsoKnownAs.Should().Be("Also Known As");
            dto.CompanyNameSearch.Should().Be("EmployerSimpleAlsoKnownAs");
            dto.Aupa.Should().Be("Aware");
            dto.CompanyType.Should().Be("Employer");
            dto.PrimaryContact.Should().Be("Primary Contact");
            dto.Phone.Should().Be("7777744465");
            dto.Email.Should().Be("email@address.com");
            dto.Postcode.Should().Be("S1 1AA");
            dto.Owner.Should().Be("Owner");
        }
    }
}
