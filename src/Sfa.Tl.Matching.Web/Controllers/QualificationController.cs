using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class QualificationController : Controller
    {
        private readonly IProviderVenueService _providerVenueService;
        private readonly IQualificationService _qualificationService;
        private readonly IProviderQualificationService _providerQualificationService;
        private readonly IRoutePathService _routePathService;

        public QualificationController(IProviderVenueService providerVenueService,
            IQualificationService qualificationService,
            IProviderQualificationService providerQualificationService,
            IRoutePathService routePathService)
        {
            _providerVenueService = providerVenueService;
            _qualificationService = qualificationService;
            _providerQualificationService = providerQualificationService;
            _routePathService = routePathService;
        }

        [Route("add-qualification/{providerVenueId}", Name = "AddQualification")]
        public async Task<IActionResult> AddQualificationAsync(int providerVenueId)
        {
            var postcode = await _providerVenueService.GetVenuePostcodeAsync(providerVenueId);

            return View("AddQualification", new AddQualificationViewModel
            {
                ProviderVenueId = providerVenueId,
                Postcode = postcode
            });
        }

        [HttpPost]
        [Route("add-qualification/{providerId}", Name = "CreateQualification")]
        public async Task<IActionResult> CreateQualificationAsync(AddQualificationViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("AddQualification", viewModel);

            var isValid = await _qualificationService.IsValidLarIdAsync(viewModel.LarId);

            if (string.IsNullOrWhiteSpace(viewModel.LarId) || !isValid)
            {
                ModelState.AddModelError("LarId", "Enter a learning aim reference (LAR) that has 8 characters");
                return View("AddQualification", viewModel);
            }

            var qualification = await _qualificationService.GetQualificationAsync(viewModel.LarId);

            if (qualification == null)
            {
                var isValidLar = await _qualificationService.IsValidOfqualLarIdAsync(viewModel.LarId);
                if (!isValidLar)
                {
                    ModelState.AddModelError("LarId", "You must enter a real learning aim reference (LAR)");
                    return View("AddQualification", viewModel);
                }

                return RedirectToRoute("MissingQualification",
                    new
                    {
                        providerVenueId = viewModel.ProviderVenueId,
                        larId = viewModel.LarId
                    });
            }

            viewModel.QualificationId = qualification.Id;
            await _providerQualificationService.CreateProviderQualificationAsync(viewModel);

            return RedirectToRoute("GetProviderVenueDetail",
                new { providerVenueId = viewModel.ProviderVenueId });
        }

        [Route("edit-qualifications", Name = "EditQualifications")]
        public IActionResult EditQualifications()
        {
            return View("SearchQualifications", new QualificationSearchViewModel());
        }

        [HttpPost]
        [Route("edit-qualifications", Name = "SearchQualifications")]
        public async Task<IActionResult> SearchQualificationsAsync(QualificationSearchViewModel viewModel)
        {
            if (IsValidSearchTerm(viewModel))
                ModelState.AddModelError("SearchTerms", "You must enter 2 or more letters for your search");

            if (!ModelState.IsValid)
                return View("SearchQualifications", viewModel);

            var searchResult = await _qualificationService.SearchQualificationAsync(viewModel.SearchTerms);

            await PopulateRoutesForQualificationSearchItem(searchResult);

            return View("SearchQualifications", searchResult);
        }

        private static bool IsValidSearchTerm(QualificationSearchViewModel viewModel)
        {
            return viewModel.SearchTerms.IsAllSpecialCharactersOrNumbers() ||
                   viewModel.SearchTerms.ToLetter().Length < 2;
        }

        [HttpGet]
        [Route("search-short-title", Name = "SearchShortTitle")]
        public async Task<IActionResult> SearchShortTitleAsync(string query)
        {
            var shortTitles = await _qualificationService.SearchShortTitleAsync(query);

            return Ok(shortTitles);
        }

        [HttpPost]
        [Route("save-qualification", Name = "SaveQualification")]
        public async Task<IActionResult> SaveQualificationAsync(SaveQualificationViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
            {
                var errorList = ModelState.Where(e => e.Value.Errors.Any()).ToDictionary(
                    kvp => kvp.Key,
                    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

                var errors = JsonConvert.SerializeObject(errorList);

                return Json(new { success = false, response = errors });
            }

            await _qualificationService.UpdateQualificationAsync(viewModel);

            return Json(new { success = true, response = string.Empty });
        }

        [Route("missing-qualification/{providerVenueId}/{larId}", Name = "MissingQualification")]
        public async Task<IActionResult> MissingQualificationAsync(int providerVenueId, string larId)
        {
            //Get title from service, based on LAR
            var title = await _qualificationService.GetLarTitleAsync(larId);

            var routes = await GetRoutesAsync();

            return View("MissingQualification", new MissingQualificationViewModel
            {
                ProviderVenueId = providerVenueId,
                LarId = larId,
                QualificationId = 1,
                Title = title,
                Routes = routes
            });
        }

        [HttpPost]
        [Route("missing-qualification/{providerVenueId}/{larId}", Name = "SaveMissingQualification")]
        public async Task<IActionResult> SaveMissingQualificationAsync(MissingQualificationViewModel viewModel)
        {
            Validate(viewModel);

            if (!ModelState.IsValid)
            {
                viewModel.Routes = await GetRoutesAsync(viewModel);
                return View("MissingQualification", viewModel);
            }

            var qualificationId = await _qualificationService.CreateQualificationAsync(viewModel);

            await _providerQualificationService.CreateProviderQualificationAsync(
                new AddQualificationViewModel
                {
                    ProviderVenueId = viewModel.ProviderVenueId,
                    QualificationId = qualificationId,
                    Source = viewModel.Source
                });

            return RedirectToRoute("GetProviderVenueDetail",
                new { providerVenueId = viewModel.ProviderVenueId });
        }

        [Route("remove-qualification/{providerVenueId}/{qualificationId}", Name = "RemoveQualification")]
        public async Task<IActionResult> RemoveQualificationAsync(int providerVenueId, int qualificationId)
        {
            await _providerQualificationService.RemoveProviderQualificationAsync(providerVenueId, qualificationId);

            return RedirectToRoute("GetProviderVenueDetail", new { providerVenueId });
        }

        private async Task<IList<RouteSummaryViewModel>> GetRoutesAsync(MissingQualificationViewModel viewModel = null)
        {
            var routes = await _routePathService.GetRouteSummaryAsync();

            foreach (var route in routes)
            {
                if (viewModel?.Routes.Any(r => r.Id == route.Id && r.IsSelected) == true)
                {
                    route.IsSelected = true;
                }
            }

            return routes;
        }

        private async Task PopulateRoutesForQualificationSearchItem(QualificationSearchViewModel searchResult)
        {
            var routes = await _routePathService.GetRouteSummaryAsync();

            foreach (var searchResultItem in searchResult.Results)
            {
                searchResultItem.Routes = routes.Select(route =>
                    new RouteSummaryViewModel
                    {
                        Id = route.Id,
                        IsSelected = searchResultItem.RouteIds.Contains(route.Id),
                        Name = route.Name,
                        Summary = route.Summary
                    }).ToList();
            }
        }

        private void Validate(QualificationViewModelBase viewModel)
        {
            if (string.IsNullOrWhiteSpace(viewModel.ShortTitle) || viewModel.ShortTitle.Length > 100)
            {
                ModelState.AddModelError("ShortTitle", "You must enter a short title that is 100 characters or fewer");
            }

            if (viewModel.Routes == null || !viewModel.Routes.Any(r => r.IsSelected))
            {
                ModelState.AddModelError("Routes", "You must choose a skill area for this qualification");
            }
        }
    }
}