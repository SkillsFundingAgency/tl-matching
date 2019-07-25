using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IReferralEmailService
    {
        Task SendEmployerReferralEmailAsync(int opportunityId, IEnumerable<int> itemIds, int backgroundProcessHistoryId, string username);
        Task SendProviderReferralEmailAsync(int opportunityId, IEnumerable<int> itemIds, int backgroundProcessHistoryId, string username);
    }
}