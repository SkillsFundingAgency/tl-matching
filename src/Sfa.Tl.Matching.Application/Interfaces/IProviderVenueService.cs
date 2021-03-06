﻿using System.Threading.Tasks;
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
        Task UpdateVenueToNotRemovedAsync(RemoveProviderVenueViewModel viewModel);
        Task<ProviderVenueDetailViewModel> GetVenueAsync(int providerVenueId);
        Task<ProviderVenueDetailViewModel> GetVenueAsync(int providerId, string postcode);
        Task<ProviderVenueDetailViewModel> GetVenueWithTrimmedPostcodeAsync(int providerId, string postcode);
        Task<RemoveProviderVenueViewModel> GetRemoveProviderVenueViewModelAsync(int providerVenueId);
        Task<string> GetVenuePostcodeAsync(int providerVenueId);
    }
}