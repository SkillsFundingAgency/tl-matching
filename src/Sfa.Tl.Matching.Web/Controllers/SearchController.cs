using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Models;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
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

        public IActionResult Index()
        {
            var model = _routePathLookupService.GetRoutes();

            return View(model);
        }
        
        [HttpPost]
        public IActionResult Results(SearchQueryViewModel viewModel)
        {
            return View();
        }
    }
}
