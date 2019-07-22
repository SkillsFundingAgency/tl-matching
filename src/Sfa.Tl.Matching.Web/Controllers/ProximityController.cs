// ReSharper disable RedundantUsingDirective

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
#if !NoAuth
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
#endif
    public class ProximityController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IProximityService _proximityService;
        private readonly IRoutePathService _routePathService;
        private readonly IOpportunityService _opportunityService;
        private readonly IEmployerService _employerService;

        public ProximityController(IMapper mapper, IRoutePathService routePathService, IProximityService proximityService,
            IOpportunityService opportunityService, IEmployerService employerService)
        {
            _mapper = mapper;
            _proximityService = proximityService;
            _routePathService = routePathService;
            _opportunityService = opportunityService;
            _employerService = employerService;
        }

        [Route("Start", Name = "Start")]
        public async Task<IActionResult> Start()
        {
            var username = HttpContext.User.GetUserName();
            var savedOpportunitiesCount = await _employerService.GetInProgressEmployerOpportunityCountAsync(username);

            return View(new DashboardViewModel
            {
                HasSavedOppportunities = savedOpportunitiesCount > 0
            });
        }

        [HttpGet]
        [Route("find-providers/{opportunityId?}", Name = "FindProviders")]
        public async Task<IActionResult> Index(int? opportunityId = null)
        {
            var viewModel = new SearchParametersViewModel
            {
                OpportunityId = opportunityId ?? 0,
                SelectedRouteId = null,
                Postcode = null,
                SearchRadius = SearchParametersViewModel.DefaultSearchRadius
            };

            return View(nameof(Index), await GetSearchParametersViewModelAsync(viewModel));
        }

        [HttpPost]
        [Route("find-providers")]
        public async Task<IActionResult> FindProviders(SearchParametersViewModel viewModel)
        {
            viewModel.SearchRadius = SearchParametersViewModel.DefaultSearchRadius;
            if (!ModelState.IsValid || !await IsSearchParametersValidAsync(viewModel))
            {
                return View(nameof(Index), await GetSearchParametersViewModelAsync(viewModel));
            }

            return RedirectToRoute("GetProviderResults", new SearchParametersViewModel
            {
                SelectedRouteId = viewModel.SelectedRouteId,
                Postcode = viewModel.Postcode,
                SearchRadius = SearchParametersViewModel.DefaultSearchRadius,
                OpportunityId = viewModel.OpportunityId,
                OpportunityItemId = viewModel.OpportunityItemId,
                CompanyNameWithAka = viewModel.CompanyNameWithAka
            });
        }

        [Route("provider-results-for-opportunity-{OpportunityId}-item-{OpportunityItemId}-within-{SearchRadius}-miles-of-{Postcode}-for-route-{SelectedRouteId}", Name = "GetProviderResults")]
        public async Task<IActionResult> Results(SearchParametersViewModel searchParametersViewModel)
        {
            var resultsViewModel = await GetSearchResultsAsync(searchParametersViewModel);

            return View(resultsViewModel);
        }

        [HttpPost]
        [Route("provider-results-for-opportunity-{OpportunityId}-item-{OpportunityItemId}-within-{SearchRadius}-miles-of-{Postcode}-for-route-{SelectedRouteId}")]
        public async Task<IActionResult> RefineSearchResults(SearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid || !await IsSearchParametersValidAsync(viewModel))
            {
                return View(nameof(Results), new SearchViewModel
                {
                    SearchParameters = await GetSearchParametersViewModelAsync(viewModel),
                    SearchResults = new SearchResultsViewModel(),
                    IsValidSearch = false
                });
            }

            return RedirectToRoute("GetProviderResults", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ValidateProviderSearchResult(SaveReferralViewModel viewModel)
        {
            if (viewModel.SelectedProvider.Any(p => p.IsSelected))
            {
                viewModel.SelectedProvider = viewModel.SelectedProvider.Where(p => p.IsSelected).ToArray();

                var selectedProviders = JsonConvert.SerializeObject(viewModel);

                return RedirectToRoute("SaveReferral", new { viewModel = selectedProviders });
            }

            ModelState.AddModelError("SearchResults.Results[0].IsSelected", "You must select at least one provider");

            var searchViewModel = await GetSearchResultsAsync(new SearchParametersViewModel
            {
                Postcode = viewModel.Postcode,
                SelectedRouteId = viewModel.SelectedRouteId,
                SearchRadius = viewModel.SearchRadius,
                OpportunityId = viewModel.OpportunityId,
                OpportunityItemId = viewModel.OpportunityItemId,
                CompanyNameWithAka = viewModel.CompanyNameWithAka
            });

            return View(nameof(Results), searchViewModel);
        }

        private async Task<SearchViewModel> GetSearchResultsAsync(SearchParametersViewModel viewModel)
        {
            var searchResults = await _proximityService.SearchProvidersByPostcodeProximity(new ProviderSearchParametersDto
            {
                Postcode = viewModel.Postcode,
                SelectedRouteId = viewModel.SelectedRouteId,
                SearchRadius = viewModel.SearchRadius
            });

            var resultsViewModel = new SearchViewModel
            {
                SearchResults = new SearchResultsViewModel
                {
                    Results =  searchResults
                },
                SearchParameters = await GetSearchParametersViewModelAsync(viewModel),
                OpportunityId = viewModel.OpportunityId,
                OpportunityItemId = viewModel.OpportunityItemId
            };

            if (viewModel.OpportunityId == 0 && viewModel.OpportunityItemId == 0)
                return resultsViewModel;

            var selectedResultsViewModel = SetProviderIsSelected(resultsViewModel);

            return selectedResultsViewModel;
        }

        private SearchViewModel SetProviderIsSelected(SearchViewModel resultsViewModel)
        {
            var referrals = _opportunityService.GetReferrals(resultsViewModel.SearchParameters.OpportunityItemId);
            foreach (var result in resultsViewModel.SearchResults.Results)
            {
                if (referrals.Any(r => r.ProviderVenueId == result.ProviderVenueId))
                    result.IsSelected = true;
            }

            return resultsViewModel;
        }

        private async Task<SearchParametersViewModel> GetSearchParametersViewModelAsync(SearchParametersViewModel viewModel)
        {
            var routes = _routePathService.GetRoutes().OrderBy(r => r.Name).ToList();

            if (viewModel.OpportunityId > 0 && string.IsNullOrEmpty(viewModel.CompanyNameWithAka))
            {
                viewModel.CompanyNameWithAka = await _opportunityService.GetCompanyNameWithAkaAsync(viewModel.OpportunityId);
            }

            return new SearchParametersViewModel
            {
                RoutesSelectList = _mapper.Map<SelectListItem[]>(routes),
                SelectedRouteId = viewModel.SelectedRouteId,
                SearchRadius = viewModel.SearchRadius != 0 ? viewModel.SearchRadius : SearchParametersViewModel.DefaultSearchRadius,
                Postcode = viewModel.Postcode?.Trim(),
                OpportunityId = viewModel.OpportunityId,
                OpportunityItemId = viewModel.OpportunityItemId,
                CompanyNameWithAka = viewModel.CompanyNameWithAka
            };
        }

        private async Task<bool> IsSearchParametersValidAsync(SearchParametersViewModel viewModel)
        {
            var result = true;

            var routes = _routePathService.GetRoutes().OrderBy(r => r.Name).ToList();
            if (viewModel.SelectedRouteId == null || routes.All(r => r.Id != viewModel.SelectedRouteId))
            {
                ModelState.AddModelError("SelectedRouteId", "You must select a valid skill area");
                result = false;
            }

            var (isValid, formatedPostCode) = await _proximityService.IsValidPostCode(viewModel.Postcode);
            if (string.IsNullOrWhiteSpace(viewModel.Postcode) || !isValid)
            {
                ModelState.AddModelError("Postcode", "You must enter a real postcode");
                result = false;
            }
            else
            {
                viewModel.Postcode = formatedPostCode;
            }

            if (viewModel.SearchRadius < 5 || viewModel.SearchRadius > 25)
            {
                ModelState.AddModelError("SearchRadius", "You must select a valid SearchRadius");
                result = false;
            }

            return result;
        }
    }
}