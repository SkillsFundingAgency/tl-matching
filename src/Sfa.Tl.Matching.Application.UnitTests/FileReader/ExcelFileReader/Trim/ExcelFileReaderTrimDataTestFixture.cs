using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
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
using Path = System.IO.Path;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ExcelFileReader.Trim
{
    public class ExcelFileReaderTrimDataTestFixture<TImportDto, TDto> where TDto : class, new() where TImportDto : FileImportDto, new()
    {
        public readonly IValidator<TImportDto> _dataValidator;
        public readonly IDataParser<TDto> _dataParser;
        public readonly IRepository<FunctionLog> _functionLogRepository;
        public readonly IList<TDto> _results;

        public ExcelFileReaderTrimDataTestFixture()
        {
            _dataValidator = Substitute.For<IValidator<TImportDto>>();
            _dataValidator
                .ValidateAsync(Arg.Any<TImportDto>())
                .Returns(Task.FromResult(new ValidationResult()));


            _dataParser = Substitute.For<IDataParser<TDto>>();
            _dataParser.Parse(Arg.Any<TImportDto>()).Returns(info => TestHelper.GetDataParser<TDto>().Parse(info.Arg<TImportDto>()));
            _functionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var excelfileReader = new ExcelFileReader<TImportDto, TDto>
            (
                new NullLogger<ExcelFileReader<TImportDto, TDto>>(),
                _dataParser,
                _dataValidator,
                _functionLogRepository
            );

            var filePath = Path.Combine(TestHelper.GetTestExecutionDirectory(), $"FileReader\\ExcelFileReader\\Trim\\{typeof(TDto).Name.Replace("Dto", string.Empty)}-Trim.xlsx");
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                _results = excelfileReader.ValidateAndParseFile(new TImportDto
                {
                    FileDataStream = stream
                }).GetAwaiter().GetResult();
            }
        }
    }
}