using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderVenueService
    {
        Task<(bool, string)> IsValidPostCodeAsync(string postCode);
        Task<int> CreateVenueAsync(ProviderVenueDto dto);
        Task<ProviderVenueDetailViewModel> GetVenueWithQualificationsAsync(long ukprn, string postcode);
        Task UpdateVenueAsync(UpdateProviderVenueDto dto);
        Task<bool> HaveUniqueVenueAsync(long ukPrn, string postCode);
        Task SetIsProviderVenueEnabledForSearchAsync(int providerVenueId, bool isEnabled);
    }
}