using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Xunit;
using Path = System.IO.Path;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ExcelFileReader
{
    public class When_ExcelFileReader_Validate_And_Parse_File_Is_Called_For_Valid_File
    {
        private readonly IValidator<EmployerFileImportDto> _employerDataValidator;
        private readonly IDataParser<EmployerDto> _employerDataParser;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public When_ExcelFileReader_Validate_And_Parse_File_Is_Called_For_Valid_File()
        {
            _employerDataValidator = Substitute.For<IValidator<EmployerFileImportDto>>();
            _employerDataValidator
                .ValidateAsync(Arg.Any<EmployerFileImportDto>())
                .Returns(Task.FromResult(new ValidationResult()));

            _employerDataParser = Substitute.For<IDataParser<EmployerDto>>();

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var excelfileReader = new ExcelFileReader<EmployerFileImportDto, EmployerDto>
            (
                new NullLogger<ExcelFileReader<EmployerFileImportDto, EmployerDto>>(),
                _employerDataParser,
                _employerDataValidator,
                _functionLogRepository
            );

            var filePath = Path.Combine(GetTestExecutionDirectory(), @"FileReader\ExcelFileReader\Employer-Simple.xlsx");
            using (var stream = File.Open(filePath, FileMode.Open))
            {

                excelfileReader.ValidateAndParseFile(new EmployerFileImportDto
                {
                    FileDataStream = stream
                }).GetAwaiter().GetResult();
            }
        }

        public static string GetTestExecutionDirectory()
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            return Path.GetDirectoryName(codeBasePath);
        }

        [Fact]
        public void Then_Data_Validator_Validate_Is_called_Exactly_Once()
        {
            _employerDataValidator.Received(1).ValidateAsync(Arg.Is<EmployerFileImportDto>(arg =>
                arg.CompanyName == "Employer-Simple"));
        }

        [Fact]
        public void Then_FunctionLog_Repository_Create_Many_Is_NOT_Called()
        {
            _functionLogRepository.DidNotReceive().CreateMany(Arg.Any<List<FunctionLog>>());
        }

        [Fact]
        public void Then_Data_Parser_Parse_Is_Called_Exactly_Once()
        {
            _employerDataParser.Received(1).Parse(Arg.Is<EmployerFileImportDto>(dto => 
                dto.CompanyName == "Employer-Simple"));
        }
    }
}
