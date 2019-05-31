// ReSharper disable RedundantUsingDirective
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
using Sfa.Tl.Matching.Models.Dto;
using Sfa.Tl.Matching.Models.ViewModel;

namespace Sfa.Tl.Matching.Web.Controllers
{
#if !NoAuth
    [Authorize(Roles = RolesExtensions.AdminUser)]
#endif
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
        public async Task<IActionResult> SearchProvider()
        {
            var model = await SearchProvidersWithFundingAsync(new ProviderSearchParametersViewModel());

            model.SearchParameters.ShowAllProvider = false;

            return View(model);
        }

        [HttpPost]
        [Route("search-ukprn", Name = "SearchProviderByUkPrn")]
        public async Task<IActionResult> SearchProvider(ProviderSearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(GetProviderSearchViewModel(viewModel));

            if (!viewModel.UkPrn.HasValue)
            {
                ModelState.AddModelError(nameof(ProviderSearchParametersViewModel.UkPrn),
                    "You must enter a UKPRN");
                return View(GetProviderSearchViewModel(viewModel));
            }

            var searchResult = await _providerService.SearchAsync(viewModel.UkPrn.Value);
            if (IsValidProviderSearch(searchResult))
                return View(await SearchProvidersWithFundingAsync(viewModel));

            searchResult = await _providerService.SearchReferenceDataAsync(viewModel.UkPrn.Value);

            return View(IsValidProviderSearch(searchResult) ?
                GetProviderSearchUkRlpViewModel(viewModel, searchResult) :
                new ProviderSearchViewModel(viewModel));
        }

        [HttpPost]
        [Route("add-provider", Name = "AddProvider")]
        public IActionResult AddProvider(AddProviderViewModel viewModel)
        {
            return RedirectToRoute("CreateProviderDetail", new
            {
                ukPrn = viewModel.UkPrn,
                name = viewModel.Name
            });
        }

        [HttpGet]
        [Route("create-provider/{ukPrn}/{name}", Name = "AddProviderDetail")]
        public IActionResult ProviderDetail(AddProviderViewModel viewModel)
        {
            return View(new ProviderDetailViewModel
            {
                Name = viewModel.Name,
                UkPrn = viewModel.UkPrn
            });
        }

        [HttpPost]
        [Route("create-provider/{ukPrn}/{name}", Name = "CreateProviderDetail")]
        public async Task<IActionResult> CreateProviderDetail(CreateProviderDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(nameof(ProviderDetail), viewModel);

            if (viewModel.IsSaveAndAddVenue)
            {
                var providerId = await _providerService.CreateProvider(viewModel);
                return RedirectToRoute("AddVenue", new
                {
                    providerId
                });
            }

            if (!viewModel.IsCdfProvider)
                return RedirectToAction(nameof(SearchProvider));

            return RedirectToAction(nameof(ProviderDetail), new AddProviderViewModel
            {
                UkPrn = viewModel.UkPrn,
                Name = viewModel.Name
            });
        }

        [HttpGet]
        [Route("provider-overview/{providerId}", Name = "GetProviderDetail")]
        public async Task<IActionResult> ProviderDetail(int providerId)
        {
            var viewModel = new ProviderDetailViewModel();

            if (providerId > 0)
                viewModel = await _providerService.GetProviderDetailByIdAsync(providerId);

            return View(viewModel);
        }

        [HttpPost]
        [Route("provider-overview/{providerId}", Name = "SaveProviderDetail")]
        public async Task<IActionResult> SaveProviderDetail(ProviderDetailViewModel viewModel)
        {
            if (viewModel.IsSaveSection)
                return await PerformSaveSection(viewModel);

            return await SaveProvider(viewModel);
        }

        private async Task<IActionResult> SaveProvider(ProviderDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return View(nameof(ProviderDetail), viewModel);

            if (!viewModel.IsSaveAndAddVenue)
                return await PerformSaveAndFinish(viewModel);

            var providerId = viewModel.Id;
            await _providerService.UpdateProviderDetail(viewModel);

            return RedirectToRoute("AddVenue", new
            {
                providerId
            });
        }

        private async Task<IActionResult> PerformSaveSection(ProviderDetailViewModel viewModel)
        {
            await _providerService.UpdateProviderDetailSectionAsync(viewModel);
            return RedirectToAction(nameof(ProviderDetail), viewModel.Id);
        }

        private async Task<IActionResult> PerformSaveAndFinish(ProviderDetailViewModel viewModel)
        {
            if (!viewModel.ProviderVenues.Any())
            {
                ModelState.AddModelError(nameof(ProviderVenue), "You must add a venue for this provider");
                return View(nameof(ProviderDetail), viewModel);
            }

            await _providerService.UpdateProviderDetail(viewModel);

            return RedirectToAction(nameof(SearchProvider));
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
                ProviderId = dto.Id,
                ProviderName = dto.Name,
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