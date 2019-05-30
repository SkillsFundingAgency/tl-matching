using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.UkRlp.Api.Client
{
    public interface IProviderDownload
    {
        Task<List<ProviderRecordStructure>> GetAll(DateTime lastUpdateDate);
    }
}