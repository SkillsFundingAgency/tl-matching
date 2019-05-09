using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Models.ViewModel;

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
        public async Task<IActionResult> SearchProvider()
        {
            return View(await SearchProvidersWithFundingAsync(new ProviderSearchParametersViewModel()));
        }
        
        [HttpPost]
        [Route("search-ukprn", Name = "SearchProviderByUkPrn")]
        public async Task<IActionResult> SearchProvider(ProviderSearchParametersViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(GetProviderSearchViewModel(viewModel));
            }

            var searchResult = viewModel.UkPrn.HasValue
                ? await _providerService.SearchAsync(viewModel.UkPrn.Value)
                : null;

            if (searchResult == null || searchResult.Id == 0)
            {
                ModelState.AddModelError(nameof(ProviderSearchParametersViewModel.UkPrn), "You must enter a real UKPRN");
                return View(GetProviderSearchViewModel(viewModel));
            }

            var resultsViewModel = await SearchProvidersWithFundingAsync(viewModel);

            return View(resultsViewModel);
        }

        [HttpGet]
        [Route("provider-overview/{providerId}", Name = "GetProviderDetail")]
        public async Task<IActionResult> ProviderDetail(int providerId)
        {
            var viewModel = new ProviderDetailViewModel();

            if (providerId > 0)
            {
                viewModel = await _providerService.GetProviderDetailByIdAsync(providerId);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Route("provider-overview", Name = "SaveProviderDetail")]
        public async Task<IActionResult> SaveProviderDetail(ProviderDetailViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.ProviderVenue = await _providerService.GetProviderVenueSummaryByProviderIdAsync(viewModel.Id);

                return View(nameof(ProviderDetail), viewModel);
            }

            if (viewModel.Id > 0)
            {
                await _providerService.UpdateProviderDetail(viewModel);
            }
            else
            {
                viewModel.Id = await _providerService.CreateProvider(viewModel);
            }

            return await ReturnView(viewModel);
        }

        private async Task<IActionResult> ReturnView(ProviderDetailViewModel viewModel)
        {
            viewModel.ProviderVenue = await _providerService.GetProviderVenueSummaryByProviderIdAsync(viewModel.Id);

            if (!string.IsNullOrWhiteSpace(viewModel.SubmitAction) && string.Equals(viewModel.SubmitAction, "SaveAndFinish", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!viewModel.ProviderVenue.Any())
                {
                    ModelState.AddModelError("ProviderVenue", "You must add a venue for this provider");
                    return View(nameof(ProviderDetail), viewModel);
                }

                return View(nameof(SearchProvider), await SearchProvidersWithFundingAsync(new ProviderSearchParametersViewModel()));
            }

            return View(nameof(ProviderDetail), viewModel);
        }

        [HttpGet]
        [Route("hide-unhide-provider/{providerId}", Name = "GetConfirmProviderChange")]
        public async Task<IActionResult> ConfirmProviderChange(int providerId)
        {
            var viewModel = await _providerService.GetHideProviderViewModelAsync(providerId);
            return View(viewModel);
        }

        [HttpPost]
        [Route("hide-unhide-provider/{providerId}", Name = "ConfirmProviderChange")]
        public async Task<IActionResult> ConfirmProviderChange(HideProviderViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("ConfirmProviderChange", viewModel);
            }

            viewModel.IsCdfProvider = !viewModel.IsCdfProvider;
            await _providerService.UpdateProviderAsync(viewModel);

            return RedirectToRoute("GetProviderDetail", new { providerId = viewModel.ProviderId });
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
            return new ProviderSearchViewModel(searchParametersViewModel)
            {
                IsAuthorisedUser = HttpContext.User.IsAuthorisedAdminUser(_configuration.AuthorisedAdminUserEmail)
            };
        }
    }
}