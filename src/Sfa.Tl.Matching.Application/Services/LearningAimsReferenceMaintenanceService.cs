using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Services
{
    public class LearningAimsReferenceSynchronizationService : ILearningAimsReferenceSynchronizationService
    {
        private readonly IFileImportService<LearningAimsReferenceStagingFileImportDto> _fileImportService;
        private readonly IRepository<LearningAimsReferenceStaging> _learningAimsReferenceStagingRepository;

        public LearningAimsReferenceSynchronizationService(IFileImportService<LearningAimsReferenceStagingFileImportDto> fileImportService, IRepository<LearningAimsReferenceStaging> learningAimsReferenceStagingRepository)
        {
            _fileImportService = fileImportService;
            _learningAimsReferenceStagingRepository = learningAimsReferenceStagingRepository;
        }

        public async Task<int> SynchronizeLearningAimsReferences(LearningAimsReferenceStagingFileImportDto learningAimsReferenceStagingFileImportDto)
        {
            var createdRecords = await _fileImportService.BulkImport(learningAimsReferenceStagingFileImportDto);

            await _learningAimsReferenceStagingRepository.MergeFromStaging();

            return createdRecords;
        }
    }
}
