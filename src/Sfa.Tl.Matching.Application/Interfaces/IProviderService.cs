using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderService
    {
        Task<HideProviderViewModel> GetHideProviderViewModelAsync(int providerId);
        Task SetIsProviderEnabledForSearchAsync(int providerId, bool isEnabled);
        Task<ProviderDetailViewModel> GetProviderDetailByIdAsync(int providerId, bool includeVenueDetails = false);
        Task<ProviderSearchResultDto> SearchAsync(long ukPrn);
        Task UpdateProviderDetail(ProviderDetailViewModel viewModel);
        Task<int> CreateProvider(ProviderDetailViewModel viewModel);
        Task<bool> ProviderHasAnyVenueAsync(int providerId);
    }
}