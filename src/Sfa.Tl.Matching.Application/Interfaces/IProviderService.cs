using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderService
    {
        Task<ProviderDetailViewModel> GetProviderDetailByIdAsync(int providerId);
        Task<IList<ProviderSearchResultItemViewModel>> SearchProvidersWithFundingAsync(ProviderSearchParametersViewModel searchParameters);
        Task<int> GetProvidersWithFundingCountAsync();
        Task<ProviderSearchResultDto> SearchAsync(long ukPrn);
        Task UpdateProviderDetail(ProviderDetailViewModel viewModel);
        Task UpdateProviderDetailSectionAsync(ProviderDetailViewModel viewModel);
        Task<int> CreateProvider(ProviderDetailViewModel viewModel);
        Task DeleteProviderAsync(int id);
    }
}