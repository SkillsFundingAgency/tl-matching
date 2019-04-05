using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Humanizer;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.Enums;
using Xunit;
using Path = System.IO.Path;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ExcelFileReader
{
    public class When_Validate_And_Parse_File_Is_Called_For_Invalid_File
    {
        private readonly IValidator<EmployerFileImportDto> _dataValidator;
        private readonly IDataParser<EmployerDto> _dataParser;
        private readonly IRepository<FunctionLog> _functionLogRepository;
        private readonly string _errorMessage = $"'{nameof(EmployerFileImportDto.CrmId)}' - {ValidationErrorCode.MissingMandatoryData.Humanize()}";
        public When_Validate_And_Parse_File_Is_Called_For_Invalid_File()
        {
            _dataValidator = Substitute.For<IValidator<EmployerFileImportDto>>();
            _dataValidator
                .ValidateAsync(Arg.Any<EmployerFileImportDto>())
                .Returns(Task.FromResult(new ValidationResult
                {
                    Errors = { new ValidationFailure("CrmId", _errorMessage) }
                }));

            _dataParser = Substitute.For<IDataParser<EmployerDto>>();

            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var excelfileReader = new ExcelFileReader<EmployerFileImportDto, EmployerDto>
            (
                new NullLogger<ExcelFileReader<EmployerFileImportDto, EmployerDto>>(),
                _dataParser,
                _dataValidator,
                _functionLogRepository
            );

            var filePath = Path.Combine(TestHelper.GetTestExecutionDirectory(), @"FileReader\ExcelFileReader\Employer-MissingMandatory.xlsx");
            using (var stream = File.Open(filePath, FileMode.Open))
            {

                excelfileReader.ValidateAndParseFile(new EmployerFileImportDto
                {
                    FileDataStream = stream
                }).GetAwaiter().GetResult();
            }
        }

        [Fact]
        public void Then_Data_Validator_Validate_Is_called_Exactly_Once()
        {
            _dataValidator.Received(1).ValidateAsync(Arg.Is<EmployerFileImportDto>(arg => 
                arg.CompanyName == "Employer-MissingMandatory"));
        }

        [Fact]
        public void Then_FunctionLog_Repository_Create_Many_Is_called_Exactly_Once()
        {
            _functionLogRepository.Received(1).CreateMany(Arg.Is<List<FunctionLog>>(list => 
                list.Count == 1 &&
                list[0].FunctionName == "Employer" &&
                list[0].RowNumber == 1 &&
                list[0].ErrorMessage == _errorMessage));
        }

        [Fact]
        public void Then_Data_Parser_Parse_Is_NOT_Called()
        {
            _dataParser.DidNotReceive().Parse(Arg.Any<EmployerFileImportDto>());
        }
    }
}
