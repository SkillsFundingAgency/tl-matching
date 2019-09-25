using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderQuarterlyUpdateEmailService
    {
        Task RequestProviderQuarterlyUpdateAsync(string userName);
        Task<int> SendProviderQuarterlyUpdateEmailsAsync(int backgroundProcessHistoryId, string userName);
    }
}