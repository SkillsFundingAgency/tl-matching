using System.IO;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Application.UnitTests.Extensions;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Path = System.IO.Path;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.FileImportService.DuplicateRows
{
    public class ExcelFileImportServiceDuplicateRowsTestFixture<TImportDto, TDto, TEntity> 
        where TImportDto : FileImportDto, new()
        where TDto : class, new()
        where TEntity : BaseEntity, new()

    {
        internal readonly IValidator<TImportDto> DataValidator;
        internal readonly IDataParser<TDto> DataParser;
        internal readonly IRepository<FunctionLog> FunctionLogRepository;
        internal readonly IRepository<TEntity> Repository;
        internal readonly FileImportService<TImportDto, TDto, TEntity> FileImportService;

        public ExcelFileImportServiceDuplicateRowsTestFixture()
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

            Repository = Substitute.For<IRepository<TEntity>>();

            FileImportService = new FileImportService<TImportDto, TDto, TEntity>
            (
                new NullLogger<FileImportService<TImportDto, TDto, TEntity>>(),
                TestHelper.GetMapper(),
                excelfileReader,
                Repository,
                Substitute.For<IDataProcessor<TEntity>>()
            );

            var filePath = Path.Combine(TestHelper.GetTestExecutionDirectory(), $"Services\\FileImportService\\DuplicateRows\\{typeof(TEntity).Name}-DuplicateRows.xlsx");
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                FileImportService.Import(new TImportDto
                {
                    FileDataStream = stream
                }).GetAwaiter().GetResult();
            }
        }
    }
}