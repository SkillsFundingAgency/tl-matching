using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
    public class OpportunityProximityController : Controller
    {
        private readonly IOpportunityProximityService _opportunityProximityService;
        private readonly ILocationService _locationService;
        private readonly IRoutePathService _routePathService;
        private readonly IOpportunityService _opportunityService;

        public OpportunityProximityController(IRoutePathService routePathService, IOpportunityProximityService opportunityProximityService, IOpportunityService opportunityService, ILocationService locationService)
        {
            _opportunityProximityService = opportunityProximityService;
            _routePathService = routePathService;
            _opportunityService = opportunityService;
            _locationService = locationService;
        }

        [HttpGet]
        [Route("find-providers/{opportunityId?}", Name = "OpportunityProximitySearch")]
        public async Task<IActionResult> Index(int? opportunityId = null)
        {
            var viewModel = new SearchParametersViewModel
            {
                OpportunityId = opportunityId ?? 0,
                SelectedRouteId = null,
                Postcode = null
            };

            return View(nameof(Index), await GetSearchParametersViewModelAsync(viewModel));
        }

        [HttpPost]
        [Route("find-providers/{opportunityId?}", Name = "OpportunityProximitySearch")]
        public async Task<IActionResult> FindProviders(SearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid || !await IsSearchParametersValidAsync(viewModel))
            {
                return View(nameof(Index), await GetSearchParametersViewModelAsync(viewModel));
            }

            return RedirectToRoute("GetOpportunityProviderResults", new SearchParametersViewModel
            {
                SelectedRouteId = viewModel.SelectedRouteId,
                Postcode = viewModel.Postcode,
                OpportunityId = viewModel.OpportunityId,
                OpportunityItemId = viewModel.OpportunityItemId,
                CompanyNameWithAka = viewModel.CompanyNameWithAka
            });
        }

        [Route("provider-results-for-opportunity-{OpportunityId}-item-{OpportunityItemId}-within-30-miles-of-{Postcode}-for-route-{SelectedRouteId}", Name = "GetOpportunityProviderResults")]
        public async Task<IActionResult> GetOpportunityProviderResultsAsync(SearchParametersViewModel searchParametersViewModel)
        {
            var resultsViewModel = await GetSearchResultsAsync(searchParametersViewModel);

            return View("Results", resultsViewModel);
        }

        [HttpPost]
        [Route("[action]/provider-results-for-opportunity-{OpportunityId}-item-{OpportunityItemId}-within-30-miles-of-{Postcode}-for-route-{SelectedRouteId}", Name = "RefineSearchResults")]
        public async Task<IActionResult> RefineSearchResultsAsync(SearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid || !await IsSearchParametersValidAsync(viewModel))
            {

                return View("Results", new OpportunityProximitySearchViewModel
                {
                    SearchParameters = await GetSearchParametersViewModelAsync(viewModel),
                    SearchResults = new OpportunityProximitySearchResultsViewModel(),
                    IsValidSearch = false
                });
            }

            return RedirectToRoute("GetOpportunityProviderResults", viewModel);
        }

        [HttpPost]
        [Route("[action]/provider-results-for-opportunity-{OpportunityId}-item-{OpportunityItemId}-within-30-miles-of-{Postcode}-for-route-{SelectedRouteId}", Name = "ValidateProviderSearchResult")]
        public async Task<IActionResult> ValidateProviderSearchResultAsync(SaveReferralViewModel viewModel)
        {
            if (viewModel.SelectedProvider.Any(p => p.IsSelected))
            {
                viewModel.SelectedProvider = viewModel.SelectedProvider.Where(p => p.IsSelected).ToArray();

                TempData["SelectedProviders"] = JsonConvert.SerializeObject(viewModel);

                return RedirectToRoute("SaveReferral");
            }

            ModelState.AddModelError("SearchResults.Results[0].IsSelected", "You must select at least one provider");

            var searchViewModel = await GetSearchResultsAsync(new SearchParametersViewModel
            {
                Postcode = viewModel.Postcode,
                SelectedRouteId = viewModel.SelectedRouteId,
                OpportunityId = viewModel.OpportunityId,
                OpportunityItemId = viewModel.OpportunityItemId,
                CompanyNameWithAka = viewModel.CompanyNameWithAka
            });

            return View("Results", searchViewModel);
        }

        private async Task<OpportunityProximitySearchViewModel> GetSearchResultsAsync(SearchParametersViewModel viewModel)
        {
            var searchResults = await _opportunityProximityService.SearchOpportunitiesByPostcodeProximityAsync(new OpportunityProximitySearchParametersDto
            {
                Postcode = viewModel.Postcode,
                SelectedRouteId = viewModel.SelectedRouteId,
                SearchRadius = SearchParametersViewModel.DefaultSearchRadius
            });

            var additionalResults = searchResults.Any()
                ? new List<OpportunityProximitySearchResultByRouteViewModelItem>()
                : await _opportunityProximityService.SearchOpportunitiesForOtherRoutesByPostcodeProximityAsync(new OpportunityProximitySearchParametersDto
                {
                    Postcode = viewModel.Postcode,
                    SelectedRouteId = viewModel.SelectedRouteId,
                    SearchRadius = SearchParametersViewModel.ZeroResultsSearchRadius
                });

            var resultsViewModel = new OpportunityProximitySearchViewModel
            {
                SearchResults = new OpportunityProximitySearchResultsViewModel
                {
                    Results = searchResults,
                    AdditionalResults = additionalResults
                },
                SearchParameters = await GetSearchParametersViewModelAsync(viewModel),
                OpportunityId = viewModel.OpportunityId,
                OpportunityItemId = viewModel.OpportunityItemId
            };

            if (searchResults.Any()
                && viewModel.OpportunityId != 0
                && viewModel.OpportunityItemId != 0)
            {
                var opportunityItem =
                    await _opportunityService.GetOpportunityItemAsync(resultsViewModel.SearchParameters
                        .OpportunityItemId);

                if (opportunityItem != null &&
                    opportunityItem.Postcode == resultsViewModel.SearchParameters.Postcode &&
                    opportunityItem.RouteId == resultsViewModel.SearchParameters.SelectedRouteId &&
                    (resultsViewModel.SearchParameters.PreviousPostcode == null
                     || resultsViewModel.SearchParameters.Postcode == resultsViewModel.SearchParameters.PreviousPostcode) &&
                    (resultsViewModel.SearchParameters.PreviousSelectedRouteId == null
                     || resultsViewModel.SearchParameters.SelectedRouteId == resultsViewModel.SearchParameters.PreviousSelectedRouteId))
                {
                    await SetProviderIsSelectedAsync(resultsViewModel);
                }
            }

            resultsViewModel.SearchParameters.PreviousPostcode = viewModel.Postcode;
            resultsViewModel.SearchParameters.PreviousSelectedRouteId = viewModel.SelectedRouteId;

            return resultsViewModel;
        }

        private async Task SetProviderIsSelectedAsync(OpportunityProximitySearchViewModel resultsViewModel)
        {
            var referrals = await _opportunityService.GetReferralsAsync(resultsViewModel.SearchParameters.OpportunityItemId);
            foreach (var result in resultsViewModel.SearchResults.Results)
            {
                if (referrals.Any(r => r.ProviderVenueId == result.ProviderVenueId))
                    result.IsSelected = true;
            }
        }

        private async Task<SearchParametersViewModel> GetSearchParametersViewModelAsync(SearchParametersViewModel viewModel)
        {
            if (viewModel.OpportunityId > 0 && string.IsNullOrEmpty(viewModel.CompanyNameWithAka))
            {
                viewModel.CompanyNameWithAka = await _opportunityService.GetCompanyNameWithAkaAsync(viewModel.OpportunityId);
            }

            var routes = await _routePathService.GetRouteSelectListItemsAsync();

            return new SearchParametersViewModel
            {
                RoutesSelectList = routes,
                SelectedRouteId = viewModel.SelectedRouteId,
                Postcode = viewModel.Postcode?.Trim(),
                OpportunityId = viewModel.OpportunityId,
                OpportunityItemId = viewModel.OpportunityItemId,
                CompanyNameWithAka = viewModel.CompanyNameWithAka,
                PreviousPostcode = viewModel.PreviousPostcode,
                PreviousSelectedRouteId = viewModel.PreviousSelectedRouteId
            };
        }

        private async Task<bool> IsSearchParametersValidAsync(SearchParametersViewModel viewModel)
        {
            var result = true;

            var routeIds = await _routePathService.GetRouteIdsAsync();
            if (viewModel.SelectedRouteId == null || routeIds.All(id => id != viewModel.SelectedRouteId))
            {
                ModelState.AddModelError("SelectedRouteId", "You must select a valid skill area");
                result = false;
            }

            var (isValid, formattedPostcode) = await _locationService.IsValidPostcodeAsync(viewModel.Postcode);
            if (string.IsNullOrWhiteSpace(viewModel.Postcode) || !isValid)
            {
                ModelState.AddModelError("Postcode", "You must enter a real postcode");
                result = false;
            }
            else
            {
                viewModel.Postcode = formattedPostcode;
            }

            return result;
        }
    }
}