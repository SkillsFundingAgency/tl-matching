using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Configuration;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;
using Sfa.Tl.Matching.Web.Filters;

namespace Sfa.Tl.Matching.Web.Controllers
{
    [Authorize(Roles = RolesExtensions.AdminUser)]
    public class ProviderController : Controller
    {
        private readonly MatchingConfiguration _configuration;
        private readonly IProviderService _providerService;

        public ProviderController(IProviderService providerService, MatchingConfiguration configuration)
        {
            _providerService = providerService;
            _configuration = configuration;
        }

        [HttpGet]
        [Route("search-ukprn", Name = "SearchProvider")]
        public async Task<IActionResult> SearchProviderAsync()
        {
            var model = await SearchProvidersWithFundingAsync(new ProviderSearchParametersViewModel());

            model.SearchParameters.ShowAllProvider = false;

            return View("SearchProvider", model);
        }

        [HttpPost]
        [Route("search-ukprn", Name = "SearchProviderByUkPrn")]
        public async Task<IActionResult> SearchProviderByUkPrnAsync(ProviderSearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("SearchProvider", GetProviderSearchViewModel(viewModel));

            if (!viewModel.UkPrn.HasValue)
            {
                ModelState.AddModelError(nameof(ProviderSearchParametersViewModel.UkPrn),
                    "You must enter a UKPRN");
                return View("SearchProvider", GetProviderSearchViewModel(viewModel));
            }

            var searchResult = await _providerService.SearchAsync(viewModel.UkPrn.Value);
            if (IsValidProviderSearch(searchResult))
                return View("SearchProvider", await SearchProvidersWithFundingAsync(viewModel));

            searchResult = await _providerService.SearchReferenceDataAsync(viewModel.UkPrn.Value);

            return View("SearchProvider", IsValidProviderSearch(searchResult) ?
                GetProviderSearchUkRlpViewModel(viewModel, searchResult) :
                GetProviderSearchViewModel(viewModel));
        }

        [HttpPost]
        [Route("add-provider", Name = "AddProvider")]
        public IActionResult AddProvider(AddProviderViewModel viewModel)
        {
            return RedirectToRoute("AddProviderDetail", new
            {
                ukPrn = viewModel.UkPrn,
                name = viewModel.Name
            });
        }

        [HttpGet]
        [Route("create-provider/{ukPrn}/{name}", Name = "AddProviderDetail")]
        public IActionResult AddProviderDetail(AddProviderViewModel viewModel)
        {
            return View("ProviderDetail", new ProviderDetailViewModel
            {
                Name = viewModel.Name,
                DisplayName = viewModel.Name.ToTitleCase(),
                UkPrn = viewModel.UkPrn
            });
        }

        [HttpPost]
        [Route("create-provider/{ukPrn}/{name}", Name = "CreateProviderDetail")]
        public async Task<IActionResult> CreateProviderDetailAsync(CreateProviderDetailViewModel viewModel)
        {
            if (viewModel.IsSaveSection)
                return await PerformSaveSection(viewModel);

            if (!ModelState.IsValid)
                return View("ProviderDetail", viewModel);

            if (viewModel.IsSaveAndAddVenue)
            {
                var providerId = await _providerService.CreateProviderAsync(viewModel);
                return RedirectToRoute("AddProviderVenue", new
                {
                    providerId
                });
            }

            if (!viewModel.IsCdfProvider)
                return RedirectToAction(nameof(SearchProviderByUkPrnAsync));

            return RedirectToAction(nameof(AddProviderDetail), new AddProviderViewModel
            {
                UkPrn = viewModel.UkPrn,
                Name = viewModel.Name
            });
        }

        [HttpGet]
        [Route("provider-overview/{providerId}", Name = "GetProviderDetail")]
        public async Task<IActionResult> GetProviderDetailAsync(int providerId)
        {
            var viewModel = new ProviderDetailViewModel();

            if (providerId > 0)
                viewModel = await _providerService.GetProviderDetailByIdAsync(providerId);

            return View("ProviderDetail", viewModel);
        }

        [HttpPost]
        [Route("provider-overview/{providerId}", Name = "SaveProviderDetail")]
        public async Task<IActionResult> SaveProviderDetailAsync(ProviderDetailViewModel viewModel)
        {
            if (viewModel.IsSaveSection)
                return await PerformSaveSection(viewModel);

            return await SaveProvider(viewModel);
        }

        private async Task<IActionResult> SaveProvider(ProviderDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View("ProviderDetail", viewModel);

            if (!viewModel.IsSaveAndAddVenue)
                return await PerformSaveAndFinish(viewModel);

            var providerId = viewModel.Id;
            await _providerService.UpdateProviderDetailAsync(viewModel);

            return RedirectToRoute("AddProviderVenue", new
            {
                providerId
            });
        }

        private async Task<IActionResult> PerformSaveSection(ProviderDetailViewModel viewModel)
        {
            var isNew = viewModel.Id == 0;
            if (!isNew)
                await _providerService.UpdateProviderDetailSectionAsync(viewModel);

            if (!viewModel.IsCdfProvider)
                return RedirectToRoute("SearchProvider");

            if (isNew)
                return RedirectToAction(nameof(AddProviderDetail), new AddProviderViewModel
                {
                    UkPrn = viewModel.UkPrn,
                    Name = viewModel.Name
                });

            return RedirectToAction(nameof(AddProviderDetail), viewModel.Id);
        }

        private async Task<IActionResult> PerformSaveAndFinish(ProviderDetailViewModel viewModel)
        {
            if (!viewModel.ProviderVenues.Any())
            {
                ModelState.AddModelError(nameof(ProviderVenue), "You must add a venue for this provider");
                return View("ProviderDetail", viewModel);
            }

            await _providerService.UpdateProviderDetailAsync(viewModel);

            return RedirectToAction(nameof(SearchProviderByUkPrnAsync));
        }

        private async Task<ProviderSearchViewModel> SearchProvidersWithFundingAsync(ProviderSearchParametersViewModel viewModel)
        {
            var resultsViewModel = GetProviderSearchViewModel(viewModel);
            resultsViewModel.SearchResults = new ProviderSearchResultsViewModel
            {
                Results = await _providerService.SearchProvidersWithFundingAsync(viewModel)
            };

            return resultsViewModel;
        }

        private ProviderSearchViewModel GetProviderSearchViewModel(ProviderSearchParametersViewModel searchParametersViewModel)
        {
            searchParametersViewModel.ShowAllProvider = true;

            return new ProviderSearchViewModel(searchParametersViewModel)
            {
                IsAuthorisedUser = HttpContext.User.IsAuthorisedAdminUser(_configuration.AuthorisedAdminUserEmail)
            };
        }

        private ProviderSearchViewModel GetProviderSearchUkRlpViewModel(ProviderSearchParametersViewModel searchParametersViewModel, ProviderSearchResultDto dto)
        {
            var viewModel = new ProviderSearchViewModel(searchParametersViewModel)
            {
                SearchResults =
                {
                    IsUkRlp = true
                },
                IsAuthorisedUser = HttpContext.User.IsAuthorisedAdminUser(_configuration.AuthorisedAdminUserEmail)
            };
            viewModel.SearchResults.Results.Add(new ProviderSearchResultItemViewModel
            {
                Id = dto.Id,
                Name = dto.Name,
                UkPrn = dto.UkPrn
            });

            return viewModel;
        }

        private static bool IsValidProviderSearch(ProviderSearchResultDto searchResult)
        {
            return searchResult != null && searchResult.Id > 0;
        }
    }
}