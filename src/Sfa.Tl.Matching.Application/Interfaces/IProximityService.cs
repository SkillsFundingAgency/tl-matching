﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IProximityService
    {
        Task<IList<SearchResultsViewModelItem>> SearchProvidersByPostcodeProximity(ProviderSearchParametersDto dto);
        Task<IList<SearchResultsByRouteViewModelItem>> SearchProvidersForOtherRoutesByPostcodeProximity(ProviderSearchParametersDto dto);
        Task<(bool, string)> IsValidPostcode(string postcode);
    }
}