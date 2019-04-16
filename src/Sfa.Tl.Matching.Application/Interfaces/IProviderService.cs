using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderService
    {
        Task<ProviderDto> GetProviderAsync(int providerId);
        Task<ProviderDto> GetProviderByUkPrnAsync(long ukPrn);
        Task SetIsProviderEnabledAsync(int providerId, bool status);
        Task<ProviderDetailViewModel> GetByIdAsync(int providerId, bool includeVeuneDetails = true);
        Task<ProviderSearchResultDto> SearchAsync(long ukPrn);
        Task UpdateProvider(ProviderDetailViewModel viewModel);
        Task<int> CreateProvider(ProviderDetailViewModel viewModel);
    }
}