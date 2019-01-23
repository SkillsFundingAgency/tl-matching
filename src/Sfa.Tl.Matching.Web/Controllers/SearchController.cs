using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Core.Interfaces;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IRoutePathService _routePathLookupService;

        public SearchController(IRoutePathService routePathLookupService)
        {
            _routePathLookupService = routePathLookupService;
        }

        public async Task<IActionResult> Index()
        {
            //TODO: Add view model and view model mapper
            var model = await _routePathLookupService.GetRoutesAsync();

            return View(model);
        }
    }
}
