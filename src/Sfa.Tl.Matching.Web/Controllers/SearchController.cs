using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sfa.Tl.Matching.Web.Mappers;
using Sfa.Tl.Matching.Infrastructure.Extensions;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.StandardUser + "," + RolesExtensions.AdminUser)]
    public class SearchController : Controller
    {
        private readonly ILogger<SearchController> _logger;
        private readonly ISearchParametersViewModelMapper _viewModelMapper;

        public SearchController(ISearchParametersViewModelMapper viewModelMapper, ILogger<SearchController> logger)
        {
            _viewModelMapper = viewModelMapper;
            _logger = logger;
        }

        public IActionResult Start()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Results(SearchQueryViewModel viewModel)
        {
            return View();
        }
		
        public IActionResult Index(string selectedRouteId, string postcode)//SearchParametersViewModel viewModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    //return RedirectToAction(nameof(Index), "Search", new { viewModel.SelectedRouteId, viewModel.Postcode });
            //}

            //TODO: Search and redirect to the results page
            //await _searchService.Search(model.);
            //return RedirectToAction(nameof(SearchResults), "Search");

            try
            {

                var viewModel = _viewModelMapper.Populate(selectedRouteId, postcode);

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Loading of routes failed.");
                throw;
            }
        }

        public IActionResult Search(SearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                //return RedirectToAction(nameof(Index), "Search", new { viewModel.SelectedRouteId, viewModel.Postcode });
                viewModel = _viewModelMapper.Populate(viewModel.SelectedRouteId, viewModel.Postcode);
                return View(nameof(Index), viewModel);
            }

            _logger.LogInformation($"Searching for route id {viewModel.SelectedRouteId}, postcode {viewModel.Postcode}");
            //await _searchService.Search(model.);

            return RedirectToAction(nameof(Index), "Search");
        }
    }
}
