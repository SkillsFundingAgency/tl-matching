using System;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Infrastructure.Extensions;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
    public class ProviderController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ILogger<ProviderController> _logger;
        private readonly IRoutePathService _routePathService;

        public ProviderController(ILogger<ProviderController> logger, IMapper mapper, IRoutePathService routePathService)
        {
            _logger = logger;
            _mapper = mapper;
            _routePathService = routePathService;
        }

        public IActionResult Start()
        {
            return View();
        }
		
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

        [HttpGet]
        public IActionResult Results()
        {
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Results(SearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return GetIndexView(viewModel.SelectedRouteId, viewModel.Postcode);
            }

            _logger.LogInformation($"Searching for route id {viewModel.SelectedRouteId}, postcode {viewModel.Postcode}");

            //TODO: Search and return SearchResultsViewModel
            //return RedirectToAction(nameof(Index), "Results");
            var routes = _routePathService.GetRoutes().OrderBy(r => r.Name);
            var resultsViewModel = new SearchParametersViewModel
            {
                RoutesSelectList = _mapper.Map<SelectListItem[]>(routes),
                SearchRadius = viewModel.SearchRadius,
                SelectedRouteId = viewModel.SelectedRouteId,
                Postcode = viewModel.Postcode
            };
            return View(resultsViewModel);
        }

        private IActionResult GetIndexView(string selectedRouteId = null, string postCode = null)
        {
            var routes = _routePathService.GetRoutes().OrderBy(r => r.Name);

            return View(nameof(Index), new SearchParametersViewModel
            {
                RoutesSelectList = _mapper.Map<SelectListItem[]>(routes),
                SelectedRouteId = selectedRouteId,
                Postcode = postCode
            });
        }
    }
}
