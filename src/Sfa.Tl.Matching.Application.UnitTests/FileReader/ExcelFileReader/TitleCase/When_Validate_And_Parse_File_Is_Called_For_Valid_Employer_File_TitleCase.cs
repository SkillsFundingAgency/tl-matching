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
using Sfa.Tl.Matching.Application.UnitTests.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;
using Path = System.IO.Path;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ExcelFileReader.TitleCase
{
    public class When_Validate_And_Parse_File_Is_Called_For_Valid_Employer_File_TitleCase
    {
        private readonly IValidator<EmployerStagingFileImportDto> _dataValidator;
        private readonly IDataParser<EmployerStagingDto> _dataParser;
        private readonly IRepository<FunctionLog> _functionLogRepository;
        private readonly IList<EmployerStagingDto> _results;

        public When_Validate_And_Parse_File_Is_Called_For_Valid_Employer_File_TitleCase()
        {
            _dataValidator = Substitute.For<IValidator<EmployerStagingFileImportDto>>();
            _dataValidator
                .ValidateAsync(Arg.Any<EmployerStagingFileImportDto>())
                .Returns(Task.FromResult(new ValidationResult()));

            _dataParser = Substitute.For<IDataParser<EmployerStagingDto>>();
            _dataParser.Parse(Arg.Any<EmployerStagingFileImportDto>()).Returns(info => new EmployerStagingDataParser().Parse(info.Arg<EmployerStagingFileImportDto>()));
            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var excelfileReader = new ExcelFileReader<EmployerStagingFileImportDto, EmployerStagingDto>
            (
                new NullLogger<ExcelFileReader<EmployerStagingFileImportDto, EmployerStagingDto>>(),
                _dataParser,
                _dataValidator,
                _functionLogRepository
            );

            var filePath = Path.Combine(TestHelper.GetTestExecutionDirectory(), @"FileReader\ExcelFileReader\TitleCase\Employer-TitleCase.xlsx");
            using (var stream = File.Open(filePath, FileMode.Open))
            {

                _results = excelfileReader.ValidateAndParseFile(new EmployerStagingFileImportDto
                {
                    FileDataStream = stream
                }).GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Data_Validator_Validate_Is_called_Exactly_Once_And_Leading_And_Trailing_Speaces_are_Trimmed()
        {
            _dataValidator.Received(1).ValidateAsync(Arg.Is<EmployerStagingFileImportDto>(arg =>
                arg.CompanyName == "HARDIK DESAI LTD" && arg.AlsoKnownAs == "also known as"));
        }

        [Fact]
        public void Then_FunctionLog_Repository_Create_Many_Is_Called_With_Empty_List()
        {
            _functionLogRepository.Received(1).CreateMany(Arg.Is<List<FunctionLog>>(logs => logs.Count == 0));
        }

        [Fact]
        public void Then_Data_Parser_Parse_Is_Called_Exactly_Once_And_Company_Name_And_Also_KnownAs_Fields_are_Converted_To_TitleCase()
        {
            _dataParser.Received(1).Parse(Arg.Any<EmployerStagingFileImportDto>());
        }

        [Fact]
        public void Then_Returned_List_Has_One_Item_And_Company_Name_And_Also_KnownAs_Fields_are_Converted_To_TitleCase()
        {
            _results.Should().NotBeNullOrEmpty();

            _results.Count.Should().Be(1);

            var dto = _results.ElementAt(0);

            dto.CompanyName.Should().Be("Hardik Desai Ltd");
            dto.AlsoKnownAs.Should().Be("Also Known As");
        }
    }
}