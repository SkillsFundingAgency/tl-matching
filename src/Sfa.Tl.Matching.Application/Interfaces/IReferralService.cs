using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IReferralService
    {
        Task SendEmployerEmail(int opportunityId);

        Task SendProviderEmail(int opportunityId);

        Task SendProvisionGapEmail(int opportunityId);
    }
}
