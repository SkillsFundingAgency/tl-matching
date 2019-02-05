using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class DataImportService<TDto>: IDataImportService<TDto> where TDto : class, new()
    {
        private readonly ILogger _logger;
        private readonly IFileReader<TDto> _fileReader;

        public DataImportService(
            ILogger logger,
            IMapper mapper,
            IFileReader<TDto> fileReader)
        {
            _logger = logger;
            _fileReader = fileReader;
        }

        public IEnumerable<TDto> Import(Stream stream, DataImportType dataImportType)
        {
            var result = _fileReader.ValidateAndParseFile(stream);

            if (!result.Any())
            {
                _logger.LogWarning("No data to add to the database, so quitting");
                return null;
            }

            return result;
        }
    }
}