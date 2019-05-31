using System;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.UkRlp.Api.Client
{
    public class ProviderDownloadClient : IProviderDownloadClient
    {
        private readonly TimeSpan _fiveMinuteTimeSpan = new TimeSpan(0, 5, 0);

        public async Task<response> RetrieveAll(ProviderQueryStructure query)
        {
            var client = new ProviderQueryPortTypeClient();
            client.Endpoint.Binding.SendTimeout = _fiveMinuteTimeSpan;
            client.Endpoint.Binding.ReceiveTimeout = _fiveMinuteTimeSpan;

            var response = await client.retrieveAllProvidersAsync(query);

            return response;
        }
    }
}