using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Data.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Enums;
using Sfa.Tl.Matching.Api.Clients.ProviderReference;
using Sfa.Tl.Matching.Models.Configuration;

namespace Sfa.Tl.Matching.Application.Services
{
    public class ProviderReferenceDataService : IReferenceDataService
    {
        private readonly IProviderReferenceDataClient _providerReferenceDataClient;
        private readonly IRepository<ProviderReferenceStaging> _repository;
        private readonly IRepository<BackgroundProcessHistory> _backgroundProcessHistoryRepository;

        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly MatchingConfiguration _matchingConfiguration;

        public ProviderReferenceDataService(IProviderReferenceDataClient providerReferenceDataClient,
            IRepository<ProviderReferenceStaging> repository,
            IRepository<BackgroundProcessHistory> backgroundProcessHistoryRepository,
            IDateTimeProvider dateTimeProvider,
            MatchingConfiguration matchingConfiguration)
        {
            _providerReferenceDataClient = providerReferenceDataClient;
            _repository = repository;
            _backgroundProcessHistoryRepository = backgroundProcessHistoryRepository;
            _dateTimeProvider = dateTimeProvider;
            _matchingConfiguration = matchingConfiguration;
        }

        public async Task<int> SynchronizeProviderReference(DateTime lastUpdateDate)
        {
            var backgroundProcessHistoryId = await CreateBackgroundProcessHistory();
            var providerReferenceStagings = await GetProvidersForStaging(lastUpdateDate);
            await _repository.BulkInsert(providerReferenceStagings, _matchingConfiguration.SqlConnectionString);
            await _repository.MergeFromStaging(_matchingConfiguration.SqlConnectionString);

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

        private async Task<List<ProviderReferenceStaging>> GetProvidersForStaging(DateTime lastUpdateDate)
        {
            var providers = await _providerReferenceDataClient.GetAll(lastUpdateDate);
            var providerReferenceStagings = providers.Select(p => new ProviderReferenceStaging
            {
                Name = p.ProviderName,
                UkPrn = long.Parse(p.UnitedKingdomProviderReferenceNumber),
                CreatedOn = _dateTimeProvider.UtcNow(),
                CreatedBy = "System"
            }).ToList();

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