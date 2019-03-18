using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IReferralService
    {
        Task SendEmployerEmail(int opportunityId);

        Task SendProviderReferralEmail(int opportunityId);
    }
}
