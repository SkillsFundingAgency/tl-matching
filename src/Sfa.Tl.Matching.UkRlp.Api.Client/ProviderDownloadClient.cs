using System;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.UkRlp.Api.Client
{
    public class ProviderDownloadClient : IProviderDownloadClient
    {
        private readonly ProviderQueryPortTypeClient _client;
        private readonly TimeSpan _fiveMinuteTimeSpan = new TimeSpan(0, 5, 0);

        public ProviderDownloadClient()
        {
            _client = new ProviderQueryPortTypeClient();
            _client.Endpoint.Binding.SendTimeout = _fiveMinuteTimeSpan;
            _client.Endpoint.Binding.ReceiveTimeout = _fiveMinuteTimeSpan;
        }

        public async Task<response> RetrieveAll(ProviderQueryStructure query)
        {
            var response = await _client.retrieveAllProvidersAsync(query);

            return response;
        }
    }
}