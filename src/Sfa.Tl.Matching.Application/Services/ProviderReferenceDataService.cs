using System;
using System.Collections.Generic;
using System.Linq;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.UkRlp.Api.Client;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderReferenceDataService : IReferenceDataService
    {
        private readonly IProviderDownload _providerDownload;
        private readonly IRepository<ProviderReferenceStaging> _repository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;

        private readonly IDateTimeProvider _dateTimeProvider;

        public ProviderReferenceDataService(IProviderDownload providerDownload,
            IRepository<ProviderReferenceStaging> repository,
            IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _providerDownload = providerDownload;
            _repository = repository;
            _backgroundProcessHistoryRepository = backgroundProcessHistoryRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> SynchronizeProviderReference()
        {
            var backgroundProcessHistoryId = await CreateBackgroundProcessHistory();

            var providerReferenceStagings = await GetProvidersForStaging();
            await _repository.BulkInsert(providerReferenceStagings);
            await _repository.MergeFromStaging();

            await UpdateBackgroundProcessHistory(backgroundProcessHistoryId, providerReferenceStagings.Count);

            return providerReferenceStagings.Count;
        }

        private async Task<int> CreateBackgroundProcessHistory()
        {
            var backgroundProcessHistoryId = await _backgroundProcessHistoryRepository.Create(
                new BackgroundProcessHistory
                {
                    ProcessType = BackgroundProcessType.ProviderReferenceData.ToString(),
                    Status = BackgroundProcessHistoryStatus.Processing.ToString(),
                    CreatedBy = "System"
                });
            return backgroundProcessHistoryId;
        }

        private async Task<List<ProviderReferenceStaging>> GetProvidersForStaging()
        {
            var providers = await _providerDownload.GetAll(DateTime.MinValue);
            var providerReferenceStagings = providers.Select(p => new ProviderReferenceStaging
            {
                Name = p.ProviderName,
                UkPrn = long.Parse(p.UnitedKingdomProviderReferenceNumber),
                CreatedOn = _dateTimeProvider.UtcNow(),
                CreatedBy = "System"
            }).ToList();
            return providerReferenceStagings;
        }

        private async Task<List<ProviderReferenceStaging>> GetProvidersForStagingTest()
        {
            var providerReferenceStagings = new List<ProviderReferenceStaging>
            {
                new ProviderReferenceStaging
                {
                    Name = "THE CAMDEN SCHOOL FOR GIRLS12",
                    UkPrn = 10000001,
                    CreatedOn = _dateTimeProvider.UtcNow(),
                    CreatedBy = "System"
                }
            };

            return providerReferenceStagings;
        }

        private async Task UpdateBackgroundProcessHistory(int backgroundProcessHistoryId, int providerReferenceCount)
        {
            var backgroundProcessHistory = await _backgroundProcessHistoryRepository.GetSingleOrDefault(p => p.Id == backgroundProcessHistoryId);
            backgroundProcessHistory.RecordCount = providerReferenceCount;
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Complete.ToString();
            await _backgroundProcessHistoryRepository.Update(backgroundProcessHistory);
        }
    }
}