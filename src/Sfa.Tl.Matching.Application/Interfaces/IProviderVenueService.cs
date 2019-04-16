using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderVenueService
    {
        Task<(bool, string)> IsValidPostCode(string postCode);
        Task<int> CreateVenue(ProviderVenueDto dto);
        Task<ProviderVenueDetailViewModel> GetVenueWithQualifications(long ukprn, string postcode);
        Task SetIsProviderEnabledForSearchAsync(int providerVenueId, bool isEnabled);
        Task UpdateVenue(UpdateProviderVenueDto dto);
    }
}