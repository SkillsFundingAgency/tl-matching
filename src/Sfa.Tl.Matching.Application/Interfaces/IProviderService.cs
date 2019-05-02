﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProviderService
    {
        Task<HideProviderViewModel> GetHideProviderViewModelAsync(int providerId);
        Task<ProviderDetailViewModel> GetProviderDetailByIdAsync(int providerId, bool includeVenueDetails = false);
		Task<IList<ProviderVenueViewModel>> GetProviderVenueSummaryByProviderIdAsync(int providerId, bool includeVenueDetails = false);
        Task<IList<ProviderSearchResultItemViewModel>> SearchProvidersWithFundingAsync(
            ProviderSearchParametersViewModel searchParameters);
        Task<int> GetProvidersWithFundingCountAsync();
        Task<ProviderSearchResultDto> SearchAsync(long ukPrn);
        Task UpdateProviderAsync(HideProviderViewModel viewModel);
        Task UpdateProviderDetail(ProviderDetailViewModel viewModel);
        Task UpdateProvider(SaveProviderFeedbackViewModel viewModel);
        Task<int> CreateProvider(ProviderDetailViewModel viewModel);
    }
}