using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.UnitTests.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Path = System.IO.Path;

namespace Sfa.Tl.Matching.Application.UnitTests.FileReader.ExcelFileReader.Trim
{
    public class ExcelFileReaderTrimDataTestFixture<TImportDto, TDto> where TDto : class, new() where TImportDto : FileImportDto, new()
    {
        public readonly IValidator<TImportDto> DataValidator;
        public readonly IDataParser<TDto> DataParser;
        public readonly IRepository<FunctionLog> FunctionLogRepository;
        public readonly IList<TDto> Results;

        public ExcelFileReaderTrimDataTestFixture()
        {
            DataValidator = Substitute.For<IValidator<TImportDto>>();
            DataValidator
                .ValidateAsync(Arg.Any<TImportDto>())
                .Returns(Task.FromResult(new ValidationResult()));


            DataParser = Substitute.For<IDataParser<TDto>>();
            DataParser.Parse(Arg.Any<TImportDto>()).Returns(info => TestHelper.GetDataParser<TDto>().Parse(info.Arg<TImportDto>()));
            FunctionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var excelfileReader = new ExcelFileReader<TImportDto, TDto>
            (
                new NullLogger<ExcelFileReader<TImportDto, TDto>>(),
                DataParser,
                DataValidator,
                FunctionLogRepository
            );

            var filePath = Path.Combine(TestHelper.GetTestExecutionDirectory(), $"FileReader\\ExcelFileReader\\Trim\\{typeof(TDto).Name.Replace("Dto", string.Empty)}-Trim.xlsx");
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                Results = excelfileReader.ValidateAndParseFile(new TImportDto
                {
                    FileDataStream = stream
                }).GetAwaiter().GetResult();
            }
        }
    }
}