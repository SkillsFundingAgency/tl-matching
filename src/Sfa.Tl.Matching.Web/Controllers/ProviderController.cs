using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
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
            return GetIndexView();
        }

        [HttpPost]
        [Route("find-providers", Name = "Providers_Post")]
        public IActionResult Index(SearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return GetIndexView(viewModel.SelectedRouteId, viewModel.Postcode);
            }

            return RedirectToRoute("ProviderResults_Get", new
            {
                routeId = viewModel.SelectedRouteId,
                postcode = viewModel.Postcode,
                searchRadius = SearchParametersViewModel.DefaultSearchRadius
            });
        }

        [Route("provider-results-within-{searchRadius}-miles-of-{postcode}-for-route-{routeId}", Name = "ProviderResults_Get")]
        public async Task<IActionResult> Results(int routeId, string postcode, int searchRadius)
        {
            var searchParametersViewModel = new SearchParametersViewModel
            {
                SelectedRouteId = routeId,
                Postcode = postcode,
                SearchRadius = searchRadius
            };

            var resultsViewModel = await GetSearchResults(searchParametersViewModel);

            return View(resultsViewModel);
        }

        [HttpPost]
        [Route("provider-results", Name = "ProviderResults_Post")]
        public IActionResult Results(SearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return GetResultsView(viewModel.SelectedRouteId, viewModel.Postcode, viewModel.SearchRadius);
            }

            return RedirectToRoute("ProviderResults_Get", new
            {
                routeId = viewModel.SelectedRouteId,
                postcode = viewModel.Postcode,
                searchRadius = viewModel.SearchRadius
            });
        }

        private IActionResult GetIndexView(int? selectedRouteId = null, string postCode = null, int searchRadius = SearchParametersViewModel.DefaultSearchRadius)
        {
            return View(nameof(Index), new SearchParametersViewModel
            {
                RoutesSelectList = _mapper.Map<SelectListItem[]>(GetRoutes()),
                SelectedRouteId = selectedRouteId,
                SearchRadius = searchRadius,
                Postcode = postCode?.Trim()
            });
        }

        private IActionResult GetResultsView(int? selectedRouteId = null, string postCode = null, int searchRadius = SearchParametersViewModel.DefaultSearchRadius)
        {
            return View(nameof(Results), new SearchViewModel
            {
                SearchParameters = new SearchParametersViewModel
                {
                    RoutesSelectList = _mapper.Map<SelectListItem[]>(GetRoutes()),
                    SelectedRouteId = selectedRouteId,
                    SearchRadius = searchRadius,
                    Postcode = postCode
                },
                SearchResults = new SearchResultsViewModel()
            });
        }

        private IOrderedQueryable<Route> GetRoutes()
        {
            return _routePathService.GetRoutes().OrderBy(r => r.Name);
        }

        private async Task<SearchViewModel> GetSearchResults(SearchParametersViewModel viewModel)
        {
            _logger.LogInformation($"Searching for route id {viewModel.SelectedRouteId}, postcode {viewModel.Postcode}");

            var searchResults = await _providerService.SearchProvidersByPostcodeProximity(new ProviderSearchParametersDto
            {
                Postcode = viewModel.Postcode,
                SelectedRouteId = viewModel.SelectedRouteId,
                SearchRadius = viewModel.SearchRadius
            } );

            var resultsViewModel = new SearchViewModel
            {
                SearchResults = new SearchResultsViewModel
                {
                    Results = searchResults
                },
                SearchParameters = new SearchParametersViewModel
                {
                    RoutesSelectList = _mapper.Map<SelectListItem[]>(GetRoutes()),
                    SearchRadius = viewModel.SearchRadius,
                    SelectedRouteId = viewModel.SelectedRouteId,
                    Postcode = viewModel.Postcode?.Trim()
                }
            };

            return resultsViewModel;
        }
    }
}