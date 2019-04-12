using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IReferralService
    {
        Task SendEmployerReferralEmail(int opportunityId);

        Task SendProviderReferralEmail(int opportunityId);
    }
}