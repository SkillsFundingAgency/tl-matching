using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using System.Linq;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderVenueQualificationFileImportService : IProviderVenueQualificationFileImportService
    {
        private readonly ILogger<IProviderVenueQualificationFileImportService> _logger;
        private readonly IProviderVenueQualificationReader _fileReader;
        private readonly IProviderVenueQualificationService _providerVenueQualificationService;

        public ProviderVenueQualificationFileImportService(ILogger<IProviderVenueQualificationFileImportService> logger,
            IProviderVenueQualificationReader fileReader,
            IProviderVenueQualificationService providerVenueQualificationService)
        {
            _logger = logger;
            _fileReader = fileReader;
            _providerVenueQualificationService = providerVenueQualificationService;
        }

        public async Task<int> BulkImportAsync(ProviderVenueQualificationFileImportDto fileImportDto)
        {
            _logger.LogInformation($"Processing {nameof(ProviderVenueQualificationFileImportDto)}.");

            var dataDtos = _fileReader.ReadData(fileImportDto);

            if (dataDtos == null || !dataDtos.ProviderVenueQualifications.Any())
            {
                _logger.LogInformation("No Data Imported.");
                return 0;
            }

            var results = 
                (await _providerVenueQualificationService.Update(dataDtos.ProviderVenueQualifications))
                .ToList();

            // Log errors in data updates
            foreach (var result in results.Where(result => result.HasErrors))
            {
                _logger.LogError(result.Message);
            }
            
            var updatedCount = results.Count(x => x.HasErrors == false);

            _logger.LogInformation($"{updatedCount} out of {dataDtos.ProviderVenueQualifications.Count} Providers data successfully updated.");

            return updatedCount;
        }
    }
}