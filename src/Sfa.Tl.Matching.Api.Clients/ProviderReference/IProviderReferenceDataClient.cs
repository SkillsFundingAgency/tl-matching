using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Api.Clients.Connected_Services.Sfa.Tl.Matching.UkRlp.Api.Client;

namespace Sfa.Tl.Matching.Api.Clients.ProviderReference
{
    public interface IProviderReferenceDataClient
    {
        Task<List<ProviderRecordStructure>> GetAllAsync(DateTime lastUpdateDate);
    }
}