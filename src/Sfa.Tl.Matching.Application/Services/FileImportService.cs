using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.EqualityComparer;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class FileImportService<TImportDto, TDto, TEntity> : IFileImportService<TImportDto>
        where TImportDto : FileImportDto
        where TDto : class, new()
        where TEntity : BaseEntity, new()
    {
        private readonly ILogger<FileImportService<TImportDto, TDto, TEntity>> _logger;
        private readonly IMapper _mapper;
        private readonly IFileReader<TImportDto, TDto> _fileReader;
        private readonly IBulkInsertRepository<TEntity> _repository;
        private readonly IDataProcessor<TEntity> _dataProcessor;

        public FileImportService(ILogger<FileImportService<TImportDto, TDto, TEntity>> logger,
            IMapper mapper,
            IFileReader<TImportDto, TDto> fileReader,
            IBulkInsertRepository<TEntity> repository,
            IDataProcessor<TEntity> dataProcessor)
        {
            _logger = logger;
            _mapper = mapper;
            _fileReader = fileReader;
            _repository = repository;
            _dataProcessor = dataProcessor;
        }

        public async Task<int> BulkImport(TImportDto fileImportDto)
        {
            _logger.LogInformation($"Processing { nameof(TImportDto) }.");

            var dataDtos = await _fileReader.ValidateAndParseFile(fileImportDto);

            if (dataDtos == null || !dataDtos.Any())
            {
                _logger.LogInformation("No Data Imported.");

                return 0;
            }

            var comparer = GetEqualityComparer();

            var entities = _mapper.Map<IList<TEntity>>(dataDtos).Distinct(comparer).ToList();

            _dataProcessor.PreProcessingHandler(entities);

            _logger.LogInformation($"Saving { entities.Count } { nameof(TEntity) }.");

            await _repository.BulkInsert(entities);

            var numberOfRecordsAffected = await _repository.MergeFromStaging();

            _dataProcessor.PostProcessingHandler(entities);

            return numberOfRecordsAffected;
        }

        private static IEqualityComparer<TEntity> GetEqualityComparer()
        {
            var comparerType = typeof(EmployerStagingEqualityComparer).Assembly.GetTypes()
                .Single(comparer => comparer.GetInterfaces().Contains(typeof(IEqualityComparer<TEntity>)));
            return (IEqualityComparer<TEntity>)Activator.CreateInstance(comparerType);
        }
    }
}