using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderVenueService
    {
        Task<(bool, string)> IsValidPostcodeAsync(string postcode);
        Task<int> CreateVenueAsync(AddProviderVenueViewModel viewModel);
        Task<ProviderVenueDetailViewModel> GetVenueWithQualificationsAsync(int providerVenueId);
        Task UpdateVenueAsync(ProviderVenueDetailViewModel viewModel);
        Task UpdateVenueAsync(RemoveProviderVenueViewModel viewModel);
        Task<ProviderVenueDetailViewModel> GetVenue(int providerId, string postcode);
        Task<RemoveProviderVenueViewModel> GetRemoveProviderVenueViewModelAsync(int providerVenueId);
        Task<string> GetVenuePostcodeAsync(int providerVenueId);
    }
}