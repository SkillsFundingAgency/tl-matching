using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
    public class ProviderController : Controller
    {
        private const string SearchParametersDataKey = "SearchParameters";

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
            try
            {
                return GetIndexView();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Loading of routes failed.");
                throw;
            }
        }

        [HttpPost]
        [Route("find-providers", Name = "Providers_Post")]
        public IActionResult Index(SearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return GetIndexView(viewModel.SelectedRouteId, viewModel.Postcode);
            }

            var serializedData = JsonConvert.SerializeObject(viewModel);
            TempData[SearchParametersDataKey] = serializedData;
            return RedirectToAction("Results");
        }

        [HttpGet]
        [Route("provider-results", Name = "ProviderResults_Get")]
        public async Task<IActionResult> Results()
        {
            var obj = TempData[SearchParametersDataKey];
            //var viewModel = obj != null
            //    ? JsonConvert.DeserializeObject<SearchParametersViewModel>((string)obj)
            //    : new SearchParametersViewModel();

            //viewModel.RoutesSelectList = _mapper.Map<SelectListItem[]>(GetRoutes());

            SearchViewModel resultsViewModel;
            if (obj != null)
            {
                var searchParametersViewModel = JsonConvert.DeserializeObject<SearchParametersViewModel>((string)obj);
                resultsViewModel = await GetSearchResultsAsync(searchParametersViewModel);
            }
            else
            {
                resultsViewModel = new SearchViewModel
                {
                    SearchParameters = new SearchParametersViewModel
                    {
                        RoutesSelectList = _mapper.Map<SelectListItem[]>(GetRoutes())
                    },
                    SearchResults = new SearchResultsViewModel()
                };
            }

            return View(resultsViewModel);
        }

        [HttpPost]
        [Route("provider-results", Name = "ProviderResults_Post")]
        public async Task<IActionResult> Results(SearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return GetResultsView(viewModel.SelectedRouteId, viewModel.Postcode, viewModel.SearchRadius);
            }

            var resultsViewModel = await GetSearchResultsAsync(viewModel);
            return View(resultsViewModel);
        }

        private IActionResult GetIndexView(string selectedRouteId = null, string postCode = null, int searchRadius = SearchParametersViewModel.DefaultSearchRadius)
        {
            return View(nameof(Index), new SearchParametersViewModel
            {
                RoutesSelectList = _mapper.Map<SelectListItem[]>(GetRoutes()),
                SelectedRouteId = selectedRouteId,
                SearchRadius = searchRadius,
                Postcode = postCode?.Trim()
            });
        }

        private IActionResult GetResultsView(string selectedRouteId = null, string postCode = null, int searchRadius = SearchParametersViewModel.DefaultSearchRadius)
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

        private async Task<SearchViewModel> GetSearchResultsAsync(SearchParametersViewModel viewModel)
        {
            _logger.LogInformation($"Searching for route id {viewModel.SelectedRouteId}, postcode {viewModel.Postcode}");

            var searchResults = await _providerService.SearchProvidersByPostcodeProximity(viewModel.Postcode, viewModel.SearchRadius, int.Parse(viewModel.SelectedRouteId));

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
