using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class FileImportService<TImportDto, TDto, TEntity> : IFileImportService<TImportDto, TDto, TEntity>
        where TImportDto : FileImportDto
        where TDto : class, new()
        where TEntity : BaseEntity, new()
    {
        private readonly ILogger<FileImportService<TImportDto, TDto, TEntity>> _logger;
        private readonly IMapper _mapper;
        private readonly IFileReader<TImportDto, TDto> _fileReader;
        private readonly IRepository<TEntity> _repository;
        private readonly IDataProcessor<TEntity> _dataProcessor;

        public FileImportService(
            ILogger<FileImportService<TImportDto, TDto, TEntity>> logger,
            IMapper mapper,
            IFileReader<TImportDto, TDto> fileReader,
            IRepository<TEntity> repository,
            IDataProcessor<TEntity> dataProcessor)
        {
            _logger = logger;
            _mapper = mapper;
            _fileReader = fileReader;
            _repository = repository;
            _dataProcessor = dataProcessor;
        }

        public async Task<int> Import(TImportDto fileImportDto)
        {
            _logger.LogInformation($"Processing { nameof(TImportDto) }.");

            var import = await _fileReader.ValidateAndParseFile(fileImportDto);

            if (import == null || !import.Any())
            {
                _logger.LogInformation("No Data Imported.");

                return 0;
            }

            var entities = _mapper.Map<IList<TEntity>>(import);
                
            _dataProcessor.PreProcessingHandler(entities);

            _logger.LogInformation($"Saving { entities.Count } { nameof(TEntity) }.");
                
            var result = await _repository.CreateMany(entities);

            _dataProcessor.PostProcessingHandler(entities);

            return result;
        }
    }
}