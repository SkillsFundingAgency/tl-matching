using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.Extensions.Logging;

namespace Sfa.Tl.Matching.UkRlp.Api.Client
{
    public class ProviderDownload : IProviderDownload
    {
        private readonly ILogger<ProviderDownload> _logger;
        private readonly IProviderDownloadClient _client;

        public ProviderDownload(ILogger<ProviderDownload> logger, IProviderDownloadClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task<List<ProviderRecordStructure>> GetAll(DateTime lastUpdateDate)
        {
            var results = new List<ProviderRecordStructure>();
            var query = GetQuery(lastUpdateDate);

            _logger.LogInformation("Downloading providers from UKRLP service...");
            var response = await _client.RetrieveAll(query);
            _logger.LogInformation($"UKRLP service returned {response.ProviderQueryResponse.MatchingProviderRecords.LongLength} providers");

            results.AddRange(response.ProviderQueryResponse.MatchingProviderRecords);

            return results;
        }

        private static ProviderQueryStructure GetQuery(DateTime dtLastUpdate)
        {
            var query = new ProviderQueryStructure
            {
                QueryId = "1",
                SelectionCriteria = new SelectionCriteriaStructure
                {
                    StakeholderId = "1",
                    ProviderUpdatedSince = dtLastUpdate,
                    ProviderUpdatedSinceSpecified = true,
                    ApprovedProvidersOnly = YesNoType.No,
                    ApprovedProvidersOnlySpecified = true,
                    CriteriaCondition = QueryCriteriaConditionType.OR,
                    CriteriaConditionSpecified = true,
                    ProviderStatus = UkRlpRecordStatus.Active.Humanize()
                }
            };

            return query;
        }
    }
}