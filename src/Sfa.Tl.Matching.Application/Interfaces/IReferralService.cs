using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IReferralService
    {
        Task ConfirmOpportunities(int opportunityId, string username);
    }
}