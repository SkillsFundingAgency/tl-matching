using System;
using System.Linq;
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
        private readonly IRoutePathService _routePathService;

        public ProviderController(ILogger<ProviderController> logger, IMapper mapper, IRoutePathService routePathService)
        {
            _logger = logger;
            _mapper = mapper;
            _routePathService = routePathService;
        }

        [Route("Start")]
        public IActionResult Start()
        {
            return View();
        }

        [HttpGet]
        [Route("find-providers")]
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
        [Route("find-providers")]
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
        [Route("provider-results")]
        public IActionResult Results()
        {
            var obj = TempData[SearchParametersDataKey];
            var viewModel = obj != null
                ? JsonConvert.DeserializeObject<SearchParametersViewModel>((string)obj)
                : new SearchParametersViewModel();

            viewModel.RoutesSelectList = _mapper.Map<SelectListItem[]>(GetRoutes());

            return View(viewModel);
        }

        [HttpPost]
        [Route("provider-results")]
        public IActionResult Results(SearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return GetResultsView(viewModel.SelectedRouteId, viewModel.Postcode, viewModel.SearchRadius);
            }

            _logger.LogInformation($"Searching for route id {viewModel.SelectedRouteId}, postcode {viewModel.Postcode}");

            var resultsViewModel = new SearchParametersViewModel
            {
                RoutesSelectList = _mapper.Map<SelectListItem[]>(GetRoutes()),
                SearchRadius = viewModel.SearchRadius,
                SelectedRouteId = viewModel.SelectedRouteId,
                Postcode = viewModel.Postcode
            };
            return View(resultsViewModel);
        }

        private IActionResult GetIndexView(string selectedRouteId = null, string postCode = null, int searchRadius = SearchParametersViewModel.DefaultSearchRadius)
        {
            return View(nameof(Index), new SearchParametersViewModel
            {
                RoutesSelectList = _mapper.Map<SelectListItem[]>(GetRoutes()),
                SelectedRouteId = selectedRouteId,
                SearchRadius = searchRadius,
                Postcode = postCode
            });
        }

        private IActionResult GetResultsView(string selectedRouteId = null, string postCode = null, int searchRadius = SearchParametersViewModel.DefaultSearchRadius)
        {
            return View(nameof(Results), new SearchParametersViewModel
            {
                RoutesSelectList = _mapper.Map<SelectListItem[]>(GetRoutes()),
                SelectedRouteId = selectedRouteId,
                SearchRadius = searchRadius,
                Postcode = postCode
            });
        }

        private IOrderedQueryable<Route> GetRoutes()
        {
            return _routePathService.GetRoutes().OrderBy(r => r.Name);
        }
    }
}
