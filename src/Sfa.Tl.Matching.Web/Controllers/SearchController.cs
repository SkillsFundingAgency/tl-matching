using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IRoutePathService _routePathLookupService;

        public SearchController(IRoutePathService routePathLookupService)
        {
            _routePathLookupService = routePathLookupService;
        }

        public IActionResult Start()
        {
            return View();
        }
        public async Task<IActionResult> Index()
        {
            //TODO: Add view model and mapper
            var model = await _routePathLookupService.GetRoutesAsync();

            return View(model);
        }
        
        [HttpPost]
        public IActionResult Results(SearchQueryViewModel viewModel)
        {
            return View();
        }
    }
}
