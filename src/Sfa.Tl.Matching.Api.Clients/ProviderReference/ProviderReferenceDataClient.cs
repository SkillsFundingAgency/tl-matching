using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Api.Clients.Connected_Services.Sfa.Tl.Matching.UkRlp.Api.Client;

namespace Sfa.Tl.Matching.Api.Clients.ProviderReference
{
    public class ProviderReferenceDataClient : IProviderReferenceDataClient
    {
        private readonly ILogger<ProviderReferenceDataClient> _logger;
        private readonly IProviderQueryPortTypeClient _providerQueryPortTypeClient;

        public ProviderReferenceDataClient(ILogger<ProviderReferenceDataClient> logger, IProviderQueryPortTypeClient providerQueryPortTypeClient)
        {
            _logger = logger;
            _providerQueryPortTypeClient = providerQueryPortTypeClient;
        }

        public async Task<List<ProviderRecordStructure>> GetAllAsync(DateTime lastUpdateDate)
        {
            var results = new List<ProviderRecordStructure>();
            var query = GetQuery(lastUpdateDate);

            _logger.LogInformation("Downloading providers from UKRLP service...");

            var response = await RetrieveAllAsync(query);

            _logger.LogInformation($"UKRLP service returned {response.ProviderQueryResponse.MatchingProviderRecords.LongLength} providers");

            results.AddRange(response.ProviderQueryResponse.MatchingProviderRecords);

            return results;
        }

        private static ProviderQueryParam GetQuery(DateTime dtLastUpdate)
        {
            var query = new ProviderQueryParam(new ProviderQueryStructure
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
            });

            return query;
        }

        private async Task<response> RetrieveAllAsync(ProviderQueryParam query)
        {
            try
            {
                var response = await _providerQueryPortTypeClient.retrieveAllProvidersAsync(query);

                return response;
            }
            catch (Exception e)
            {
                if (_providerQueryPortTypeClient.State == CommunicationState.Faulted)
                {
                    _providerQueryPortTypeClient.Abort();
                }

                _logger.LogError($"Error Executing ProviderQueryPortTypeClient Internal Exception ==> {e}");
            }

            return null;
        }
    }
}

namespace Sfa.Tl.Matching.Api.Clients.Connected_Services.Sfa.Tl.Matching.UkRlp.Api.Client
{
    public interface IProviderQueryPortTypeClient : ICommunicationObject, ProviderQueryPortType, IDisposable { }
    public partial class ProviderQueryPortTypeClient : IProviderQueryPortTypeClient { }
}