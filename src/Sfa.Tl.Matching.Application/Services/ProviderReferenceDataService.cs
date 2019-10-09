using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Api.Clients.ProviderReference;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderReferenceDataService : IReferenceDataService
    {
        private readonly IProviderReferenceDataClient _providerReferenceDataClient;
        private readonly IBulkInsertRepository<ProviderReferenceStaging> _bulkInsertRepository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;

        private readonly IDateTimeProvider _dateTimeProvider;

        public ProviderReferenceDataService(IProviderReferenceDataClient providerReferenceDataClient,
            IBulkInsertRepository<ProviderReferenceStaging> bulkInsertRepository,
            IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository,
            IDateTimeProvider dateTimeProvider)
        {
            _providerReferenceDataClient = providerReferenceDataClient;
            _bulkInsertRepository = bulkInsertRepository;
            _backgroundProcessHistoryRepository = backgroundProcessHistoryRepository;
            _dateTimeProvider = dateTimeProvider;
        }

        public async Task<int> SynchronizeProviderReferenceAsync(DateTime lastUpdateDate)
        {
            var backgroundProcessHistoryId = await CreateBackgroundProcessHistoryAsync();
            var providerReferenceStagings = await GetProvidersForStagingAsync(lastUpdateDate);
            await _bulkInsertRepository.BulkInsertAsync(providerReferenceStagings);
            await _bulkInsertRepository.MergeFromStagingAsync();

            await UpdateBackgroundProcessHistoryAsync(backgroundProcessHistoryId, providerReferenceStagings.Count);

            return providerReferenceStagings.Count;
        }

        private async Task<int> CreateBackgroundProcessHistoryAsync()
        {
            var backgroundProcessHistoryId = await _backgroundProcessHistoryRepository.CreateAsync(
                new BackgroundProcessHistory
                {
                    ProcessType = BackgroundProcessType.ProviderReferenceData.ToString(),
                    Status = BackgroundProcessHistoryStatus.Processing.ToString(),
                    CreatedBy = "System"
                });

            return backgroundProcessHistoryId;
        }

        private async Task<List<ProviderReferenceStaging>> GetProvidersForStagingAsync(DateTime lastUpdateDate)
        {
            var providers = await _providerReferenceDataClient.GetAllAsync(lastUpdateDate);
            var providerReferenceStagings = providers.Select(p => new ProviderReferenceStaging
            {
                Name = p.ProviderName,
                UkPrn = long.Parse(p.UnitedKingdomProviderReferenceNumber),
                CreatedOn = _dateTimeProvider.UtcNow(),
                CreatedBy = "System"
            }).ToList();

            return providerReferenceStagings;
        }

        private async Task UpdateBackgroundProcessHistoryAsync(int backgroundProcessHistoryId, int providerReferenceCount)
        {
            var backgroundProcessHistory = await _backgroundProcessHistoryRepository.GetSingleOrDefaultAsync(p => p.Id == backgroundProcessHistoryId);
            backgroundProcessHistory.RecordCount = providerReferenceCount;
            backgroundProcessHistory.Status = BackgroundProcessHistoryStatus.Complete.ToString();
            await _backgroundProcessHistoryRepository.UpdateAsync(backgroundProcessHistory);
        }
    }
}