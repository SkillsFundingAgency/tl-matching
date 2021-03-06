﻿using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderVenueQualificationFileImportService : IProviderVenueQualificationFileImportService
    {
        private readonly ILogger<IProviderVenueQualificationFileImportService> _logger;
        private readonly IProviderVenueQualificationReader _fileReader;
        private readonly IProviderVenueQualificationService _providerVenueQualificationService;
        private readonly IRepository<FunctionLog> _functionLogRepository;

        public ProviderVenueQualificationFileImportService(ILogger<IProviderVenueQualificationFileImportService> logger,
            IProviderVenueQualificationReader fileReader,
            IProviderVenueQualificationService providerVenueQualificationService,
            IRepository<FunctionLog> functionLogRepository)
        {
            _logger = logger;
            _fileReader = fileReader;
            _providerVenueQualificationService = providerVenueQualificationService;
            _functionLogRepository = functionLogRepository;
        }

        public async Task<int> BulkImportAsync(ProviderVenueQualificationFileImportDto fileImportDto)
        {
            _logger.LogInformation($"Processing {nameof(ProviderVenueQualificationFileImportDto)}.");

            var readResultDto = _fileReader.ReadData(fileImportDto);

            if (readResultDto?.ProviderVenueQualifications == null || !readResultDto.ProviderVenueQualifications.Any())
            {
                _logger.LogInformation("No Data Imported.");
                return 0;
            }

            var results = 
                (await _providerVenueQualificationService.UpdateAsync(readResultDto.ProviderVenueQualifications))
                .ToList();

            // Log errors in data updates
            foreach (var result in results.Where(result => result.HasErrors))
            {
                _logger.LogError(result.Message);
                await _functionLogRepository.CreateAsync(new FunctionLog
                {
                    ErrorMessage = result.Message,
                    FunctionName = nameof(ProviderVenueQualificationFileImportService),
                    RowNumber = -1
                });
            }
            
            var updatedCount = results.Count(x => x.HasErrors == false);

            _logger.LogInformation($"{updatedCount} out of {readResultDto.ProviderVenueQualifications.Count} Providers data successfully updated.");

            return updatedCount;
        }
    }
}