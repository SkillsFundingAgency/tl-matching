using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly IProviderVenueQualificationReader _fileReader;

        public ProviderVenueQualificationFileImportService(ILogger<IProviderVenueQualificationFileImportService> logger,
            IProviderVenueQualificationReader fileReader)
        {
            _logger = logger;
            _fileReader = fileReader;
        }

        public async Task<int> BulkImportAsync(ProviderVenueQualificationFileImportDto fileImportDto)
        {
            _logger.LogInformation($"Processing {typeof(ProviderVenueQualificationFileImportDto).Name}.");

            var dataDtos = _fileReader.ReadData(fileImportDto.FileDataStream);

            if (dataDtos == null && !dataDtos.Qualifications.Any())
            {
                _logger.LogInformation("No Data Imported.");

                return 0;
            }

            return await Task.FromResult(dataDtos.Qualifications.Count());
        }
    }
}