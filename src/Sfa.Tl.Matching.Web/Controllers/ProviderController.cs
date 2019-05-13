using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sfa.Tl.Matching.Application.Configuration;
using Sfa.Tl.Matching.Application.Extensions;
using Sfa.Tl.Matching.Application.Interfaces;
using Sfa.Tl.Matching.Domain.Models;
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
        [Route("provider-overview/{providerId}", Name = "SaveProviderDetail")]
        public async Task<IActionResult> SaveProviderDetail(ProviderDetailViewModel viewModel)
        {
            if (viewModel.IsSaveSection)
                return await PerformSaveSection(viewModel);

            if (!ModelState.IsValid)
                return View(nameof(ProviderDetail), viewModel);

            if (viewModel.IsSaveAndAddVenue)
                return await PerformSaveAndAddVenue(viewModel);

            return await PerformSaveAndFinish(viewModel);
        }

        private async Task<IActionResult> PerformSaveSection(ProviderDetailViewModel viewModel)
        {
            if (viewModel.IsCdfProvider)
            {
                await _providerService.UpdateProviderDetailSectionAsync(viewModel);
                return View(nameof(ProviderDetail), viewModel);
            }

            var isNew = IsNewProvider(viewModel);

            if (isNew)
                await _providerService.DeleteProviderAsync(viewModel.Id);
            else
                await _providerService.UpdateProviderDetailSectionAsync(viewModel);

            return View(nameof(SearchProvider),
                await SearchProvidersWithFundingAsync(new ProviderSearchParametersViewModel()));
        }

        private async Task<IActionResult> PerformSaveAndAddVenue(ProviderDetailViewModel viewModel)
        {
            await _providerService.UpdateProviderDetail(viewModel);
            return RedirectToRoute("AddVenue", new
            {
                providerId = viewModel.Id
            });
        }

        private async Task<IActionResult> PerformSaveAndFinish(ProviderDetailViewModel viewModel)
        {
            if (!viewModel.ProviderVenue.Any())
            {
                ModelState.AddModelError(nameof(ProviderVenue), "You must add a venue for this provider");
                return View(nameof(ProviderDetail), viewModel);
            }

            await _providerService.UpdateProviderDetail(viewModel);

            return View(nameof(SearchProvider),
                await SearchProvidersWithFundingAsync(new ProviderSearchParametersViewModel()));
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

        private static bool IsNewProvider(ProviderDetailViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.Source))
                return true;

            return false;
        }
    }
}