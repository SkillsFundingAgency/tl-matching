using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
    public class ProviderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProviderController> _logger;
        private readonly IProviderService _providerService;
        private readonly IRoutePathService _routePathService;

        public ProviderController(ILogger<ProviderController> logger, IMapper mapper, IRoutePathService routePathService, IProviderService providerService)
        {
            _logger = logger;
            _mapper = mapper;
            _providerService = providerService;
            _routePathService = routePathService;
        }

        [Route("Start", Name = "Start_Get")]
        public IActionResult Start()
        {
            return View();
        }

        [HttpGet]
        [Route("find-providers", Name = "Providers_Get")]
        public IActionResult Index()
        {
            return GetIndexViewAsync();
        }

        [HttpPost]
        [Route("find-providers", Name = "Providers_Post")]
        public async Task<IActionResult> Index(SearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid || !await IsSearchParametersValidAsync(viewModel.SelectedRouteId, viewModel.Postcode, SearchParametersViewModel.DefaultSearchRadius))
            {
                return GetIndexViewAsync(viewModel.SelectedRouteId, viewModel.Postcode, viewModel.SearchRadius);
            }

            return RedirectToRoute("ProviderResults_Get", new SearchParametersViewModel
            {
                SelectedRouteId = viewModel.SelectedRouteId,
                Postcode = viewModel.Postcode,
                SearchRadius = SearchParametersViewModel.DefaultSearchRadius
            });
        }

        [Route("provider-results-within-{SearchRadius}-miles-of-{Postcode}-for-route-{SelectedRouteId}", Name = "ProviderResults_Get")]
        public async Task<IActionResult> Results(SearchParametersViewModel searchParametersViewModel)
        {
            var resultsViewModel = await GetSearchResultsAsync(searchParametersViewModel);

            return View(resultsViewModel);
        }

        [HttpPost]
        [Route("provider-results", Name = "ProviderResults_Post")]
        public async Task<IActionResult> RefineSearchResults(SearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid || !await IsSearchParametersValidAsync(viewModel.SelectedRouteId, viewModel.Postcode,
                    viewModel.SearchRadius))
            {
                return View(nameof(Results), new SearchViewModel
                {
                    SearchParameters = GetSearchParametersViewModelAsync(viewModel.SelectedRouteId, viewModel.Postcode?.Trim(), viewModel.SearchRadius),
                    SearchResults = new SearchResultsViewModel(),
                    IsValidSearch = false
                });
            }

            return RedirectToRoute("ProviderResults_Get", viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ValidateProviderSearchResult(CreateReferralViewModel viewModel)
        {
            if (viewModel.SelectedProvider.Any(p => p.IsSelected))
            {
                viewModel.SelectedProvider = viewModel.SelectedProvider.Where(p => p.IsSelected).ToArray();

                var selectedProviders = JsonConvert.SerializeObject(viewModel);

                return RedirectToRoute("CreateReferral", new { viewModel = selectedProviders });
            }

            ModelState.AddModelError("SearchResults.Results[0].IsSelected", "You must select at least one provider");

            var searchViewModel = await GetSearchResultsAsync(new SearchParametersViewModel
            {
                Postcode = viewModel.Postcode,
                SelectedRouteId = viewModel.SelectedRouteId,
                SearchRadius = viewModel.SearchRadius
            });

            return View(nameof(Results), searchViewModel);
        }

        private IActionResult GetIndexViewAsync(int? selectedRouteId = null, string postCode = null, int searchRadius = SearchParametersViewModel.DefaultSearchRadius)
        {
            return View(nameof(Index), GetSearchParametersViewModelAsync(selectedRouteId, postCode?.Trim(), searchRadius));
        }

        private async Task<SearchViewModel> GetSearchResultsAsync(SearchParametersViewModel viewModel)
        {
            _logger.LogInformation($"Searching for route id {viewModel.SelectedRouteId}, postcode {viewModel.Postcode}");

            var searchResults = await _providerService.SearchProvidersByPostcodeProximity(new ProviderSearchParametersDto
            {
                Postcode = viewModel.Postcode,
                SelectedRouteId = viewModel.SelectedRouteId,
                SearchRadius = viewModel.SearchRadius
            });

            var resultsViewModel = new SearchViewModel
            {
                SearchResults = new SearchResultsViewModel
                {
                    Results = _mapper.Map<List<SearchResultsViewModelItem>>(searchResults)
                },
                SearchParameters = GetSearchParametersViewModelAsync(viewModel.SelectedRouteId, viewModel.Postcode?.Trim(), viewModel.SearchRadius)
            };

            return resultsViewModel;
        }

        private SearchParametersViewModel GetSearchParametersViewModelAsync(int? selectedRouteId = null, string postCode = null, int searchRadius = SearchParametersViewModel.DefaultSearchRadius)
        {
            var routes = _routePathService.GetRoutes().OrderBy(r => r.Name).ToList();

            return new SearchParametersViewModel
            {
                RoutesSelectList = _mapper.Map<SelectListItem[]>(routes),
                SelectedRouteId = selectedRouteId,
                SearchRadius = searchRadius != 0 ? searchRadius : SearchParametersViewModel.DefaultSearchRadius,
                Postcode = postCode?.Trim()
            };
        }

        private async Task<bool> IsSearchParametersValidAsync(int? selectedRouteId, string postCode, int? searchRadius)
        {
            var result = true;

            var routes = _routePathService.GetRoutes().OrderBy(r => r.Name).ToList();
            if (selectedRouteId == null || routes.All(r => r.Id != selectedRouteId))
            {
                ModelState.AddModelError("SelectedRouteId", "You must select a valid skill area");
                result = false;
            }

            var isPostcodeValidation = await _providerService.IsValidPostCode(postCode);
            if (string.IsNullOrWhiteSpace(postCode) || !isPostcodeValidation)
            {
                ModelState.AddModelError("Postcode", "You must enter a real postcode");
                result = false;
            }

            if (searchRadius == null || searchRadius < 5 || searchRadius > 25)
            {
                ModelState.AddModelError("SearchRadius", "You must select a valid SearchRadius");
                result = false;
            }

            return result;
        }
    }
}