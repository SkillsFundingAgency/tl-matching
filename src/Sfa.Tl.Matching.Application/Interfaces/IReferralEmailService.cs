using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IReferralEmailService
    {
        Task SendEmployerReferralEmailAsync(int opportunityId, IList<int> itemIds, int backgroundProcessHistoryId, string username);
        Task SendProviderReferralEmailAsync(int opportunityId, IList<int> itemIds, int backgroundProcessHistoryId, string username);
    }
}