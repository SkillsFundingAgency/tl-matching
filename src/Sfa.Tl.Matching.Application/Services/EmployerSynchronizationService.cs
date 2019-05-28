using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class EmployerSynchronizationService : IEmployerSynchronizationService
    {
        private readonly IFileImportService<EmployerStagingFileImportDto> _fileImportService;
        private readonly IRepository<EmployerStaging> _employerStagingRepository;

        public EmployerSynchronizationService(IFileImportService<EmployerStagingFileImportDto> fileImportService, IRepository<EmployerStaging> employerStagingRepository)
        {
            _fileImportService = fileImportService;
            _employerStagingRepository = employerStagingRepository;
        }

        public async Task<int> SynchronizeEmployers(EmployerStagingFileImportDto employerStagingFileImportDto)
        {
            var createdRecords = await _fileImportService.BulkImport(employerStagingFileImportDto);

            await _employerStagingRepository.MergeFromStaging();

            return createdRecords;
        }
    }
}