using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IReferralEmailService
    {
        Task SendEmployerReferralEmailAsync(int opportunityId, int backgroundProcessHistoryId, string username);
        Task SendProviderReferralEmailAsync(int opportunityId, int backgroundProcessHistoryId, string username);
    }
}