﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Application.Interfaces
{
    public interface IOpportunityProximityService
    {
        Task<IList<SearchResultsViewModelItem>> SearchOpportunitiesByPostcodeProximityAsync(ProviderSearchParametersDto dto);
        Task<IList<SearchResultsByRouteViewModelItem>> SearchOpportunitiesForOtherRoutesByPostcodeProximityAsync(ProviderSearchParametersDto dto);
    }
}