using System.Threading.Tasks;

namespace Sfa.Tl.Matching.UkRlp.Api.Client
{
    public interface IProviderDownloadClient
    {
        Task<response> RetrieveAll(ProviderQueryStructure query);
    }
}