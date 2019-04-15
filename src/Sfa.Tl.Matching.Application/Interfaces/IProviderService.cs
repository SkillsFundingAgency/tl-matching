using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderService
    {
        Task<Models.ViewModel.ProviderDetailViewModel> GetByIdAsync(int providerId);
        Task<ProviderSearchResultDto> SearchAsync(long ukPrn);
        Task<ProviderDto> GetProviderAsync(int providerId);
        Task<ProviderDto> GetProviderByUkPrnAsync(long ukPrn);
        Task SetIsProviderEnabledAsync(int providerId, bool status);
    }
}