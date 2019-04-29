using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IMessageQueueService
    {
        Task PushProximityDataAsync(GetProximityData getProximityData);
        Task PushProviderQuarterlyRequestMessageAsync(ProviderRequestData providerRequest);
    }
}