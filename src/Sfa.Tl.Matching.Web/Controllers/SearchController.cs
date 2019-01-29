using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;

namespace Sfa.Tl.Matching.Web.Controllers
{
    public class SearchController : Controller
    {
        private readonly IRoutePathService _routePathLookupService;

        public SearchController(IRoutePathService routePathLookupService)
        {
            _routePathLookupService = routePathLookupService;
        }

        public IActionResult Index()
        {
            //TODO: Add view model and mapper. ToListAsync() is probably inside mapper.
            var model = _routePathLookupService.GetRoutes();

            //TODO: Create actual view and remove R# comment
            // ReSharper disable once Mvc.ViewNotResolved
            return View(model);
        }
    }
}
