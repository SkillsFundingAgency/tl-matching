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
        public ILogger _logger;
        private readonly IMapper _mapper;
        public IFileReader<TImportDto, TDto> _fileReader;
        private readonly IRepository<TEntity> _repository;

        public FileImportService(
            IMapper mapper,
            IFileReader<TImportDto, TDto> fileReader,
            IRepository<TEntity> repository)
        {
            _mapper = mapper;
            _fileReader = fileReader;
            _repository = repository;
        }

        public async Task<int> Import(TImportDto fileImportDto)
        {
            _logger.LogInformation($"Processing { nameof(TImportDto) }.");

            var import = _fileReader.ValidateAndParseFile(fileImportDto);

            if (import != null && import.Any())
            {
                var providerQualifications = _mapper.Map<IList<TEntity>>(import);

                _logger.LogInformation($"Saving { providerQualifications.Count } { nameof(TEntity) }.");
                return await _repository.CreateMany(providerQualifications);
            }

            _logger.LogInformation("No Data Imported.");
            return 0;
        }
    }
}