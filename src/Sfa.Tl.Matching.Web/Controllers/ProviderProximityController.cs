using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class ProviderProximityController : Controller
    {
        private readonly IRoutePathService _routePathService;

        public ProviderProximityController(IRoutePathService routePathService)
        {
            _routePathService = routePathService;
        }

        [HttpGet]
        [Route("provider-results-{searchCriteria}", Name = "GetProviderProximityResults", Order = 1)]
        public async Task<IActionResult> GetProviderProximityResults(string searchCriteria)
        {
            var routeNames = _routePathService.GetRoutes().OrderBy(r => r.Name)
                .Select(r => r.Name).ToList();

            var searchParametersViewModel = new ProviderProximitySearchParametersViewModel(searchCriteria, routeNames);
            var viewModel = new ProviderProximitySearchViewModel(searchParametersViewModel);

            return View("Results", viewModel);
        }

        [HttpPost]
        //[Route("[action]/provider-results-{searchCriteria}", Name = "FilterResultsAsync")]
        public async Task<IActionResult> FilterResultsAsync(ProviderProximitySearchParametersViewModel viewModel)
        {
            return RedirectToRoute("GetProviderProximityResults", viewModel);
        }

        [HttpPost]
        public IActionResult Finish()
        {
            return RedirectToRoute("Start");
        }
    }
}