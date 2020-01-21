using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;
using Sfa.Tl.Matching.Application.FileReader;
using Sfa.Tl.Matching.Application.FileReader.LearningAimReferenceStaging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Application.Mappers;
using Sfa.Tl.Matching.Application.Services;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Tests.Common;

namespace Sfa.Tl.Matching.Application.UnitTests.Services.FileImportService.DuplicateRows
{
    public class CsvFileImportServiceDuplicateRowsTestFixture<TImportDto, TDto, TEntity> 
        where TImportDto : FileImportDto, new()
        where TDto : class, new()
        where TEntity : BaseEntity, new()
    {
        internal readonly IValidator<TImportDto> DataValidator;
        internal readonly IDataParser<TDto> DataParser;
        internal readonly IRepository<FunctionLog> FunctionLogRepository;
        internal readonly IBulkInsertRepository<TEntity> Repository;
        internal readonly FileImportService<TImportDto, TDto, TEntity> FileImportService;

        public CsvFileImportServiceDuplicateRowsTestFixture()
        {
            DataValidator = Substitute.For<IValidator<TImportDto>>();
            DataValidator
                .ValidateAsync(Arg.Any<TImportDto>())
                .Returns(Task.FromResult(new ValidationResult()));

            DataParser = Substitute.For<IDataParser<TDto>>();

            DataParser.Parse(Arg.Any<TImportDto>())
                .Returns(info => 
                    GetDataParser().Parse(info.Arg<TImportDto>()));

            FunctionLogRepository = Substitute.For<IRepository<FunctionLog>>();

            var csvfileReader = new CsvFileReader<TImportDto, TDto>
            (
                new NullLogger<CsvFileReader<TImportDto, TDto>>(),
                DataParser,
                DataValidator,
                FunctionLogRepository
            );

            Repository = Substitute.For<IBulkInsertRepository<TEntity>>();

            FileImportService = new FileImportService<TImportDto, TDto, TEntity>
            (
                new NullLogger<FileImportService<TImportDto, TDto, TEntity>>(),
                GetMapper(),
                csvfileReader,
                Repository,
                Substitute.For<IDataProcessor<TEntity>>()
            );

            var filePath = System.IO.Path.Combine(TestConfiguration.GetTestExecutionDirectory(), $"Services\\FileImportService\\DuplicateRows\\{typeof(TEntity).Name}-DuplicateRows.csv");
            using (var stream = File.Open(filePath, FileMode.Open))
            {
                FileImportService.BulkImportAsync(new TImportDto
                {
                    FileDataStream = stream
                }).GetAwaiter().GetResult();
            }
        }

        private static IMapper GetMapper()
        {
            var config = new MapperConfiguration(c => 
                c.AddMaps(typeof(EmployerMapper).Assembly));
            return new Mapper(config);
        }

        private static IDataParser<TDto> GetDataParser()
        {
            return (IDataParser<TDto>)Activator.CreateInstance(
                typeof(LearningAimReferenceStagingDataParser).Assembly.GetTypes()
                .First(t => typeof(IDataParser<TDto>).IsAssignableFrom(t)));
        }
    }
}