using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderVenueService
    {
        Task<(bool, string)> IsValidPostCodeAsync(string postCode);
        Task<int> CreateVenueAsync(AddProviderVenueViewModel viewModel);
        Task<ProviderVenueDetailViewModel> GetVenueWithQualificationsAsync(int providerVenueId);
        Task UpdateVenueAsync(ProviderVenueDetailViewModel viewModel);
        Task UpdateVenueAsync(RemoveProviderVenueViewModel viewModel);
        Task<ProviderVenueDetailViewModel> GetVenue(int providerId, string postCode);
        Task<RemoveProviderVenueViewModel> GetRemoveProviderVenueViewModelAsync(int providerVenueId);
    }
}