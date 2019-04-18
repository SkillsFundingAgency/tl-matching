using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderService
    {
        Task<ProviderDto> GetProviderByUkPrnAsync(long ukPrn);
        Task SetIsProviderEnabledForSearchAsync(int providerId, bool isEnabled);
        Task<ProviderDetailViewModel> GetProviderDetailByUkprnAsync(long ukPrn, bool includeVenueDetails = true);
        Task<ProviderSearchResultDto> SearchAsync(long ukPrn);
        Task UpdateProviderDetail(ProviderDetailViewModel viewModel);
        Task<int> CreateProvider(ProviderDetailViewModel viewModel);
    }
}