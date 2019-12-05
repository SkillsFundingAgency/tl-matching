using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class ProviderProximityController : Controller
    {
        private readonly ILocationService _locationService;
        private readonly IProviderProximityService _providerProximityService;
        private readonly IRoutePathService _routePathService;

        public ProviderProximityController(
            IRoutePathService routePathService,
            IProviderProximityService providerProximityService,
            ILocationService locationService)
        {
            _routePathService = routePathService;
            _providerProximityService = providerProximityService;
            _locationService = locationService;
        }

        [Route("find-all-providers/{opportunityId?}", Name = "ProviderProximitySearch")]
        public IActionResult SearchPostcode()
        {
            return View();
        }

        [HttpPost]
        [Route("find-all-providers/{opportunityId?}", Name = "ProviderProximitySearch")]
        public async Task<IActionResult> FindAllProviders(ProviderProximitySearchParamViewModel viewModel)
        {
            if (!ModelState.IsValid || !await IsPostCodeValidAsync(viewModel))
            {
                return View(nameof(SearchPostcode), viewModel);
            }

            return RedirectToRoute("GetProviderProximityResults", new { searchCriteria = viewModel.Postcode });
        }

        [HttpGet]
        [Route("all-provider-results-{searchCriteria}", Name = "GetProviderProximityResults")]
        public async Task<IActionResult> GetProviderProximityResults(string searchCriteria)
        {
            var routeNames = await _routePathService.GetRouteDictionaryAsync();

            var searchParametersViewModel = new ProviderProximitySearchParametersViewModel(searchCriteria, routeNames.Values);
            var viewModel = new ProviderProximitySearchViewModel(searchParametersViewModel);

            viewModel.SearchResults = new ProviderProximitySearchResultsViewModel
            {
                Results = await _providerProximityService.SearchProvidersByPostcodeProximityAsync(
                    new ProviderProximitySearchParametersDto
                    {
                        Postcode = viewModel.SearchParameters.Postcode,
                        SearchRadius = SearchParametersViewModel.DefaultSearchRadius,
                        SelectedRoutes = routeNames.Where(r => viewModel.SearchParameters.SelectedFilters.Contains(r.Value)).Select(r => r.Key).ToList()
                    })
            };

            return View("Results", viewModel);
        }

        [HttpPost]
        public IActionResult FilterResultsAsync(ProviderProximitySearchParametersViewModel viewModel)
        {
            if (viewModel.Filters == null || viewModel.Filters.Count(f => f.IsSelected) == 0)
                return RedirectToRoute("GetProviderProximityResults", new
                {
                    searchCriteria = $"{viewModel.Postcode}"
                });

            var filters = string.Join("-", viewModel.Filters.Where(f => f.IsSelected).Select(f => f.Name));

            return RedirectToRoute("GetProviderProximityResults", new
            {
                searchCriteria = $"{viewModel.Postcode}-{filters}"
            });
        }

        private async Task<bool> IsPostCodeValidAsync(ProviderProximitySearchParamViewModel viewModel)
        {
            var result = true;

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