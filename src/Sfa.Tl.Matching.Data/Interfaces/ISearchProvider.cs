﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Data.Interfaces
{
    public interface ISearchProvider
    {
        Task<IList<SearchResultsViewModelItem>> SearchProvidersByPostcodeProximity(ProviderSearchParametersDto dto);

        Task<IList<SearchResultsByRouteViewModelItem>> SearchProvidersForOtherRoutesByPostcodeProximity(ProviderSearchParametersDto dto);
    }
}
