using System.Threading.Tasks;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IReferralService
    {
        Task ConfirmOpportunitiesAsync(int opportunityId, string username);
    }
}