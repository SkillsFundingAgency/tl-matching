using System.Threading.Tasks;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface IProviderVenueRepository : IRepository<ProviderVenue>
    {
        Task<ProviderVenueDetailViewModel> GetVenueWithQualificationsAsync(int id);
    }
}